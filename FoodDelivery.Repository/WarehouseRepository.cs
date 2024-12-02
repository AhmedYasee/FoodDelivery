using FoodDelivery.Models;
using FoodDelivery.Repository.Data;
using FoodDelivery.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace FoodDelivery.Repository
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly ApplicationDbContext _context;

        public WarehouseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Warehouse warehouse)
        {
            _context.Warehouses.Add(warehouse);
        }

        public void Update(Warehouse warehouse)
        {
            _context.Warehouses.Update(warehouse);
        }

        public void Delete(Warehouse warehouse)
        {
            _context.Warehouses.Remove(warehouse);
        }

        public Warehouse Get(int id)
        {
            return _context.Warehouses.FirstOrDefault(w => w.Id == id);
        }

        public List<Warehouse> GetAll()
        {
            return _context.Warehouses.ToList();
        }

        public List<Warehouse> GetByBranch(int branchId)
        {
            return _context.Warehouses.Where(w => w.BranchId == branchId).ToList();
        }


        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
