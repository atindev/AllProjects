using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalRtest
{
    public class ChatHub : Hub
    {
        public Task SendMessage(string user, string message) => Clients.Client("H1aeATIn").SendAsync("ReceiveMessage", user, message);
    }
}
