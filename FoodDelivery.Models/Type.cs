using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models
{
    public class ProductType
    {
        [Key]
        public int TypeID { get; set; }

        [Required]
        public string TypeName { get; set; }

        [ValidateNever]
        public ICollection<Product> Products { get; set; }
    }
}
