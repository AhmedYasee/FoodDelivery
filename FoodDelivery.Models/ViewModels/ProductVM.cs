using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FoodDelivery.Models.ViewModels
{
    public class ProductVM
    {
        [ValidateNever]
        public int ProductID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Cost Price")]
        public decimal CostPrice { get; set; }  // Cost Price

        [Required]
        [Display(Name = "Sales Price")]
        public decimal Price { get; set; }  // Sales Price

        public string Description { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Product Type")]
        public int TypeId { get; set; }  // Product Type (Raw Material or Finished Product)

        [Required]
        [Display(Name = "Unit of Measurement")]
        public int UnitOfMeasurementId { get; set; }  // Unit of Measurement

        [Display(Name = "Reorder Level")]
        public int? ReorderLevel { get; set; }  // Reorder Level (optional)

        [Required]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }  // Quantity to add to the inventory

        [Display(Name = "Batch Number")]
        public string BatchNumber { get; set; }  // Batch Number

        [Display(Name = "Expiration Date")]
        [DataType(DataType.Date)]
        public DateTime? ExpirationDate { get; set; }  // Expiration Date (optional)

        [Required]
        public List<IFormFile> files { get; set; }  // Files for image uploads
    }
}
