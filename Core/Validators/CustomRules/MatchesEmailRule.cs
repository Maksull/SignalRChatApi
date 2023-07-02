using FluentValidation;
using System.Text.RegularExpressions;

namespace Core.Validators.CustomRules
{
    public static class MatchesEmailRule
    {
        public static IRuleBuilderOptions<T, string> MatchesEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(email => Regex.IsMatch(email, "^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}$"))
                              .WithMessage("Please enter a valid email.");
        }
    }
}
