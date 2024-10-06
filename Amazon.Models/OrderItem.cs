using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Models
{
    internal class OrderItem
    {
        [Key]
        public int OrderItemID { get; set; }  // Primary Key

        [Required]
        public string ProductName { get; set; }  // Product name

        [Required]
        public int Quantity { get; set; }  // Quantity of the product

        [Required]
        public decimal Price { get; set; }  // Price per item

        public int OrderID { get; set; }  // Foreign Key

        public Order Order { get; set; }  // Navigation property to the order
    }
}
