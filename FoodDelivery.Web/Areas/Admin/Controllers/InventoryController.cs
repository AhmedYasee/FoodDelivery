using FoodDelivery.Models;
using FoodDelivery.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FoodDelivery.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InventoryController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IInventoryListRepository _inventoryListRepository;
        private readonly IWarehouseRepository _warehouseRepository;

        public InventoryController(IProductRepository productRepository, IInventoryListRepository inventoryListRepository, IWarehouseRepository warehouseRepository)
        {
            _productRepository = productRepository;
            _inventoryListRepository = inventoryListRepository;
            _warehouseRepository = warehouseRepository;
        }

        public IActionResult Index()
        {
            return View("~/Areas/Admin/Views/Modules/Inventory/InventoryList/Index.cshtml");
        }

        [HttpGet]
        public IActionResult GetInventoryItems()
        {
            var inventoryItems = _inventoryListRepository.GetAllWithIncludes()
                .Select(i => new
                {
                    i.InventoryListID,
                    ProductName = i.Product.Name,
                    TypeName = i.Product.Type != null ? i.Product.Type.TypeName : "N/A",
                    CategoryName = i.Product.Category != null ? i.Product.Category.Name : "N/A",
                    UnitOfMeasurementName = i.Product.UnitOfMeasurement != null ? i.Product.UnitOfMeasurement.UoMName : "N/A",
                    WarehouseName = i.Warehouse != null ? i.Warehouse.WarehouseName : "N/A",
                    BranchName = i.Warehouse != null && i.Warehouse.Branch != null ? i.Warehouse.Branch.BranchName : "N/A",
                    i.BatchNumber,
                    i.Quantity,
                    i.ExpirationDate,
                    i.Product.ReorderLevel,
                })
                .Where(i => i.Quantity > 0).ToList();

            return Json(new { data = inventoryItems });
        }




        [HttpGet]
        public IActionResult AddNewItem()
        {
            ViewBag.Products = _productRepository.GetAll().ToList();
            ViewBag.Warehouses = _warehouseRepository.GetAll();
            return View("~/Areas/Admin/Views/Modules/Inventory/InventoryList/AddNewItem.cshtml");
        }

        [HttpPost]
        public IActionResult AddNewItem(int productId, int warehouseId, int quantity, string batchNumber, DateTime? expirationDate)
        {
            // Validate inputs
            if (productId <= 0 || warehouseId <= 0 || quantity <= 0)
            {
                return BadRequest("Invalid input data.");
            }

            // Check if a batch of the same product exists in the same warehouse
            var existingInventoryItem = _inventoryListRepository.GetAllWithIncludes()
                .FirstOrDefault(i => i.ProductID == productId
                                     && i.WarehouseID == warehouseId
                                     && i.BatchNumber == batchNumber);

            if (existingInventoryItem != null)
            {
                // Update existing record: Add quantity and update expiration date
                existingInventoryItem.Quantity += quantity;
                if (expirationDate.HasValue)
                {
                    // Update to the nearer expiration date
                    existingInventoryItem.ExpirationDate = existingInventoryItem.ExpirationDate.HasValue
                        ? (existingInventoryItem.ExpirationDate < expirationDate ? existingInventoryItem.ExpirationDate : expirationDate)
                        : expirationDate;
                }

                // Save the changes to the database
                _inventoryListRepository.Update(existingInventoryItem);
            }
            else
            {
                // Create a new record in the inventory list
                var newInventoryItem = new InventoryList
                {
                    ProductID = productId,
                    WarehouseID = warehouseId,
                    Quantity = quantity,
                    BatchNumber = batchNumber,
                    ExpirationDate = expirationDate
                };

                _inventoryListRepository.Add(newInventoryItem);
            }

            // Commit changes to the database
            _inventoryListRepository.Save();

            return RedirectToAction(nameof(Index));
        }


    }
}
