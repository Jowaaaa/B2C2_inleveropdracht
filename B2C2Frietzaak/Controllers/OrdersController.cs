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

            int cartItemCount = cartItems.Sum(ci => ci.Quantity);


            ViewData["CartItemCount"] = cartItemCount;

            return RedirectToAction("Orders", cartItems);
        }


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

        public async Task<IActionResult> FinalizeOrder([Bind("OrderId,userId,OrderTime,Total,OrderItems")] Order Order)
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
            float total = cartItems.Sum(item => (float)(item.Price));
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

        public IActionResult DeleteFromOrder(int productId)
        {

            List<CartItem> cartItems;

            if (HttpContext.Session.TryGetValue("CartItems", out byte[] cartItemsData))
            {
                cartItems = JsonConvert.DeserializeObject<List<CartItem>>(Encoding.UTF8.GetString(cartItemsData));
            }
            else
            {
                cartItems = new List<CartItem>();
            }

            // Find the item to remove by ProductId
            var itemToRemove = cartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (itemToRemove != null)
            {
                // Remove the item from the cart
                cartItems.Remove(itemToRemove);

                // Update the session with the modified cart
                HttpContext.Session.Set("CartItems", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(cartItems)));
            }

            // Redirect back to the cart page or another appropriate page
            return RedirectToAction("Orders");
        }
        public IActionResult GetCartItemCount()
        {
            List<CartItem> cartItems;

            if (HttpContext.Session.TryGetValue("CartItems", out byte[] cartItemsData))
            {
                // Deserialize objects if items are in the cart
                cartItems = JsonConvert.DeserializeObject<List<CartItem>>(Encoding.UTF8.GetString(cartItemsData));
            }
            else
            {
                // Initialize a new CartItem List
                cartItems = new List<CartItem>();
            }

            int cartItemCount = cartItems.Sum(ci => ci.Quantity);

            return Json(cartItemCount);
        }
    }
}

