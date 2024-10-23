using System;
using System.ComponentModel.DataAnnotations;

public class ValidateProductionDateAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var productionDate = (DateOnly)value;

        // Check if production date is in the future
        if (productionDate > DateOnly.FromDateTime(DateTime.Now))
        {
            return new ValidationResult("Production date cannot be in the future.");
        }

        return ValidationResult.Success;
    }
}
