using FoodDelivery.Models;
using FoodDelivery.Repository.Data;
using FoodDelivery.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FoodDelivery.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }

        public void Delete(Product product)
        {
            _context.Products.Remove(product);
        }

        public void Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductID == id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
        }

        public Product Get(int id)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Type)  // Include Product Type
                .Include(p => p.UnitOfMeasurement)  // Include Unit of Measurement
                .Include(p => p.ProductImages)
                .FirstOrDefault(p => p.ProductID == id);
        }

        public List<Product> GetFinishedProducts()
        {
            // Fetch products where the type name is "Finished Product", case-insensitive
            return _context.Products
                           .Include(p => p.Category)
                           .Include(p => p.UnitOfMeasurement)
                           .Include(p => p.ProductImages)
                           .Include(p => p.Type)  // Ensure the Type is included
                           .Where(p => p.Type.TypeName.ToLower() == "finished product")  // Filter by type name (case-insensitive)
                           .ToList();
        }

        public List<Product> GetAllProductsInWarehouse(int warehouseId)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.UnitOfMeasurement)
                .Include(p => p.Warehouse)  // Include the warehouse information
                .Where(p => p.WarehouseId == warehouseId)  // Filter by warehouse ID
                .ToList();
        }

        public IQueryable<Product> GetAll()
        {
            return _context.Products
                           .Include(p => p.Category)            // Include Category data
                           .Include(p => p.Type)               // Include Product Type data
                           .Include(p => p.UnitOfMeasurement);  // Include Unit of Measurement data
        }

        // Implement the GetByType method
        public List<Product> GetByType(int typeId, string include = null)
        {
            IQueryable<Product> query = _context.Products.Where(p => p.TypeId == typeId);

            if (include != null)
            {
                var includes = include.Split(',');
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.ToList();
        }

        // Implement the GetByCategory method
        public List<Product> GetByCategory(int categoryId, string include = null)
        {
            IQueryable<Product> query = _context.Products.Where(p => p.CategoryId == categoryId);

            if (include != null)
            {
                var includes = include.Split(',');
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.ToList();
        }

        public List<Product> GetAll(string include = null)
        {
            IQueryable<Product> query = _context.Products;

            if (include != null)
            {
                var includes = include.Split(',');
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.ToList();
        }


        public void Save()
        {
            _context.SaveChanges();
        }
    }

}