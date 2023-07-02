using Core.Contracts.Controllers.Auth;
using Core.Mediator.Queries.Auth;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.Mediator.Auth
{
    public sealed class GetUserDataHandler : IRequestHandler<GetUserDataQuery, UserResponse?>
    {
        private readonly IAuthService _authService;

        public GetUserDataHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<UserResponse?> Handle(GetUserDataQuery request, CancellationToken cancellationToken)
        {
            var userResponse = await _authService.GetUserData(request.User);

            return userResponse;
        }
    }
}
