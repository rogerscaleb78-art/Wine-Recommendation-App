using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GroupProject421_521.Data;
using GroupProject421_521.Models;
using System.Security.Claims;

namespace GroupProject421_521.Controllers
{
    public class UserPromptsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserPromptsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserPrompts (Only Show Non-Deleted Prompts)
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

        // Free Wine Recommendation System
        private async Task<string> GetWineRecommendation(string userPrompt)
        {
            try
            {
                // Simulate API delay for realistic experience
                await Task.Delay(1500);

                var recommendation = AnalyzeUserInput(userPrompt);
                return recommendation;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetWineRecommendation: {ex.Message}");
                return "An error occurred while generating the wine recommendation.";
            }
        }

        private string AnalyzeUserInput(string userInput)
        {
            var input = userInput.ToLower();
            var recommendations = new List<string>();

            // Mood-based recommendations
            if (ContainsAny(input, new[] { "celebrat", "party", "happy", "excit", "cheer" }))
            {
                recommendations.Add("🍾 **Champagne or Prosecco** - Perfect for celebrations! The bubbles and crisp acidity will enhance your joyful mood. Try a Brut Champagne or Italian Prosecco.");
            }
            else if (ContainsAny(input, new[] { "relax", "unwind", "stress", "tired", "calm" }))
            {
                recommendations.Add("🍷 **Pinot Noir** - A gentle, smooth red wine perfect for relaxation. Its soft tannins and cherry notes will help you unwind.");
            }
            else if (ContainsAny(input, new[] { "romantic", "date", "love", "intimate" }))
            {
                recommendations.Add("🌹 **Rosé Wine** - Elegant and romantic, a dry rosé from Provence or a sparkling rosé would be perfect for your special moment.");
            }

            // Weather-based recommendations
            if (ContainsAny(input, new[] { "hot", "summer", "warm", "sunny", "beach" }))
            {
                recommendations.Add("🥂 **Sauvignon Blanc or Pinot Grigio** - Crisp, refreshing white wines perfect for warm weather. They'll cool you down beautifully.");
            }
            else if (ContainsAny(input, new[] { "cold", "winter", "snow", "cozy", "fireplace" }))
            {
                recommendations.Add("🔥 **Cabernet Sauvignon or Malbec** - Rich, full-bodied reds that will warm you up on cold days. Perfect with hearty meals.");
            }

            // Food pairing recommendations
            if (ContainsAny(input, new[] { "steak", "beef", "meat", "bbq", "grill" }))
            {
                recommendations.Add("🥩 **Cabernet Sauvignon or Syrah** - Bold reds that pair perfectly with red meat. The tannins will complement the richness of your meal.");
            }
            else if (ContainsAny(input, new[] { "fish", "seafood", "salmon", "shrimp", "lobster" }))
            {
                recommendations.Add("🐟 **Chardonnay or Pinot Grigio** - Light, crisp whites that won't overpower delicate seafood flavors.");
            }
            else if (ContainsAny(input, new[] { "pasta", "pizza", "italian", "tomato" }))
            {
                recommendations.Add("🍝 **Chianti or Sangiovese** - Italian reds that are perfect with tomato-based dishes and pasta.");
            }
            else if (ContainsAny(input, new[] { "cheese", "appetizer", "snack" }))
            {
                recommendations.Add("🧀 **Riesling or Pinot Noir** - Versatile wines that pair beautifully with various cheeses and appetizers.");
            }

            // Occasion-based recommendations
            if (ContainsAny(input, new[] { "dinner", "meal", "cooking" }))
            {
                recommendations.Add("🍽️ **Consider the main dish** - For red meat, choose a bold red. For fish or chicken, a crisp white or light red works well.");
            }
            else if (ContainsAny(input, new[] { "dessert", "sweet", "chocolate" }))
            {
                recommendations.Add("🍰 **Port or Moscato** - Sweet wines that complement desserts beautifully. A Ruby Port with chocolate is divine!");
            }

            // Default recommendation if no specific context found
            if (recommendations.Count == 0)
            {
                recommendations.Add("🍷 **Pinot Noir** - A versatile, crowd-pleasing wine that works well in most situations. It's smooth, approachable, and pairs with many foods.");
            }

            // Add general wine wisdom
            recommendations.Add("\n💡 **Pro Tip**: Serve red wines slightly below room temperature (60-65°F) and white wines chilled (45-50°F) for the best experience.");

            return string.Join("\n\n", recommendations);
        }

        private bool ContainsAny(string input, string[] keywords)
        {
            return keywords.Any(keyword => input.Contains(keyword));
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> AskGPT([FromBody] dynamic request)
        {
            var startTime = DateTime.UtcNow;
            try
            {
                // Handle both System.Text.Json and Newtonsoft.Json formats
                string prompt = "";
                if (request is System.Text.Json.JsonElement jsonElement)
                {
                    prompt = jsonElement.GetProperty("prompt").GetString() ?? "";
                }
                else
                {
                    // Fallback for Newtonsoft.Json
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
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return Json(new { success = false, error = $"An unexpected error occurred: {ex.Message}" });
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
                    userPrompt.CreatedAt = DateTime.UtcNow; // ✅ Fix missing timestamps

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

        // POST: UserPrompts/Delete/5 (Soft Delete)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userPrompt = await _context.UserPrompt.FindAsync(id);
            if (userPrompt != null)
            {
                userPrompt.IsDeleted = true; // ✅ Soft delete
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
