using FoodDelivery.Models;
using FoodDelivery.Repository.Data;
using FoodDelivery.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodDelivery.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Fetch KPI data
        public object GetKPIData()
        {
            var totalStockValue = _context.InventoryLists.Sum(i => i.Quantity * i.Product.CostPrice);
            var totalStockQuantity = _context.InventoryLists.Sum(i => i.Quantity);
            var reorderAlerts = _context.InventoryLists.Count(i => i.Quantity <= i.Product.ReorderLevel);
            var expiringSoon = _context.InventoryLists.Count(i => i.ExpirationDate.HasValue && i.ExpirationDate.Value <= DateTime.Now.AddDays(30));

            return new
            {
                TotalStockValue = totalStockValue,
                TotalStockQuantity = totalStockQuantity,
                ReorderAlerts = reorderAlerts,
                ExpiringSoon = expiringSoon
            };
        }

        // Fetch stock distribution by category
        public List<object> GetStockDistributionByCategory()
        {
            return _context.InventoryLists
                .GroupBy(i => i.Product.Category.Name)
                .Select(g => new
                {
                    CategoryName = g.Key,
                    TotalQuantity = g.Sum(i => i.Quantity)
                })
                .ToList<object>();
        }

        // Fetch stock value trend
        public List<object> GetStockValueTrend()
        {
            return _context.InventoryLists
                .GroupBy(i => i.Product.CreatedDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalValue = g.Sum(i => i.Quantity * i.Product.CostPrice)
                })
                .OrderBy(d => d.Date)
                .ToList<object>();
        }

        // Fetch reorder level items
        public List<object> GetReorderLevelItems()
        {
            return _context.InventoryLists
                .Where(i => i.Quantity <= i.Product.ReorderLevel)
                .Select(i => new
                {
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    ReorderLevel = i.Product.ReorderLevel
                })
                .ToList<object>();
        }

        // Fetch warehouse stock comparison
        public List<object> GetWarehouseStockComparison()
        {
            return _context.InventoryLists
                .GroupBy(i => i.Warehouse.WarehouseName)
                .Select(g => new
                {
                    WarehouseName = g.Key,
                    TotalStock = g.Sum(i => i.Quantity)
                })
                .ToList<object>();
        }

        // Fetch alerts
        public List<object> GetAlerts()
        {
            var lowStockAlerts = _context.InventoryLists
                .Where(i => i.Quantity <= i.Product.ReorderLevel)
                .Select(i => new { Type = "Low Stock", ProductName = i.Product.Name, Quantity = i.Quantity })
                .ToList<object>();

            var expiringBatchAlerts = _context.InventoryLists
                .Where(i => i.ExpirationDate.HasValue && i.ExpirationDate.Value <= DateTime.Now.AddDays(30))
                .Select(i => new { Type = "Expiring Soon", ProductName = i.Product.Name, ExpirationDate = i.ExpirationDate })
                .ToList<object>();

            return lowStockAlerts.Concat(expiringBatchAlerts).ToList();
        }
    }
}
