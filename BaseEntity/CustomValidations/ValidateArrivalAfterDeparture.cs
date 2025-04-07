using System;
using System.ComponentModel.DataAnnotations;

namespace BaseEntity.CustomValidations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ArrivalAfterDepartureAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            // Get the arrival date value
            if (value is not DateTime arrivalDate)
            {
                return ValidationResult.Success!; // Let RequiredAttribute handle nulls
            }

            // Try to get DepartureDate from different property names
            var departureDateProperty = validationContext.ObjectType.GetProperty("DepartureDate");
            if (departureDateProperty == null)
            {
                return new ValidationResult("Could not find DepartureDate property");
            }

            var departureDateValue = departureDateProperty.GetValue(validationContext.ObjectInstance);
            if (departureDateValue is not DateTime departureDate)
            {
                return ValidationResult.Success!; 
            }

      
            if (arrivalDate.Date < departureDate.Date)
            {
                return new ValidationResult("Arrival date must be after departure date");
            }

            return ValidationResult.Success!;
        }
    }
}