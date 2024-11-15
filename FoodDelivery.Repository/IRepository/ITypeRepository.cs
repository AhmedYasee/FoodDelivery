using FoodDelivery.Models;
using System.Collections.Generic;

namespace FoodDelivery.Repository.IRepository
{
    public interface IProductTypeRepository
    {
        void Add(ProductType type);
        void Update(ProductType type);
        void Remove(int typeId);
        List<ProductType> GetAll();
        ProductType GetById(int id);
        void Save();
    }
}
