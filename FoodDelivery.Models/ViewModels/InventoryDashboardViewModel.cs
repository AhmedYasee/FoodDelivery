using System.Collections.Generic;

namespace FoodDelivery.Models.ViewModels
{
    public class InventoryDashboardViewModel
    {
        // Filters
        public IEnumerable<Branch> Branches { get; set; } // List of branches for the filter
        public IEnumerable<Warehouse> Warehouses { get; set; } // List of warehouses for the filter

        // Key Performance Indicators (KPIs)
        public decimal StockValue { get; set; } // Total stock value
        public int StockQuantity { get; set; } // Total stock quantity
        public int ReorderAlerts { get; set; } // Number of products below reorder level
        public int ExpiringSoon { get; set; } // Number of batches near expiration

        // Visual Reports
        public IEnumerable<CategoryStock> StockDistributionByCategory { get; set; } // Pie chart data
        public IEnumerable<StockValueTrend> StockValueTrends { get; set; } // Line chart data
        public IEnumerable<ReorderLevelItem> ReorderLevelItems { get; set; } // Bar chart data
        public IEnumerable<WarehouseStockComparison> WarehouseStockComparisons { get; set; } // Bar chart data

        // Alerts and Notifications
        public IEnumerable<LowStockAlert> LowStockAlerts { get; set; }
        public IEnumerable<ExpiringBatchAlert> ExpiringBatchAlerts { get; set; }
    }

    public class CategoryStock
    {
        public string CategoryName { get; set; } // Name of the category
        public int TotalQuantity { get; set; }   // Total quantity in this category
    }

    public class StockValueTrend
    {
        public string Period { get; set; }       // E.g., Month or Date
        public decimal StockValue { get; set; }  // Stock value for this period
    }

    public class ReorderLevelItem
    {
        public string ProductName { get; set; }  // Name of the product
        public int Quantity { get; set; }       // Current quantity
        public int ReorderLevel { get; set; }   // Reorder level
    }

    public class WarehouseStockComparison
    {
        public string WarehouseName { get; set; } // Name of the warehouse
        public int StockQuantity { get; set; }    // Stock quantity in the warehouse
    }

    public class LowStockAlert
    {
        public string ProductName { get; set; }   // Name of the product
        public string WarehouseName { get; set; } // Name of the warehouse
        public int Quantity { get; set; }        // Current quantity
        public int ReorderLevel { get; set; }    // Reorder level
    }

    public class ExpiringBatchAlert
    {
        public string ProductName { get; set; }   // Name of the product
        public string BatchNumber { get; set; }  // Batch number
        public string WarehouseName { get; set; } // Name of the warehouse
        public DateTime ExpirationDate { get; set; } // Expiration date
        public int Quantity { get; set; }        // Current quantity
    }
}
