using System.Diagnostics;
using System.Security.Claims;
using GroupProject421_521.Data;
using GroupProject421_521.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroupProject421_521.Controllers
{
    /// <summary>
    /// Landing page, the "get suggestions" page, the signed-in user's saved
    /// suggestions, and the admin overview.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Home/suggestions
        public IActionResult suggestions()
        {
            return View();
        }

        // GET: /Home/savedsuggestions
        // Shows the current user's past recommendations (empty for anonymous visitors).
        public async Task<IActionResult> savedsuggestions()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return View(new List<UserPrompt>());
            }

            var suggestions = await _context.UserPrompt
                .Where(p => p.UserId == userId && !p.IsDeleted)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(suggestions);
        }

        // GET: /Home/AdminPage
        // Lists all users. Only available to users flagged as admins.
        public async Task<IActionResult> AdminPage()
        {
            var username = User.Identity?.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (currentUser?.IsAdmin != true)
            {
                return Unauthorized();
            }

            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
