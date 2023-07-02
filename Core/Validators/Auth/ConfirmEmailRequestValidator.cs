using Core.Contracts.Controllers.Auth;
using FluentValidation;

namespace Core.Validators.Auth
{
    public sealed class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
    {
        public ConfirmEmailRequestValidator()
        {
            RuleFor(c => c.UserId).NotEmpty();
            RuleFor(c => c.Token).NotEmpty();
        }
    }
}
