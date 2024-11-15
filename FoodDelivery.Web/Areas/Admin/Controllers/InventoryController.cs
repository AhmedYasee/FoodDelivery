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
        private readonly IWarehouseRepository _warehouseRepository;

        public InventoryController(IProductRepository productRepository, IWarehouseRepository warehouseRepository)
        {
            _productRepository = productRepository;
            _warehouseRepository = warehouseRepository;
        }

        // Action to render the Inventory List view
        public IActionResult Index()
        {
            // The Index view is located at:
            // D:\Graduation_Project\Food_Delivery\FoodDelivery.Web\Areas\Admin\Views\Modules\Inventory\InventoryList\Index.cshtml
            return View("~/Areas/Admin/Views/Modules/Inventory/InventoryList/Index.cshtml");
        }

        [HttpGet]
        public IActionResult GetInventoryItems()
        {
            var products = _productRepository.GetAll("Category,Type,UnitOfMeasurement,Warehouse")
                                              .Where(p => p.Quantity > 0)
                                              .Select(p => new {
                                                  p.ProductID,
                                                  p.Name,
                                                  TypeName = p.Type != null ? p.Type.TypeName : "N/A",  // Handle null case for Type
                                                  CategoryName = p.Category != null ? p.Category.Name : "N/A",  // Handle null case for Category
                                                  UnitOfMeasurementName = p.UnitOfMeasurement != null ? p.UnitOfMeasurement.UoMName : "N/A",  // Handle null case for UoM
                                                  p.BatchNumber,
                                                  p.Quantity,
                                                  p.ExpirationDate,
                                                  p.ReorderLevel,
                                                  p.ProductImages
                                              }).ToList();

            return Json(new { data = products });
        }


        [HttpGet]
        public IActionResult AddNewItem()
        {
            ViewBag.Products = _productRepository.GetAll().ToList();
            ViewBag.Warehouses = _warehouseRepository.GetAll();

            // The AddNewItem view is located at:
            // D:\Graduation_Project\Food_Delivery\FoodDelivery.Web\Areas\Admin\Views\Modules\Inventory\InventoryList\AddNewItem.cshtml
            return View("~/Areas/Admin/Views/Modules/Inventory/InventoryList/AddNewItem.cshtml");
        }

        [HttpPost]
        public IActionResult AddNewItem(int productId, int warehouseId, int quantity, decimal costPrice, decimal? price, string batchNumber, DateTime? expirationDate)
        {
            var product = _productRepository.Get(productId);
            if (product == null)
            {
                return NotFound();
            }

            product.Quantity += quantity;
            product.CostPrice = costPrice;
            if (price.HasValue)
            {
                product.Price = price.Value;
            }

            product.BatchNumber = batchNumber;
            product.ExpirationDate = expirationDate;
            product.WarehouseId = warehouseId;

            _productRepository.Update(product);
            _productRepository.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}
