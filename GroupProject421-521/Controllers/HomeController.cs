using GroupProject421_521.Data;
using GroupProject421_521.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace GroupProject421_521.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult suggestions()
        {
            return View();
        }
        public async Task<IActionResult> savedsuggestions()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Anonymous";

            if (userId == "Anonymous")
            {
                return View();
            }

            var suggestions = await _context.UserPrompt.Where(x => x.UserId == userId).ToListAsync();
            return View(suggestions);
        }
        public IActionResult AdminPage()
        {
            var users = _context.Users.ToList();

            if (users == null || !users.Any())
            {
                // Log or debug here
                throw new InvalidOperationException("No users found in the database.");
            }

            return View(users);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}