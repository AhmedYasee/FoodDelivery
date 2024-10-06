using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Models
{
    internal class Order
    {
        [Key]
        public int OrderID { get; set; }  // Primary Key

        [Required]
        public string CustomerName { get; set; }  // Customer name

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;  // Date when the order was placed

        [Required]
        public decimal TotalAmount { get; set; }  // Total price of the order

        [Required]
        public string Status { get; set; }  // Order status (Pending, Shipped, Delivered, Cancelled)

        public List<OrderItem> OrderItems { get; set; }  // List of items in the order
    }
}
