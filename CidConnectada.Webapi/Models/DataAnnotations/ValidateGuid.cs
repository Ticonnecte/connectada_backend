using System;
using System.ComponentModel.DataAnnotations;

public class ValidateGuid : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        return Guid.TryParse(value.ToString(), out Guid guid) && guid != Guid.Empty ? ValidationResult.Success : new ValidationResult("O GUID informado é inválido ou está vazio");
    }
}