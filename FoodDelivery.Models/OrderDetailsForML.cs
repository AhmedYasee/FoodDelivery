using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Models
{
    public class OrderDetailsForML
    {
        public float ProductId { get; set; }  // Changed to float
        public float Quantity { get; set; }   // Changed to float
        public DateTime OrderDate { get; set; }
    }
}
