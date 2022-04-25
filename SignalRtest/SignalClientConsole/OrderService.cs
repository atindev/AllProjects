using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SignalClientConsole
{
    class OrderService
    {
        ////public event EventHandler<MessageEventArgs> OnReceivedMessage;
        ////public event EventHandler<MessageEventArgs> OnEnteredOrExited;
        ////public event EventHandler<MessageEventArgs> OnConnectionClosed;

        HubConnection hubConnection;

        public bool IsConnected { get; set; }
        Dictionary<string, string> ActiveChannels { get; } = new Dictionary<string, string>();

        public void Init(string url)
        {
            hubConnection = new HubConnectionBuilder().WithUrl(url).WithAutomaticReconnect().Build();

            hubConnection.Closed += async (error) =>
            {
                IsConnected = false;
                Console.WriteLine("\r\nCONNECTION LOST\r\n");
                await Task.Delay(5000);
                await ConnectAsync();
            };
            hubConnection.On<string>("ProdUnitDisConnected", (message) =>
            {
                if (string.Equals(message, "ProdUnitConnectedChecklist", StringComparison.InvariantCultureIgnoreCase))
                    Console.WriteLine("ProdUnitDisConnected: " + message);
            });
            hubConnection.On<string>("ProdUnitConnected", (message) =>
            {
                if (string.Equals(message, "ProdUnitConnectedChecklist", StringComparison.InvariantCultureIgnoreCase))
                    Console.WriteLine("ProdUnitConnected: " + message);
            });
            hubConnection.On<string>("ProdUnitUpdateStatus", (message) =>
            {
                ////OnReceivedMessage?.Invoke(this, new MessageEventArgs(message));
                Console.WriteLine("ProdUnitUpdateStatus: " + message);
            });

            hubConnection.On<string>("Allocation", (message) =>
            {
                ////OnReceivedMessage?.Invoke(this, new MessageEventArgs(message));
                Console.WriteLine("Allocation: " + message);
            });

            hubConnection.On<string>("Deallocation", (message) =>
            {
                ////OnReceivedMessage?.Invoke(this, new MessageEventArgs(message));
                Console.WriteLine("Deallocation: " + message);
            });

            hubConnection.On<object>("ServiceCall", (message) =>
            {
                ////OnReceivedMessage?.Invoke(this, new MessageEventArgs(message));
                Console.WriteLine("ServiceCall: " + message);
                var data1 = message.ToString();
                var data = JsonConvert.DeserializeObject<NotificationModel>(data1);
                if (data != null)
                {
                    string messages = $"{data.Message} from {data.UserName} at workstation {data.PuName ?? data.RoomName}";
                    Console.WriteLine(messages);
                }
            });

            ////hubConnection.On<string>("EnteredOrLeft", (message) =>
            ////{
            ////    OnEnteredOrExited?.Invoke(this, new MessageEventArgs(message));
            ////});
        }

        public async Task ConnectAsync()
        {
            try
            {
                if (IsConnected)
                    return;

                await hubConnection.StartAsync();
                IsConnected = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task DisconnectAsync()
        {
            try
            {
                if (!IsConnected)
                    return;

                await hubConnection.StopAsync();

                ActiveChannels.Clear();
                IsConnected = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task JoinChannelAsync(string group)
        {
            try
            {
                if (!IsConnected)
                    return;

                await hubConnection.SendAsync("ConnectToHubGroup", group);
                await hubConnection.SendAsync("AddtoRoom", "Quality");
                await hubConnection.SendAsync("AddtoRoom", "Planning");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public async Task SendMessageAsync(int i, string group, object text1)
        {
            try
            {
                if (!IsConnected)
                    throw new InvalidOperationException("Not connected");

                string strCommand = null;
                switch (i)
                {
                    case 1:
                        strCommand = "ProductionUnitStatusUpdated";
                        text1 = null;
                        break;
                    case 2:
                        strCommand = "ProductionUnitAllocate";
                        break;
                    case 3:
                        strCommand = "ProductionUnitDeallocate";
                        break;
                    case 4:
                        strCommand = "PingRoom";
                        group = "shopfloor";
                        text1 = new NotificationModel()
                        {
                            PuName = "PDLEVE1",
                            RoomName = "Quality",
                            Message = "Please assist at ",
                            UserName = "Singla, Atin"
                        };
                        break;
                }

                if (!string.IsNullOrEmpty(strCommand) && text1 != null)
                    await hubConnection.SendAsync(strCommand, group, text1);
                if (!string.IsNullOrEmpty(strCommand))
                    await hubConnection.SendAsync(strCommand, group);
                else
                    Console.WriteLine("No Command found");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
