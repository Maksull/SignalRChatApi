using Core.Mediator.Commands.Auth;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Auth
{
    public sealed class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, IEnumerable<string>>
    {
        private readonly IAuthService _authService;

        public ConfirmEmailHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<IEnumerable<string>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var errors = await _authService.ConfirmEmail(request.ConfirmEmailRequest);

            return errors;
        }
    }
}
