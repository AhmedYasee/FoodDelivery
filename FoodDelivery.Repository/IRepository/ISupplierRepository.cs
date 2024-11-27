using FoodDelivery.Models;
using System.Collections.Generic;

namespace FoodDelivery.Repository.IRepository
{
    public interface ISupplierRepository
    {
        IEnumerable<Supplier> GetAllSuppliers(); // Retrieve all suppliers
        Supplier GetSupplierById(int id); // Retrieve supplier by ID
        void AddSupplier(Supplier supplier); // Add a new supplier
        void UpdateSupplier(Supplier supplier); // Update an existing supplier
        void DeleteSupplier(int id); // Remove supplier by ID
        bool SaveChanges(); // Save changes to the database
    }
}
