using System.ComponentModel.DataAnnotations;

namespace Amazon.Models.Attributes
{
    public class DiscountAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Cast the object to the Coupun class
            var coupun = validationContext.ObjectInstance as Coupun;

            if (coupun == null)
            {
                return new ValidationResult("Invalid Coupun object.");
            }

            // Compare enum value CoupunType.Percent, not a string
            if (coupun.Type == CoupunType.Percent)
            {
                // Validate that the discount for percentage is between 1 and 100
                if (coupun.Discount > 100 || coupun.Discount <= 0)
                {
                    return new ValidationResult(ErrorMessage = "Percent should be between 1 and 100.");
                }
            }
            else
            {
                // Validate that the discount is not negative
                if (coupun.Discount <= 0)
                {
                    return new ValidationResult(ErrorMessage = "Discount cannot be negative.");
                }
            }

            // If validation is successful
            return ValidationResult.Success;
        }
    }
}
