using Core.Contracts.Controllers.Auth;
using Core.Validators.CustomRules;
using FluentValidation;

namespace Core.Validators.Auth
{
    public sealed class ConfirmResetPasswordRequestValidator : AbstractValidator<ConfirmResetPasswordRequest>
    {
        public ConfirmResetPasswordRequestValidator()
        {
            RuleFor(c => c.UserId).NotEmpty();
            RuleFor(c => c.Token).NotEmpty();
            RuleFor(c => c.NewPassword).MatchesPassword();
        }
    }
}
