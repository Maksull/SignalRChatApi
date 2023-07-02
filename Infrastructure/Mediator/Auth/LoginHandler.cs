using Core.Mediator.Commands.Auth;
using Core.Models;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Auth
{
    public sealed class LoginHandler : IRequestHandler<LoginCommand, Jwt?>
    {
        private readonly IAuthService _authService;

        public LoginHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Jwt?> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var jwtResponse = await _authService.Login(request.LoginRequest);

            return jwtResponse;
        }
    }
}
