using Amazon.Models;
using Amazon.Models.ViewModels;
using Amazon.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using System;

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
    }
}
