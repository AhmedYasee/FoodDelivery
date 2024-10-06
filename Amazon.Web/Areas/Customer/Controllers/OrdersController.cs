using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Amazon.Repository.Data;
using Amazon.Models;

namespace Amazon.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all orders for the logged-in user
        public IActionResult CustomerOrders()
        {
            // Get the current user's ID
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            // Fetch all orders belonging to the logged-in user
            var orders = _context.OrderHeaders
                                 .Where(o => o.AppUserId == userId)
                                 .ToList();

            return View(orders); // Pass orders to the view
        }
        public IActionResult Details(int id)
        {
            // Fetch the order header
            var orderHeader = _context.OrderHeaders
                                      .Include(o => o.ApplicationUser)
                                      .Include(o => o.shippingInfo)
                                      .FirstOrDefault(o => o.Id == id);

            // If the order doesn't exist, return a 404 Not Found
            if (orderHeader == null)
            {
                return NotFound();
            }

            // Fetch the order details (the products, quantities, etc.)
            var orderDetails = _context.OrderDetails
                                       .Include(d => d.Product)
                                       .Where(d => d.OrderHeaderId == id)
                                       .ToList();

            // Create a view model to pass the order header and details to the view
            var viewModel = new OrderDetailsViewModel
            {
                OrderHeader = orderHeader,
                OrderDetails = orderDetails
            };

            return View(viewModel);
        }

    }
}
