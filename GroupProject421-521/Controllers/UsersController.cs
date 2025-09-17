using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroupProject421_521.Data;
using GroupProject421_521.Models;
using System.Text.Json; 


namespace GroupProject421_521.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Users user)
        {
            if (ModelState.IsValid)
            {
                // Add user to the database
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // TESTING PURPOSES
                TempData["UserJson"] = JsonSerializer.Serialize(user);

                return RedirectToAction(nameof(SaveToLocalStorage));
            }

            return View(user);
        }

        // Page to Save User to Local Storage
        public IActionResult SaveToLocalStorage()
        {
            return View();
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,Username,Email,PasswordHash,IsAdmin,CreatedAt")] Users users)
        {
            if (id != users.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.UserID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(users);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var users = await _context.Users.FindAsync(id);
            if (users != null)
            {
                _context.Users.Remove(users);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }

        public IActionResult AdminPage()
        {
            var currentCustomUser = _context.Users.Where(x => x.Username == User.Identity.Name);

            if (currentCustomUser != null && currentCustomUser.First().IsAdmin == true)
            {
                // Log or debug here
                var users = _context.Users.ToList();
                return View(users);
            }

            return Unauthorized();

        }

        //public IActionResult AdminPage()
        //{
        //    var currentCustomUser = _context.Users.Where(x => x.Username == User.Identity.Name);

        //    if (currentCustomUser != null && currentCustomUser.First().IsAdmin == true)
        //    {
        //        // Log or debug here
        //        var users = _context.Users.ToList();
        //        return View(users);
        //    }

        //    return Unauthorized();

        //}

        //public IActionResult AdminPage()
        //{
        //    var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        //    var currentCustomUser = _context.Users.FirstOrDefault(x => x.Email == userEmail);


        //    // Ensure that currentCustomUser has at least one record
        //    if (currentCustomUser != null && currentCustomUser.IsAdmin == true)
        //    {
        //        // REPORTS
        //        // Total number of users
        //        var totalUsers = _context.Users.Count();

        //        // Newest User
        //        var newestUser = _context.Users
        //            .OrderByDescending(u => u.CreatedAt)
        //            .FirstOrDefault();

        //        // Oldest User
        //        var oldestUser = _context.Users
        //           .OrderBy(u => u.CreatedAt)
        //           .FirstOrDefault();

        //        // Store values in ViewBag
        //        ViewBag.TotalUsers = totalUsers;
        //        ViewBag.NewestUser = newestUser;
        //        ViewBag.OldestUser = oldestUser;

        //        // Retrieve all users for display
        //        var users = _context.Users.ToList();
        //        return View(users);
        //    }

        //    // If no user found or user is not admin
        //    return Unauthorized();
        //}
    }
}
