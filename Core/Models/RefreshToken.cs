namespace Core.Models
{
    public sealed record RefreshToken(string Token, DateTime Expired);
}
