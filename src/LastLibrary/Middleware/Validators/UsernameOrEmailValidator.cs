using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using LastLibrary.Models.AccountViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LastLibrary.Middleware.Validators
{
    public class UsernameOrEmailAttribute : ValidationAttribute, IClientModelValidator
    {

        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            var errorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            MergeAttribute(context.Attributes, "data-val-cannotbered", ErrorMessage);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var loginAttempt = (LoginViewModel) validationContext.ObjectInstance;

            var emailValidator = new EmailAddressAttribute();
            var regexValidator = @"^[a-zA-Z0-9]*$";

            if (emailValidator.IsValid(loginAttempt.UsernameOrEmail) ||
                Regex.IsMatch(loginAttempt.UsernameOrEmail, regexValidator))
                return ValidationResult.Success;

            return new ValidationResult("Please input a Valid username or Email Address");
        }

        private bool MergeAttribute(
            IDictionary<string, string> attributes,
            string key,
            string value)
        {
            if (attributes.ContainsKey(key))
                return false;
            attributes.Add(key, value);
            return true;
        }
    }
}