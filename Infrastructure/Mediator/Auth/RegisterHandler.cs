using Core.Mediator.Commands.Auth;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Auth
{
    public sealed class RegisterHandler : IRequestHandler<RegisterCommand, IEnumerable<string>>
    {
        private readonly IAuthService _authService;

        public RegisterHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<IEnumerable<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var errors = await _authService.Register(request.RegisterRequest);

            return errors;
        }
    }
}
