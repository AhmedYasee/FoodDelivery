using Amazon.Models;
using Amazon.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Amazon.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminBookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminBookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Display all bookings
        public IActionResult Index()
        {
            var bookings = _context.Bookings.ToList();
            return View(bookings);
        }

        // GET: Display booking details
        public IActionResult Details(int id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        // POST: Update the booking status
        [HttpPost]
        public IActionResult UpdateStatus(int id, string status)
        {
            var booking = _context.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }

            booking.Status = status;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Delete a booking
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        // GET: Admin/AdminBooking/Edit/5
        public IActionResult Edit(int id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking); // Show the edit form with booking details
        }

        // POST: Admin/AdminBooking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var bookingInDb = _context.Bookings.Find(id);
                if (bookingInDb == null)
                {
                    return NotFound();
                }

                // Update booking details
                bookingInDb.CustomerName = booking.CustomerName;
                bookingInDb.Email = booking.Email;
                bookingInDb.DateAndTime = booking.DateAndTime;
                bookingInDb.NumberOfPeople = booking.NumberOfPeople;
                bookingInDb.SpecialRequest = booking.SpecialRequest;
                bookingInDb.Status = booking.Status;

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(booking);
        }

    }
}
