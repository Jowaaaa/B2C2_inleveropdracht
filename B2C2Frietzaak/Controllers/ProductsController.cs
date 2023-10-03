using B2C2Frietzaak.Data;
using B2C2Frietzaak.Data.Migrations;
using B2C2Frietzaak.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace B2C2Frietzaak.Controllers
{
    public class ProductsController : Controller
    {
        //Dependancy Injection
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public Product? Product { get; set; }


        //Product Overview
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ProductsOverview()
        {

            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            return View(products);
        }


        //Products Add
        public async Task<IActionResult> AddProducts(Product product, IFormFile ImageFile)
        {
            // Handle file upload first
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                var assetDirectory = Path.Combine(_environment.WebRootPath, "Assets");
                var filePath = Path.Combine(assetDirectory, uniqueFileName);

                if (!Directory.Exists(assetDirectory))
                {
                    Directory.CreateDirectory(assetDirectory);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                product.ImageUrl = "Assets/" + uniqueFileName;
                Console.WriteLine("ImageUrl: " + product.ImageUrl);


                if (ModelState.IsValid) //Gave errors because of "Required" annotation in model
                {
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ProductsOverview");
                }
            }
         
            ViewBag.Categories = await _context.Categories.ToListAsync();

            return View(product);
        }



        //Products Edit
        public async Task<IActionResult> EditProducts(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(product);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProducts(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ProductsOverview");
            }
            return View(product);
        }

        //Products Delete
        public async Task<IActionResult> DeleteProducts(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        //Products Delete Confirmation
        [HttpPost, ActionName("DeleteProducts")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'AppDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("ProductsOverview");
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
