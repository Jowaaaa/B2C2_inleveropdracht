using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using B2C2Frietzaak.Data;
using B2C2Frietzaak.Models;

namespace FrietzaakAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaucesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SaucesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Sauces
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sauce>>> GetSauces()
        {
          if (_context.Sauces == null)
          {
              return NotFound();
          }
            return await _context.Sauces.ToListAsync();
        }

        // GET: api/Sauces/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sauce>> GetSauce(int id)
        {
          if (_context.Sauces == null)
          {
              return NotFound();
          }
            var sauce = await _context.Sauces.FindAsync(id);

            if (sauce == null)
            {
                return NotFound();
            }

            return sauce;
        }

        // PUT: api/Sauces/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSauce(int id, Sauce sauce)
        {
            if (id != sauce.SauceId)
            {
                return BadRequest();
            }

            _context.Entry(sauce).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SauceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Sauces
        [HttpPost]
        public async Task<ActionResult<Sauce>> PostSauce(Sauce sauce)
        {
          if (_context.Sauces == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Sauces'  is null.");
          }
            _context.Sauces.Add(sauce);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSauce", new { id = sauce.SauceId }, sauce);
        }

        // DELETE: api/Sauces/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSauce(int id)
        {
            if (_context.Sauces == null)
            {
                return NotFound();
            }
            var sauce = await _context.Sauces.FindAsync(id);
            if (sauce == null)
            {
                return NotFound();
            }

            _context.Sauces.Remove(sauce);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SauceExists(int id)
        {
            return (_context.Sauces?.Any(e => e.SauceId == id)).GetValueOrDefault();
        }
    }
}
