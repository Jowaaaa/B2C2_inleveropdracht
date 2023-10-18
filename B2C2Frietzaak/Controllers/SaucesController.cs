using B2C2Frietzaak.Data;
using B2C2Frietzaak.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace B2C2Frietzaak.Controllers
{
    public class SaucesController : Controller
    {
        //Dependancy Injection
        private readonly ApplicationDbContext _context;

        public SaucesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaucesOverview()
        {
            return _context.Sauces != null ?
                        View(await _context.Sauces.ToListAsync()) :
                        Problem("Entity set 'AppDbContext.Sauce'  is null.");
        }

        public async Task<IActionResult> AddSauce(Sauce sauce)
        {

            if (ModelState.IsValid)
            {
                _context.Add(sauce);
                await _context.SaveChangesAsync();
                return RedirectToAction("SaucesOverview");
            }

            return View(sauce);

        }

        public async Task<IActionResult> EditSauce(int? id)
        {
            if (id == null || _context.Sauces == null)
            {
                return NotFound();
            }

            var sauce = await _context.Sauces.FindAsync(id);
            if (sauce == null)
            {
                return NotFound();
            }

            return View(sauce);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSauce(int id, Sauce sauce)
        {
            if (id != sauce.SauceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sauce);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SauceExists(sauce.SauceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("SaucesOverview");
            }
            return View(sauce);
        }

        //Sauce Delete
        public async Task<IActionResult> DeleteSauce(int? id)
        {
            if (id == null || _context.Sauces == null)
            {
                return NotFound();
            }

            var sauce = await _context.Sauces
                .FirstOrDefaultAsync(m => m.SauceId == id);
            if (sauce == null)
            {
                return NotFound();
            }

            return View(sauce);
        }

        //Sauce Delete Confirmation
        [HttpPost, ActionName("DeleteSauce")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sauces == null)
            {
                return Problem("Entity set 'AppDbContext.Products'  is null.");
            }
            var sauce = await _context.Sauces.FindAsync(id);
            if (sauce != null)
            {
                _context.Sauces.Remove(sauce);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("SaucesOverview");
        }

        private bool SauceExists(int id)
        {
            return (_context.Sauces?.Any(e => e.SauceId == id)).GetValueOrDefault();
        }

    }
}
