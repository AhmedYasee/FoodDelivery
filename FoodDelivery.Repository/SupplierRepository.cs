using FoodDelivery.Models;
using FoodDelivery.Repository.Data;
using FoodDelivery.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FoodDelivery.Repository.Repository
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly ApplicationDbContext _db;

        public SupplierRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Supplier> GetAllSuppliers()
        {
            return _db.Suppliers.ToList();
        }

        public Supplier GetSupplierById(int id)
        {
            return _db.Suppliers.FirstOrDefault(s => s.Id == id);
        }

        public void AddSupplier(Supplier supplier)
        {
            _db.Suppliers.Add(supplier);
        }

        public void UpdateSupplier(Supplier supplier)
        {
            _db.Suppliers.Update(supplier);
        }

        public void DeleteSupplier(int id)
        {
            var supplier = GetSupplierById(id);
            if (supplier != null)
            {
                _db.Suppliers.Remove(supplier);
            }
        }

        public bool SaveChanges()
        {
            return _db.SaveChanges() > 0;
        }
    }
}
