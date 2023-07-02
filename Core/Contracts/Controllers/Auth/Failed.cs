namespace Core.Contracts.Controllers.Auth
{
    public sealed record Failed(IEnumerable<string> Errors);
}
