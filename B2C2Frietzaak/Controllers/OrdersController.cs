using B2C2Frietzaak.Data;
using B2C2Frietzaak.Data.Migrations;
using B2C2Frietzaak.Models;
using B2C2Frietzaak.Models.ViewModels;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace B2C2Frietzaak.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        //Dependancy Injection
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor? _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        
        public OrdersController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
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
            List<CartItem> cartItems;

            if (HttpContext.Session.TryGetValue("CartItems", out byte[] cartItemsData))
            {
                //deserialize objects if items in Cart
                cartItems = JsonConvert.DeserializeObject<List<CartItem>>(Encoding.UTF8.GetString(cartItemsData));
            }
            else
            {
                //initiaze new CartItem List
                cartItems = new List<CartItem>();
            }
            //Check if item exists in Cart, add one if exists
            var existingCartItem = cartItems.FirstOrDefault(ci => ci.ProductId == item.ProductId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity++;        
                existingCartItem.Price = existingCartItem.Quantity * item.Price;
            }
            else
            {
                //else add items to Cart
                cartItems.Add(new CartItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.Name,
                    Price = item.Price,
                    Quantity = 1
                });
            }
            //Serialize Objects
            HttpContext.Session.Set("CartItems", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(cartItems)));

            return RedirectToAction("Orders", cartItems);
        }


        //if (CartItems == null)
        //{
        //    CartItems = $"{item.Name} - {item.Price}";
        //}
        //TempData.Keep("CartItems");

        //_context.Products.Add(item);
        //_context.SaveChanges(); //check inserted columns 

        //write check to see if product already in cart if so, +1 quantity
        //Make use of ViewData //Sessieobject session variable
        //ViewBag.ProductId = item.ProductId;
        //ViewBag.Name = item.Name;
        //ViewBag.Price = item.Price;
        //ViewBag.ImageUrl = item.ImageUrl;

        //string CartString = JsonConvert.SerializeObject(item);
        //_httpContextAccessor!.HttpContext!.Session.SetString("CartItems", CartString);


        //return RedirectToAction("Orders");


        public IActionResult OrderCheck()
        {
            List<CartItem> cartItems;

            if (HttpContext.Session.TryGetValue("CartItems", out byte[] cartItemsData))
            {
                //deserialize objects if items in Cart
                cartItems = JsonConvert.DeserializeObject<List<CartItem>>(Encoding.UTF8.GetString(cartItemsData));
            }
            else
            {
                //initiaze new CartItem List
                cartItems = new List<CartItem>();
            }

            return View("OrderCheck", cartItems);
        }

        public async Task <IActionResult> FinalizeOrder([Bind("OrderId,userId,OrderTime,Total,OrderItems")] Order Order)
        {
            var userId = _userManager.GetUserId(User);
            var OrderTime = DateTime.Now;
            List<CartItem> cartItems;

            if (HttpContext.Session.TryGetValue("CartItems", out byte[] cartItemsData))
            {
                //deserialize objects if items in Cart
                cartItems = JsonConvert.DeserializeObject<List<CartItem>>(Encoding.UTF8.GetString(cartItemsData));
            }
            else
            {
                return Content("No Cart Available");
            }
            float total = cartItems.Sum(item => (float)(item.Price * item.Quantity));
            var orderItem = cartItems.Select(item => item.ProductName);
            string productNamesString = string.Join(", ", orderItem);

            var order = new Order
            {
                UserId = userId,
                OrderTime = OrderTime,
                Total = total,
                OrderItems = productNamesString,
                
                 
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return View();
        }

        public IActionResult DeleteFromOrder()
        {
            return RedirectToAction("Orders");
        }
    }   
}
        