using System;
using System.ComponentModel.DataAnnotations;

namespace Amazon.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; } // This is the 'Name' you're referring to

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Booking Date and Time")]
        [DataType(DataType.DateTime)]
        public DateTime DateAndTime { get; set; } // This is the 'BookingDate' you're referring to

        [Required]
        [Display(Name = "Number of People")]
        public int NumberOfPeople { get; set; }

        public string SpecialRequest { get; set; }

        [Required]
        public string Status { get; set; } // e.g., "Pending", "Confirmed", "Canceled"
    }
}
