using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Infrastructure.Hubs
{
    public class NotificationHub : Hub
    {
        public static Dictionary<Guid, string> _ConnectionsMap = new();

        public override async Task OnConnectedAsync()
        {
            var userId = new Guid(Context.GetHttpContext().Request.Query["userId"]);
            _ConnectionsMap[userId] = Context.ConnectionId;
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = new Guid(Context.GetHttpContext().Request.Query["userId"]);
            _ConnectionsMap.Remove(userId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
