using Amazon.Models;
using Amazon.Models.ViewModels;
using Amazon.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Amazon.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CustomerBookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerBookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Booking Form
        public IActionResult Index()
        {
            return View(new BookingViewModel());
        }

        // POST: Submit Booking
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(BookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var booking = new Booking
                {
                    CustomerName = model.Name,
                    Email = model.Email,
                    DateAndTime = model.BookingDate,
                    NumberOfPeople = model.NumberOfPeople,
                    SpecialRequest = model.SpecialRequest,
                    Status = "Pending" // Set default status
                };

                _context.Bookings.Add(booking);
                _context.SaveChanges();

                TempData["success"] = "Booking Successful!";
                return RedirectToAction("Confirmation");
            }
            return View(model);
        }

        // GET: Booking Confirmation
        public IActionResult Confirmation()
        {
            return View();
        }

        // GET: My Bookings (Customer View)
        public IActionResult MyBookings(string status)
        {
            // Get the current customer's email from the User identity
            var customerEmail = User.Identity.Name;

            var bookings = _context.Bookings
                .Where(b => b.Email == customerEmail);

            // Filter based on status if provided
            if (!string.IsNullOrEmpty(status))
            {
                bookings = bookings.Where(b => b.Status == status);
            }

            return View(bookings.ToList());
        }

        // POST: Cancel Booking
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(int id)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.Id == id && b.Email == User.Identity.Name);

            if (booking == null)
            {
                return NotFound();
            }

            if (booking.Status == "Pending")
            {
                booking.Status = "Canceled";
                _context.SaveChanges();
                TempData["success"] = "Your booking has been canceled.";
            }
            else
            {
                TempData["error"] = "Only pending bookings can be canceled.";
            }

            return RedirectToAction("MyBookings");
        }
    }
}
