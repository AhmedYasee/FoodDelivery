using Microsoft.AspNetCore.Mvc;
using System;

namespace FoodDelivery.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ModulesController : Controller
    {
        // This is the method that serves the Modules page.
        public IActionResult Index()
        {
            return View();
        }

        // Location Management Page
        public IActionResult Locations()
        {
            return View("Inventory/Locations/Index");
        }

        // Add Location Page
        public IActionResult AddLocation()
        {
            return View("Inventory/Locations/AddLocation");
        }

        

        // Inventory Dashboard Page
        public IActionResult InventoryDashboard(string branch = "All", string inventory = "All")
        {
            // Setting default values for branch and inventory
            ViewBag.Branch = branch;
            ViewBag.Inventory = inventory;

            // Pass any necessary data for charts, alerts, etc.
            ViewBag.TotalStock = 500; // Placeholder
            ViewBag.LowStockItems = 15; // Placeholder
            ViewBag.ExpiringItems = 8; // Placeholder

            return View("Inventory/InventoryDashboard/Index");
        }

        // Reports Page
        public IActionResult Reports(string reportType = "stock", DateTime? startDate = null, DateTime? endDate = null, string branch = "All", string inventory = "All", string category = "All")
        {
            // Set report type, date range, and filters in ViewBag for the view
            ViewBag.ReportType = reportType;
            ViewBag.StartDate = startDate ?? DateTime.Now.AddMonths(-1); // Default to one month ago
            ViewBag.EndDate = endDate ?? DateTime.Now; // Default to today
            ViewBag.Branch = branch;
            ViewBag.Inventory = inventory;
            ViewBag.Category = category;

            // Placeholder for passing report data to the view
            // This is where you would query your database to get the relevant report data
            ViewBag.ReportData = GetReportData(reportType, startDate, endDate, branch, inventory, category);

            return View("Inventory/Reports/Index");
        }

        // Mock method to get report data (this would normally pull from a database)
        private object GetReportData(string reportType, DateTime? startDate, DateTime? endDate, string branch, string inventory, string category)
        {
            // Replace this with actual logic to retrieve report data based on filters
            switch (reportType.ToLower())
            {
                case "stock":
                    return new[] {
                        new { ItemName = "Milk", CurrentStock = 100, Location = "Freezer", Category = "Dairy", Date = DateTime.Now },
                        new { ItemName = "Beef", CurrentStock = 50, Location = "Cold Storage", Category = "Meat", Date = DateTime.Now }
                    };
                case "movement":
                    return new[] {
                        new { ItemName = "Milk", FromLocation = "Freezer", ToLocation = "Kitchen", QuantityMoved = 20, Date = DateTime.Now },
                        new { ItemName = "Beef", FromLocation = "Cold Storage", ToLocation = "Kitchen", QuantityMoved = 10, Date = DateTime.Now }
                    };
                case "lowstock":
                    return new[] {
                        new { ItemName = "Butter", CurrentStock = 5, ReorderLevel = 10, Location = "Freezer", Category = "Dairy", Date = DateTime.Now }
                    };
                case "expiration":
                    return new[] {
                        new { ItemName = "Yogurt", ExpirationDate = DateTime.Now.AddDays(3), CurrentStock = 30, Location = "Refrigerator", Category = "Dairy" }
                    };
                default:
                    return null;
            }
        }
    }
}
