using GroupProject421_521.Data;
using GroupProject421_521.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroupProject421_521.Controllers
{
    /// <summary>
    /// CRUD pages for the wine catalog.
    /// </summary>
    public class WinesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Wines
        public async Task<IActionResult> Index()
        {
            return View(await _context.Wine.ToListAsync());
        }

        // GET: Wines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wine = await _context.Wine
                .FirstOrDefaultAsync(m => m.WineID == id);
            if (wine == null)
            {
                return NotFound();
            }

            return View(wine);
        }

        // GET: Wines/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Wines/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WineID,Name,Type,Region,Description,FlavorProfile,Price")] Wine wine)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wine);
        }

        // GET: Wines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wine = await _context.Wine.FindAsync(id);
            if (wine == null)
            {
                return NotFound();
            }
            return View(wine);
        }

        // POST: Wines/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WineID,Name,Type,Region,Description,FlavorProfile,Price")] Wine wine)
        {
            if (id != wine.WineID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WineExists(wine.WineID))
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
            return View(wine);
        }

        // GET: Wines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wine = await _context.Wine
                .FirstOrDefaultAsync(m => m.WineID == id);
            if (wine == null)
            {
                return NotFound();
            }

            return View(wine);
        }

        // POST: Wines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wine = await _context.Wine.FindAsync(id);
            if (wine != null)
            {
                _context.Wine.Remove(wine);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WineExists(int id)
        {
            return _context.Wine.Any(e => e.WineID == id);
        }
    }
}
