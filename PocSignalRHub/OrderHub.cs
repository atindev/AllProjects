using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace PocSignalRHub
{
    public class OrderHub : Hub
    {
        public async Task OrderReady(Guid equimentId)
        {
            try
            {
                await Clients.Group(equimentId.ToString()).SendAsync(Constants.ReceiveMessage, Constants.OrderReady);
            }
            catch (Exception ex)
            {
                throw new HubException("Error", ex);
            }
        }

        public async Task AddToGroup(Guid equimentId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, equimentId.ToString());
            await Clients.Group(equimentId.ToString()).SendAsync(Constants.EnteredOrLeft, $"{Context.ConnectionId} has joined the group {equimentId.ToString()}.");
        }

        public async Task RemoveFromGroup(Guid equimentId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, equimentId.ToString());
            await Clients.Group(equimentId.ToString()).SendAsync(Constants.EnteredOrLeft, $"{Context.ConnectionId} has left the group {equimentId.ToString()}.");
        }

        public async override Task OnConnectedAsync()
        {
            //await Clients.AllExcept(Context.ConnectionId).SendAsync(Constants.EnteredOrLeft, Context.ConnectionId);
            await base.OnConnectedAsync();
        }
    }
}