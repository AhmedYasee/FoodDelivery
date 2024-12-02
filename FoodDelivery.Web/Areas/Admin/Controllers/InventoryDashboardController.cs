using FoodDelivery.Models.ViewModels;
using FoodDelivery.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodDelivery.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InventoryDashboardController : Controller
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IInventoryListRepository _inventoryListRepository;

        public InventoryDashboardController(
            IBranchRepository branchRepository,
            IWarehouseRepository warehouseRepository,
            IInventoryListRepository inventoryListRepository)
        {
            _branchRepository = branchRepository;
            _warehouseRepository = warehouseRepository;
            _inventoryListRepository = inventoryListRepository;
        }

        public IActionResult Index()
        {
            var branches = _branchRepository.GetAll();
            var warehouses = _warehouseRepository.GetAll();

            var model = new InventoryDashboardViewModel
            {
                Branches = branches,
                Warehouses = warehouses
            };

            return View("~/Areas/Admin/Views/Modules/Inventory/InventoryDashboard/Index.cshtml", model);
        }

        [HttpGet]
        public IActionResult GetDashboardData(string branchId, string warehouseId)
        {
            var selectedWarehouseIds = GetSelectedWarehouseIds(branchId, warehouseId);

            if (!selectedWarehouseIds.Any())
            {
                return Json(new
                {
                    StockValue = "$0.00",
                    StockQuantity = 0,
                    ReorderAlerts = 0,
                    ExpiringSoon = 0,
                    LowStockAlerts = new List<object>(),
                    ExpiringBatchAlerts = new List<object>()
                });
            }

            var stockValue = _inventoryListRepository.GetStockValue(selectedWarehouseIds);
            var stockQuantity = _inventoryListRepository.GetStockQuantity(selectedWarehouseIds);

            var lowStockAlerts = _inventoryListRepository.GetAllWithIncludes()
                .Where(il => selectedWarehouseIds.Contains(il.WarehouseID))
                .GroupBy(il => new { il.ProductID, il.WarehouseID })
                .Where(g => g.Sum(il => il.Quantity) <= (g.First().Product.ReorderLevel ?? 0))
                .Select(g => new
                {
                    ProductName = g.First().Product.Name,
                    BranchName = g.First().Warehouse.Branch?.BranchName ?? "Unknown Branch",
                    WarehouseName = g.First().Warehouse.WarehouseName,
                    TotalQuantity = g.Sum(il => il.Quantity),
                    ReorderLevel = g.First().Product.ReorderLevel ?? 0
                });

            var expiringBatchAlerts = _inventoryListRepository.GetAllWithIncludes()
                .Where(il => selectedWarehouseIds.Contains(il.WarehouseID) &&
                             il.ExpirationDate.HasValue &&
                             il.ExpirationDate <= DateTime.Now.AddDays(30))
                .Select(il => new
                {
                    ProductName = il.Product.Name,
                    BatchNumber = il.BatchNumber,
                    BranchName = il.Warehouse.Branch?.BranchName ?? "Unknown Branch",
                    WarehouseName = il.Warehouse.WarehouseName,
                    ExpirationDate = il.ExpirationDate.Value.ToShortDateString(),
                    Quantity = il.Quantity
                });

            return Json(new
            {
                StockValue = stockValue.ToString("C"),
                StockQuantity = stockQuantity,
                ReorderAlerts = lowStockAlerts.Count(),
                ExpiringSoon = expiringBatchAlerts.Count(),
                LowStockAlerts = lowStockAlerts,
                ExpiringBatchAlerts = expiringBatchAlerts
            });
        }

        [HttpGet]
        public IActionResult GetChartData(string branchId, string warehouseId)
        {
            var selectedWarehouseIds = GetSelectedWarehouseIds(branchId, warehouseId);

            var stockDistribution = _inventoryListRepository.GetAllWithIncludes()
                .Where(il => selectedWarehouseIds.Contains(il.WarehouseID))
                .GroupBy(il => il.Product.Category.Name)
                .Select(g => new
                {
                    Category = g.Key,
                    Quantity = g.Sum(il => il.Quantity)
                }).ToList();

            var warehouseComparison = _inventoryListRepository.GetAllWithIncludes()
                .Where(il => selectedWarehouseIds.Contains(il.WarehouseID))
                .GroupBy(il => il.Warehouse.WarehouseName)
                .Select(g => new
                {
                    Warehouse = g.Key,
                    Quantity = g.Sum(il => il.Quantity)
                }).ToList();

            var reorderLevelItems = _inventoryListRepository.GetAllWithIncludes()
                .Where(il => selectedWarehouseIds.Contains(il.WarehouseID) &&
                             il.Quantity <= (il.Product.ReorderLevel ?? 0))
                .GroupBy(il => il.Product.Name)
                .Select(g => new
                {
                    Product = g.Key,
                    Quantity = g.Sum(il => il.Quantity)
                }).ToList();

            var stockValueTrend = _inventoryListRepository.GetAllWithIncludes()
                .Where(il => selectedWarehouseIds.Contains(il.WarehouseID))
                .GroupBy(il => il.CreatedDate.Date)
                .Select(g => new
                {
                    Date = g.Key.ToString("yyyy-MM-dd"),
                    Value = g.Sum(il => il.Quantity * il.Product.Price)
                }).ToList();

            return Json(new
            {
                StockDistribution = stockDistribution,
                WarehouseComparison = warehouseComparison,
                ReorderLevelItems = reorderLevelItems,
                StockValueTrend = stockValueTrend
            });
        }

        private List<int> GetSelectedWarehouseIds(string branchId, string warehouseId)
        {
            // Handle "null" string or null values
            if (string.IsNullOrEmpty(branchId) || branchId == "null" || branchId == "all")
            {
                // Return all warehouse IDs if branchId is not specified
                return _warehouseRepository.GetAll().Select(w => w.Id).ToList();
            }

            // Parse branchId if valid
            var branchWarehouses = _warehouseRepository.GetByBranch(int.Parse(branchId));
            if (string.IsNullOrEmpty(warehouseId) || warehouseId == "null" || warehouseId == "all")
            {
                // Return all warehouses for the branch
                return branchWarehouses.Select(w => w.Id).ToList();
            }

            // Return a specific warehouse ID
            return new List<int> { int.Parse(warehouseId) };
        }

    }
}
