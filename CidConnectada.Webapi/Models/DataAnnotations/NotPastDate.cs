using System;
using System.ComponentModel.DataAnnotations;

public class NotPastDate : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
            return ValidationResult.Success;
        
        DateTime date = Convert.ToDateTime(value);
        return date >= DateTime.Now ? ValidationResult.Success : new ValidationResult("A data informada não pode ser anterior a data atual.");
    }
}