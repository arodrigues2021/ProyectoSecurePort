using System;
using System.ComponentModel.DataAnnotations;

namespace SecurePort.Entities.Models.Validacion
{
    public class MaxNumeroAtributo : ValidationAttribute
    {
        private readonly int _numeroMax;

        public MaxNumeroAtributo(int numeroMax)
            : base("{0} longitud invalida")
        {
            _numeroMax = numeroMax;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var longitud = value.ToString();

                if (longitud.Length > _numeroMax)
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }
            }
            return ValidationResult.Success;
        }
      
    }
}