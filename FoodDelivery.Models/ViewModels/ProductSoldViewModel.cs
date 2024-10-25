using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Models.ViewModels
{
    public class ProductSoldViewModel
    {
        public string ProductName { get; set; }
        public float TotalQuantity { get; set; } // Change this to float for ML.NET compatibility
        public float Profit { get; set; }

    }
}
