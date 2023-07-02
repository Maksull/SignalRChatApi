namespace Core.Contracts.Controllers.Auth
{
    public sealed record ConfirmResetPasswordRequest(string UserId, string Token, string NewPassword);
}
