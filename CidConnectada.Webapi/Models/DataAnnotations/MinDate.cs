using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

public class MinDate : ValidationAttribute
{
    public MinDate(string datePropertyName)
    {
        _datePropertyName = datePropertyName;
    }

    public string _datePropertyName { get; set; }


    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        DateTime date = Convert.ToDateTime(value);

        PropertyInfo prop = validationContext.ObjectType.GetProperty(validationContext.MemberName);
        bool propHasValue = false;

        if (prop != null)
        {
            object propValue = prop.GetValue(validationContext.ObjectInstance, null);
            propHasValue = propValue != null && !String.IsNullOrEmpty(propValue.ToString());
        }

        var dateProp = validationContext.ObjectInstance.GetType().GetProperty(_datePropertyName);
        var datePropValue = dateProp?.GetValue(validationContext.ObjectInstance, null);
        DateTime minDate = Convert.ToDateTime(datePropValue);

        return minDate <= date ? ValidationResult.Success : new ValidationResult($"A data deve ser maior ou igual a {minDate:dd/MM/yyyy HH:mm:ss}.");
    }
}