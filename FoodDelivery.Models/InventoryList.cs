using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Models
{
    public class InventoryList
    {
        [Key]
        public int InventoryListID { get; set; }

        // Product reference
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        public Product Product { get; set; }

        // Warehouse reference
        public int WarehouseID { get; set; }
        [ForeignKey("WarehouseID")]
        public Warehouse Warehouse { get; set; }

        // Batch number, expiration date, and quantity
        public string BatchNumber { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int Quantity { get; set; }

        // Timestamps
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
