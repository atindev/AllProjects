using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRTesting
{
    public class OrderHub : Hub
    {
        public Task SendMessage(string user, string message) => Clients.All.SendAsync("ReceiveMessage", message);
        public Task Send(string user, string message) => Clients.Users(new List<string> { user }).SendAsync("ReceiveMessage", message);

        public override Task OnConnectedAsync()
        {
            SendMessage(Clients.Caller, "Welcome").Wait();
            return base.OnConnectedAsync();
        }

        private Task SendMessage(IClientProxy caller, string message) => caller.SendAsync("ReceiveMessage", message);
    }
}