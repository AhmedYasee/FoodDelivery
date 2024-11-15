using FoodDelivery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Repository.IRepository
{
    public interface IProductRepository
    {
        void Add(Product product);
        void Update(Product product);
        void Delete(Product product);
        void Delete(int productId);
        Product Get(int id);
        List<Product> GetFinishedProducts();
        public List<Product> GetAllProductsInWarehouse(int warehouseId);
        IQueryable<Product> GetAll();
        List<Product> GetAll(string include = null);
        List<Product> GetByType(int typeId, string include = null); // Fetch products by type (e.g., Raw Material or Finished Product)
        List<Product> GetByCategory(int categoryId, string include = null); // Fetch products by category
        void Save();
    }
}
