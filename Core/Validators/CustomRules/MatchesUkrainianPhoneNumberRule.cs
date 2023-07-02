using FluentValidation;
using System.Text.RegularExpressions;

namespace Core.Validators.CustomRules
{
    public static class MatchesUkrainianPhoneNumberRule
    {
        public static IRuleBuilderOptions<T, string> MatchesUkrainianPhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(phoneNumber => Regex.IsMatch(phoneNumber, @"^\+380\d{9}$"))
                              .WithMessage("Please enter a valid Ukrainian phone number starting with '+380'.");
        }
    }
}
