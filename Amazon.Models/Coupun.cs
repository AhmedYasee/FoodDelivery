using Amazon.Models.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Models
{
    public class Coupun
    {
        public int CoupunID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public CoupunType Type { get; set; }  // Changed to enum

        [Required]
        [Discount]
        public decimal Discount { get; set; }

        [Required]
        public decimal MinAmount { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }

    public enum CoupunType
    {
        [Display(Name = "Percentage")]
        Percent,

        [Display(Name = "Currency Amount")]
        Currency
    }
}
