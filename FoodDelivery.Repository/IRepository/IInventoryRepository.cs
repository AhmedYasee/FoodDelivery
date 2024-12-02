using System.Collections.Generic;

namespace FoodDelivery.Repository.IRepository
{
    public interface IInventoryRepository
    {
        // Fetch KPI data
        object GetKPIData();

        // Fetch stock distribution by category
        List<object> GetStockDistributionByCategory();

        // Fetch stock value trend
        List<object> GetStockValueTrend();

        // Fetch reorder level items
        List<object> GetReorderLevelItems();

        // Fetch warehouse stock comparison
        List<object> GetWarehouseStockComparison();

        // Fetch alerts
        List<object> GetAlerts();
    }
}
