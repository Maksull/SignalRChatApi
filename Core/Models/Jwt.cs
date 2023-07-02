namespace Core.Models
{
    public sealed record Jwt(string Token, RefreshToken RefreshToken);
}
