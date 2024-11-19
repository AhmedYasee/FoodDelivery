using FoodDelivery.Models;
using System.Collections.Generic;

namespace FoodDelivery.Repository.IRepository
{
    public interface IInventoryListRepository
    {
        void Add(InventoryList inventory);
        void Update(InventoryList inventory);
        void Delete(InventoryList inventory);
        InventoryList Get(int id);
        List<InventoryList> GetAll();
        List<InventoryList> GetAllWithIncludes();
        List<InventoryList> GetByProduct(int productId);
        List<InventoryList> GetByWarehouse(int warehouseId);
        void Save();
    }
}
