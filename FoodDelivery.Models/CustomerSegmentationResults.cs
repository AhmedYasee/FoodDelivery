using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Models
{
    internal class CustomerSegmentationResults
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string CustomerName { get; set; }

        [Required]
        public float TotalRevenue { get; set; }

        [Required]
        public int OrderCount { get; set; }

        [Required]
        public int Recency { get; set; }

        [Required]
        [MaxLength(50)]
        public string Segment { get; set; }

        [Required]
        public float LifetimeValue { get; set; }

        [Required]
        [MaxLength]
        public string ChurnRisk { get; set; }
    }
}
