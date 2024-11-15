using FoodDelivery.Models;
using FoodDelivery.Repository.Data;
using FoodDelivery.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace FoodDelivery.Repository
{
    public class ProductTypeRepository : IProductTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(ProductType type)
        {
            _context.ProductTypes.Add(type);
        }

        public List<ProductType> GetAll()
        {
            return _context.ProductTypes.ToList();
        }

        public ProductType GetById(int id)
        {
            return _context.ProductTypes.FirstOrDefault(t => t.TypeID == id);
        }

        public void Remove(int typeId)
        {
            var type = _context.ProductTypes.FirstOrDefault(t => t.TypeID == typeId);
            if (type != null)
                _context.ProductTypes.Remove(type);
        }

        public void Update(ProductType type)
        {
            _context.ProductTypes.Update(type);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
