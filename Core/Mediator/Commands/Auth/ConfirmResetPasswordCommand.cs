using Core.Contracts.Controllers.Auth;
using MediatR;

namespace Core.Mediator.Commands.Auth
{
    public sealed record ConfirmResetPasswordCommand(ConfirmResetPasswordRequest ConfirmResetPasswordRequest) : IRequest<IEnumerable<string>>;
}
