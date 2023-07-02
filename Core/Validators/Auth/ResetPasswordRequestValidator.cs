using Core.Contracts.Controllers.Auth;
using FluentValidation;

namespace Core.Validators.Auth
{
    public sealed class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(r => r.Username).NotEmpty();
        }
    }
}
