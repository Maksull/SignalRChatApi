using Core.Contracts.Controllers.Auth;
using Core.Validators.CustomRules;
using FluentValidation;

namespace Core.Validators.Auth
{
    public sealed class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(r => r.FirstName).NotEmpty();
            RuleFor(r => r.LastName).NotEmpty();
            RuleFor(r => r.Username).NotEmpty();
            RuleFor(r => r.Email).MatchesEmail();
            RuleFor(r => r.PhoneNumber).MatchesUkrainianPhoneNumber();
            RuleFor(r => r.Password).MatchesPassword();
            RuleFor(r => r.ConfirmPassword).Equal(r => r.Password)
                .WithMessage("'ConfirmPassword' must be equal to password");
        }
    }
}
