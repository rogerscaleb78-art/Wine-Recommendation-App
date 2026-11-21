using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GroupProject421_521.Data;
using GroupProject421_521.Models;
using GroupProject421_521.Services;
using System.Security.Claims;

namespace GroupProject421_521.Controllers
{
    /// <summary>
    /// Handles wine recommendation requests and the history of past prompts.
    /// Deleting a prompt is a soft delete: the row stays but is hidden.
    /// </summary>
    public class UserPromptsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserPromptsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserPrompts (newest first, soft-deleted prompts hidden)
        public async Task<IActionResult> Index()
        {
            var prompts = await _context.UserPrompt
                .Where(p => !p.IsDeleted)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(prompts);
        }

        // GET: UserPrompts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var userPrompt = await _context.UserPrompt
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

            if (userPrompt == null) return NotFound();
            return View(userPrompt);
        }

        // Builds a recommendation for the user's text. The short delay mimics the
        // response time of an external API, which the suggestions page displays.
        private static async Task<string> GetWineRecommendation(string userPrompt)
        {
            try
            {
                await Task.Delay(1500);

                var recommendation = WineRecommender.Recommend(userPrompt);
                return recommendation;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetWineRecommendation: {ex.Message}");
                return "An error occurred while generating the wine recommendation.";
            }
        }

        // POST: UserPrompts/AskGPT
        // Called from the suggestions page with a JSON body: { "prompt": "..." }.
        // Returns the recommendation as JSON and saves it to the user's history.
        [HttpPost]
        public async Task<IActionResult> AskGPT([FromBody] dynamic request)
        {
            var startTime = DateTime.UtcNow;
            try
            {
                // Accept the body whether it was parsed by System.Text.Json or Newtonsoft.Json.
                string prompt = "";
                if (request is System.Text.Json.JsonElement jsonElement)
                {
                    prompt = jsonElement.GetProperty("prompt").GetString() ?? "";
                }
                else
                {
                    prompt = request.prompt?.ToString() ?? "";
                }

                if (string.IsNullOrEmpty(prompt))
                    return Json(new { success = false, error = "Prompt is empty" });

                string apiResponse = await GetWineRecommendation(prompt);
                var elapsedSeconds = (DateTime.UtcNow - startTime).TotalSeconds;

                var userPrompt = new UserPrompt()
                {
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Anonymous",
                    PromptText = prompt,
                    Preferences = "",
                    WineSuggestion = apiResponse,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Add(userPrompt);
                await _context.SaveChangesAsync();

                return Json(new { success = true, response = apiResponse, timeTaken = elapsedSeconds, timestamp = userPrompt.CreatedAt });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AskGPT failed: {ex}");
                return Json(new { success = false, error = "An unexpected error occurred. Please try again." });
            }
        }

        // POST: UserPrompts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,PromptText,Preferences")] UserPrompt userPrompt)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    userPrompt.WineSuggestion = await GetWineRecommendation(userPrompt.PromptText + " " + userPrompt.Preferences);
                    userPrompt.CreatedAt = DateTime.UtcNow;

                    _context.Add(userPrompt);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Error generating wine recommendation: " + ex.Message);
                }
            }
            return View(userPrompt);
        }

        // GET: UserPrompts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var userPrompt = await _context.UserPrompt.FindAsync(id);
            if (userPrompt == null) return NotFound();

            return View(userPrompt);
        }

        // POST: UserPrompts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,PromptText,Preferences,WineSuggestion")] UserPrompt userPrompt)
        {
            if (id != userPrompt.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userPrompt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserPromptExists(userPrompt.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userPrompt);
        }

        // GET: UserPrompts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var userPrompt = await _context.UserPrompt
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

            if (userPrompt == null) return NotFound();
            return View(userPrompt);
        }

        // POST: UserPrompts/Delete/5 (soft delete)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userPrompt = await _context.UserPrompt.FindAsync(id);
            if (userPrompt != null)
            {
                userPrompt.IsDeleted = true;
                _context.UserPrompt.Update(userPrompt);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserPromptExists(int id)
        {
            return _context.UserPrompt.Any(e => e.Id == id);
        }
    }
}
