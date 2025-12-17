using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

public class RequiredIfPropertyEquals : ValidationAttribute
{
    private readonly string _firstPropName;
    private readonly string _firstPropValue1;
    private readonly string _firstPropValue2;
    private readonly string _secondPropName;
    private readonly string _secondPropValue;
    public RequiredIfPropertyEquals(string firstPropName, string firstPropValue1, string firstPropValue2 = null)
    {
        _firstPropName = firstPropName;
        _firstPropValue1 = firstPropValue1;
        _firstPropValue2 = firstPropValue2;
    }

    public RequiredIfPropertyEquals(string firstPropName, string firstPropValue1, string secondPropName, string secondPropValue)
    {
        _firstPropName = firstPropName;
        _firstPropValue1 = firstPropValue1;
        _secondPropName = secondPropName;
        _secondPropValue = secondPropValue;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        PropertyInfo prop = validationContext.ObjectType.GetProperty(validationContext.MemberName);
        bool propHasValue = false;

        if (prop != null)
        {
            object propValue = prop.GetValue(validationContext.ObjectInstance, null);
            propHasValue = propValue != null && !string.IsNullOrEmpty(propValue.ToString());
        }
        
        if (propHasValue)
            return ValidationResult.Success;

        var firstProperty = validationContext.ObjectInstance.GetType().GetProperty(_firstPropName);
        var firstPropValue = firstProperty?.GetValue(validationContext.ObjectInstance, null);
        bool firstPropIsValid = firstPropValue?.ToString() != _firstPropValue1;
        string errorMessage = $"A propriedade '{prop?.Name}', é obrigatória quando '{_firstPropName}' é igual a '{_firstPropValue1}'";

        if (!string.IsNullOrEmpty(_firstPropValue2))
        {
            errorMessage += $" ou  igual a '{_firstPropValue2}'";

            if (firstPropIsValid)
            {
                firstPropIsValid = firstPropValue?.ToString() != _firstPropValue2;
            }
        }

        bool secondPropIsValid = true;
        if (!String.IsNullOrWhiteSpace(_secondPropName))
        {
            var secondProperty = validationContext.ObjectInstance.GetType().GetProperty(_secondPropName);
            var secondPropertyValue = secondProperty?.GetValue(validationContext.ObjectInstance, null);
            secondPropIsValid = secondPropertyValue?.ToString() != _secondPropValue;
            errorMessage = errorMessage + $" e '{_secondPropName}' é igual a '{_secondPropValue}'";
        }

        return !firstPropIsValid || !secondPropIsValid ?
            new ValidationResult(errorMessage) : ValidationResult.Success;
    }
}