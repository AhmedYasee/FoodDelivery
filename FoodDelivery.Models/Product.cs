using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal CostPrice { get; set; } // Cost Price of the item

        [Required]
        public decimal Price { get; set; } // Sales Price of the item

        public string Description { get; set; }

        // Category reference
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        // Type reference (e.g., Raw Material, Finished Product)
        public int TypeId { get; set; }
        [ForeignKey("TypeId")]
        public ProductType Type { get; set; }

        // Unit of Measurement reference
        public int UnitOfMeasurementId { get; set; }
        [ForeignKey("UnitOfMeasurementId")]
        public UnitOfMeasurement UnitOfMeasurement { get; set; }

        // Reorder level (optional)
        public int? ReorderLevel { get; set; }

        // Relationship to product images
        public ICollection<ProductImages> ProductImages { get; set; } = new HashSet<ProductImages>();

        // Timestamps
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
