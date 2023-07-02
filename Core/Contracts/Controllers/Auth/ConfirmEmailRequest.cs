namespace Core.Contracts.Controllers.Auth
{
    public sealed record ConfirmEmailRequest(string UserId, string Token);
}
