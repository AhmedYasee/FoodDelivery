using System;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; } // Unique identifier

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } // Supplier name

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } // Email address

        [Required]
        [Phone]
        [MaxLength(20)]
        public string Phone { get; set; } // Phone number

        [Required]
        [MaxLength(500)]
        public string Address { get; set; } // Complete address

        [MaxLength(50)]
        public string TaxID { get; set; } // VAT number

        public DateTime CreateDate { get; set; } = DateTime.Now; // Record creation date
    }
}
