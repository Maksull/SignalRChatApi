using Core.Contracts.Controllers.Auth;
using Core.Models;
using MediatR;

namespace Core.Mediator.Commands.Auth
{
    public sealed record LoginCommand(LoginRequest LoginRequest) : IRequest<Jwt?>;
}
