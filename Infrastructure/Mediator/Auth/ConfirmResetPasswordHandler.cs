using Core.Mediator.Commands.Auth;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Auth
{
    public sealed class ConfirmResetPasswordHandler : IRequestHandler<ConfirmResetPasswordCommand, IEnumerable<string>>
    {
        private readonly IAuthService _authService;

        public ConfirmResetPasswordHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<IEnumerable<string>> Handle(ConfirmResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var errors = await _authService.ConfirmResetPassword(request.ConfirmResetPasswordRequest);

            return errors;
        }
    }
}
