using B2C2Frietzaak.Data;
using B2C2Frietzaak.Models;
using B2C2Frietzaak.Models.ViewModels;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace B2C2Frietzaak.Controllers
{
    public class OrdersController : Controller
    {
        //Dependancy Injection
        private readonly ApplicationDbContext _context;
        
        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }
        


        public IActionResult Orders()
        {
            List<Product> products = _context.Products.ToList();


            var viewModel = new ProductsViewModel
            {
                Products = products
            };

            return View(viewModel);
        }

        public IActionResult AddToCart(Product item)
        {  
            
            _context.Products.Add(item);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        
        
    }
}
