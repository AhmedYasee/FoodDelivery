using FoodDelivery.Models;
using FoodDelivery.Models.ViewModels;
using FoodDelivery.Repository.Data;
using FoodDelivery.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodDelivery.Repository
{
    public class InventoryListRepository : IInventoryListRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryListRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(InventoryList inventory)
        {
            _context.InventoryLists.Add(inventory);
        }

        public void Update(InventoryList inventory)
        {
            _context.InventoryLists.Update(inventory);
        }

        public void Delete(InventoryList inventory)
        {
            _context.InventoryLists.Remove(inventory);
        }

        public InventoryList Get(int id)
        {
            return _context.InventoryLists
                .Include(i => i.Product)
                .Include(i => i.Warehouse)
                .FirstOrDefault(i => i.InventoryListID == id);
        }

        public List<InventoryList> GetAll()
        {
            return _context.InventoryLists
                .Include(i => i.Product)
                .Include(i => i.Warehouse)
                .ToList();
        }

        public List<InventoryList> GetAllWithIncludes()
        {
            return _context.InventoryLists
                .Include(i => i.Product)                         // Include Product
                    .ThenInclude(p => p.Type)                   // Include Product Type
                .Include(i => i.Product.Category)               // Include Product Category
                .Include(i => i.Product.UnitOfMeasurement)      // Include Product Unit of Measurement
                .Include(i => i.Warehouse)                      // Include Warehouse
                    .ThenInclude(w => w.Branch)                 // Include Branch for Warehouse
                .ToList();
        }


        public List<InventoryList> GetByProduct(int productId)
        {
            return _context.InventoryLists
                .Where(i => i.ProductID == productId)
                .Include(i => i.Warehouse)
                .ToList();
        }

        public List<InventoryList> GetByWarehouse(int warehouseId)
        {
            return _context.InventoryLists
                .Where(i => i.WarehouseID == warehouseId)
                .Include(i => i.Product)
                .ToList();
        }

        // New Methods for Dashboard Functionality
        public decimal GetStockValue(IEnumerable<int> warehouseIds)
        {
            return _context.InventoryLists
                          .Where(il => warehouseIds.Contains(il.WarehouseID))
                          .Sum(il => il.Quantity * il.Product.CostPrice);
        }

        public int GetStockQuantity(IEnumerable<int> warehouseIds)
        {
            return _context.InventoryLists
                          .Where(il => warehouseIds.Contains(il.WarehouseID))
                          .Sum(il => il.Quantity);
        }

        public int GetReorderAlertCount(IEnumerable<int> warehouseIds)
        {
            return _context.InventoryLists
                          .Where(il => warehouseIds.Contains(il.WarehouseID) &&
                                       il.Quantity <= il.Product.ReorderLevel)
                          .Count();
        }

        public int GetExpiringSoonCount(IEnumerable<int> warehouseIds, DateTime dateThreshold)
        {
            return _context.InventoryLists
                          .Where(il => warehouseIds.Contains(il.WarehouseID) &&
                                       il.ExpirationDate.HasValue &&
                                       il.ExpirationDate <= dateThreshold)
                          .Count();
        }

        public IEnumerable<CategoryStock> GetStockDistributionByCategory(IEnumerable<int> warehouseIds)
        {
            return _context.InventoryLists
                          .Where(il => warehouseIds.Contains(il.WarehouseID))
                          .GroupBy(il => il.Product.Category.Name)
                          .Select(g => new CategoryStock
                          {
                              CategoryName = g.Key,
                              TotalQuantity = g.Sum(il => il.Quantity)
                          })
                          .ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
