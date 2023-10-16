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


        //Full Overview
        [Route("/bestellen")]
        public IActionResult Orders()
        {
            //Group by Category
            var groupedProducts = _context.Products
                .Where(p => !string.IsNullOrEmpty(p.Name))
                .GroupBy(p => p.Category)
                .Select(group => new GroupedProduct
                {
                    Category = group.Key,
                    Products = group.ToList()
                })
                .ToList();

            var viewModel = new ProductsViewModel
            {
                GroupedProducts = groupedProducts,
                Sauces = _context.Sauces.ToList()
            };

            return View(viewModel);
        }


        //Add to cart method
        public IActionResult AddToCart(int ProductId, float Price, int Quantity, string Name, int SauceId)
        {

            List<CartItem> cartItems;

            if (HttpContext.Session.TryGetValue("CartItems", out byte[] cartItemsData))
            {
                //Deserialize objects if items in cart
                cartItems = JsonConvert.DeserializeObject<List<CartItem>>(Encoding.UTF8.GetString(cartItemsData));
            }
            else
            {
                //Initialize CartItem List
                cartItems = new List<CartItem>();
            }

            //extra check to make new line in Cart if the sauce don't match
            var existingCartItem = cartItems.FirstOrDefault(ci => ci.CartItemId == ProductId && ci.Product.SauceId == SauceId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += Quantity;
                existingCartItem.Product.Price = existingCartItem.Quantity * Price;
            }
            else
            {
                //Get saucename by Id
                var sauceName = _context.Sauces
                            .Where(s => s.SauceId == SauceId)
                            .Select(s => s.SauceName)
                            .FirstOrDefault();



                cartItems.Add(new CartItem
                {
                    CartItemId = ProductId,
                    Quantity = Quantity,
                    Product = new Product
                    {
                        Name = Name,
                        ProductId = ProductId,
                        Price = Price,
                        SauceId = SauceId,
                        Sauce = new Sauce { SauceName = sauceName }

                    }
                });
            }

            //Serialize to set in session - object -> string
            HttpContext.Session.Set("CartItems", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(cartItems)));

            int cartItemCount = cartItems.Sum(ci => ci.Quantity);

            ViewData["CartItemCount"] = cartItemCount;

            return RedirectToAction("Orders", cartItems);
        }




        //button to give overview of full order
        [Route("/bestellingcheck")]
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

        //Finalize order and push order to DB
        [Route("/bestellingsucces")]
        public async Task<IActionResult> FinalizeOrder()
        {
            var userId = _userManager.GetUserId(User);

            List<CartItem> cartItems;

            if (HttpContext.Session.TryGetValue("CartItems", out byte[] cartItemsData))
            {
                //Deserialize objects if items in Cart
                cartItems = JsonConvert.DeserializeObject<List<CartItem>>(Encoding.UTF8.GetString(cartItemsData));
            }
            else
            {
                //Initialize new CartItem List
                cartItems = new List<CartItem>();
            }

            //Order Table
            var order = new Order
            {
                UserId = userId,
                OrderTime = DateTime.Now,
                Total = cartItems.Sum(item => item.Product.Price * item.Quantity),
                OrderItems = new List<OrderItem>(), //Initialize OrderItems collection
                StatusId = 1 //status "in behandeling"
            };

            foreach (var cartItem in cartItems)
            {

                var orderItem = new OrderItem
                {
                    Quantity = cartItem.Quantity,
                    ProductId = cartItem.Product.ProductId,
                    //Set the SauceId for the OrderItem
                    SauceId = cartItem.Product.SauceId,                  
                };

                order.OrderItems.Add(orderItem);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("CartItems");

            return View();
        }



        //Delete method, reversed Add method :)
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

            //Find item by linking product id to the id in the cart -> changed from previous version
            var itemToRemove = cartItems.FirstOrDefault(ci => ci.CartItemId == productId);

            if (itemToRemove != null)
            {

                cartItems.Remove(itemToRemove);

                //Update storage after removal
                HttpContext.Session.Set("CartItems", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(cartItems)));
            }

            return RedirectToAction("Orders");
        }


        //Item Count method used to display the number behind the car in navbar
        public IActionResult GetCartItemCount()
        {
            List<CartItem> cartItems;

            if (HttpContext.Session.TryGetValue("CartItems", out byte[] cartItemsData))
            {
                //Deserialize objects if items in the cart
                cartItems = JsonConvert.DeserializeObject<List<CartItem>>(Encoding.UTF8.GetString(cartItemsData));
            }
            else
            {
                cartItems = new List<CartItem>();
            }

            int cartItemCount = cartItems.Sum(ci => ci.Quantity);

            return Json(cartItemCount);
        }
        [Authorize(Roles = "Admin")]
        //All Orders
        public async Task<IActionResult> AllOrders()
        {

            var userOrders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(s => s.Sauce)
                .Include(st => st.Status)
                .ToListAsync();


            if (userOrders == null)
            {
                return NotFound();
            }
            var Status = _context.Status.ToDictionary(s => s.StatusId, s => s.StatusName);
            var Sauces = _context.Sauces.ToDictionary(s => s.SauceId, s => s.SauceName); //map Sauces to dictionary https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.todictionary?view=net-7.0

            return View(userOrders);
        }

        //Products Edit
        public async Task<IActionResult> UpdateOrder(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                    .Include(u => u.User)
                    .FirstOrDefaultAsync(o => o.OrderId == id);

            if (orders == null)
            {
                return NotFound();
            }

            ViewBag.Status = await _context.Status.ToListAsync();
            return View(orders);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrder(int id, Order orders, string userId)
        {
            if (id != orders.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orders);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(orders.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("AllOrders");
            }
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine($"Property: {modelState.Errors}, Error: {error.ErrorMessage}");
                }
            }

            return View(orders);
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> MyOrders()
        {
            var userId = _userManager.GetUserId(User);

            var userOrders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(s => s.Sauce)
                .Include(st => st.Status)
                .ToListAsync();

            var Sauces = _context.Sauces.ToDictionary(s => s.SauceId, s => s.SauceName);

            return View(userOrders);
        }
    }
}

