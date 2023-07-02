using Core.Contracts.Controllers.Auth;
using Core.Validators.CustomRules;
using FluentValidation;

namespace Core.Validators.Auth
{
    public sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(l => l.Username).NotEmpty();
            RuleFor(l => l.Password).MatchesPassword();
        }
    }
}
