using B2C2Frietzaak.Data;
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
                        Problem("Entity set 'AppDbContext.Products'  is null.");
        }
    }
}
