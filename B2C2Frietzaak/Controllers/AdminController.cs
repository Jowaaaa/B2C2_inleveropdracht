using B2C2Frietzaak.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace B2C2Frietzaak.Controllers

{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole>? roleManager;
        private readonly UserManager<IdentityUser>? userManager;

        public AdminController(RoleManager<IdentityRole>? roleManager, UserManager<IdentityUser>? userManager, ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            _context = context;
        }

        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users);

        }

        public async Task<IActionResult> UserDetails(string? id)
        {
            //string userId = userManager.GetUserId(User);


            var userOrders = _context.Orders
                .Where(o => o.UserId == id)
                .ToList();

            return View(userOrders);
        }
    }
}
