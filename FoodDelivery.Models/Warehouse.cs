using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Models
{
    public class Warehouse
    {
        public int Id { get; set; }  // Primary Key
        public string WarehouseName { get; set; }  // Warehouse name, required
        public string Description { get; set; }  // Warehouse description, optional
        public int BranchId { get; set; }  // Foreign Key linking to the Branch
        public virtual Branch Branch { get; set; }  // Navigation property for the branch
    }
}

