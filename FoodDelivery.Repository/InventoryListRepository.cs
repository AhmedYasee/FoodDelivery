using FoodDelivery.Models;
using FoodDelivery.Repository.Data;
using FoodDelivery.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
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
                .Include(i => i.Product)
                .ThenInclude(p => p.Type)
                .Include(i => i.Product.Category)
                .Include(i => i.Product.UnitOfMeasurement)
                .Include(i => i.Warehouse)
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

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
