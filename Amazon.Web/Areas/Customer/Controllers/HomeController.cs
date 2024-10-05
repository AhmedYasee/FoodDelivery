using Amazon.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Amazon.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly ApplicationDbContext _context;

        public HomeController(IProductRepository productRepository, ApplicationDbContext context)
        {
            _productRepo = productRepository;
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> products = _context.Products.Include(p => p.Category).Include(p => p.ProductImages).ToList();
            return View(products);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            if (id != 0)
            {
                Cart cart = new()
                {
                    Product = _context.Products.Include(p => p.Category).Include(p => p.ProductImages).FirstOrDefault(p => p.ProductID == id),
                    ProductID = id,
                    Count = 1,
                };
                return View(cart);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public IActionResult Details(Cart cart)
        {
            if (ModelState.IsValid)
            {
                var claimIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
                cart.ApplicationUserId = claims.Value;
                var cartDb = _context.Carts.Where(c => c.ApplicationUserId == cart.ApplicationUserId && c.ProductID == cart.ProductID).FirstOrDefault();
                if (cartDb is null)
                    _context.Add(cart);
                else
                    cartDb.Count += cart.Count;
                _context.SaveChanges();
                //Set Cart Session
                HttpContext.Session.SetInt32("CartSession", _context.Carts.Where(c => c.ApplicationUserId == claims.Value).ToList().Count);

                var productName = _context.Products.FirstOrDefault(p => p.ProductID == cart.ProductID).Name;
                TempData["success"] = $"{productName} Added Successfully To The Cart";
            }
            return RedirectToAction("Details",cart.ProductID);
        }
    }
}
