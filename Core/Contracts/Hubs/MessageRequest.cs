namespace Core.Contracts.Hubs
{
    public sealed record MessageRequest(string Username, string Message, string Time);
}
