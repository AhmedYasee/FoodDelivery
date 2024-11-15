using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Models
{
    public class UnitOfMeasurement
    {
        [Key]
        public int UoMID { get; set; }

        [Required]
        public string UoMName { get; set; }

        [ValidateNever]
        public ICollection<Product> Products { get; set; }
    }
}
