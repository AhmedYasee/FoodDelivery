using FoodDelivery.Models;
using FoodDelivery.Models.ViewModels;
using System;
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

        // New methods for dashboard
        decimal GetStockValue(IEnumerable<int> warehouseIds);
        int GetStockQuantity(IEnumerable<int> warehouseIds);
        int GetReorderAlertCount(IEnumerable<int> warehouseIds);
        int GetExpiringSoonCount(IEnumerable<int> warehouseIds, DateTime dateThreshold);
        IEnumerable<CategoryStock> GetStockDistributionByCategory(IEnumerable<int> warehouseIds);


        void Save();
    }
}
