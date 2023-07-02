using Core.Contracts.Controllers.Auth;
using MediatR;

namespace Core.Mediator.Commands.Auth
{
    public sealed record ConfirmEmailCommand(ConfirmEmailRequest ConfirmEmailRequest) : IRequest<IEnumerable<string>>;
}
