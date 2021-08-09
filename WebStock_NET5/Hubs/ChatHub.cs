using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStock_NET5.Hubs
{
    public class ChatHub : Hub
    {
        public async Task Broadcast(string user, string message)
        {
            var content = $"{user} 於{DateTime.UtcNow.AddHours(08).ToShortTimeString()}說：{message}";
            await Clients.All.SendAsync("ReceiveMessage", content);//這裡的ReceiveMessage代表是客户端的方法
        }
    }
}
