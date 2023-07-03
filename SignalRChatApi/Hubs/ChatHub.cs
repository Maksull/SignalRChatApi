using Core.Contracts.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace SignalRChatApi.Hubs
{
    public sealed class ChatHub : Hub
    {
        public async Task SendMessage(MessageRequest message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        [Authorize]
        public async Task SendMessageByName(MessageRequest message, string username)
        {
            var userId = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value.ToString()!;
            await Clients.User(userId).SendAsync(username, message);
        }

        public async Task SendMessageToConnectionId(MessageRequest message, string fromConnectionId, string toConnectionId)
        {
            await Clients.Clients(fromConnectionId, toConnectionId).SendAsync("ReceiveMessage", message);
        }
    }
}
