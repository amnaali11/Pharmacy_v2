using System;
using Pharmacy_v2.Models;
using System.ComponentModel.DataAnnotations;

public class ExpiryDateValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var medicine = (Medicine)validationContext.ObjectInstance;
        var expiryDate = (DateOnly)value;

        if (expiryDate <= medicine.ProductionDate)
        {
            return new ValidationResult("Expiry date must be after the production date.");
        }
        return ValidationResult.Success;
    }
}
