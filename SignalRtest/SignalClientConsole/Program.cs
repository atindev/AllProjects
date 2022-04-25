using System;
using System.Threading.Tasks;

namespace SignalClientConsole
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            OrderService service = new OrderService();
            ////service.OnReceivedMessage += Service_OnReceivedMessage;
            ////service.OnEnteredOrExited += Service_OnReceivedMessage;

            ////var url = @"https://localhost:44371/orderHub";
            ////var url = @"http://localhost:32768/orderHub";
            ////var url = @"https://checkr.azurewebsites.net/orderHub";
            ////var url = @"https://messignalr.azurewebsites.net/mesmessaginghub";
            ////var url = string.Join("/", @"https://messignalrperf.azurewebsites.net", "MesMessagingHub");
            ////var url = string.Join("/", @"https://localhost:44323","mesmessaginghub");

            ////var url = string.Join("/", @"https://usendevmes.westpharma.net:30001/signalrapi", "MesMessagingHub");
            var url = string.Join("/", @"https://usendvvmes.westpharma.net:30001/signalrapi", "MesMessagingHub");
            ////var url = string.Join("/", @"https://inbadvvmes.westpharma.net:30001/signalrapi", "MesMessagingHub");

            ////var url = string.Join("/", @"https://brdirelmes.westpharma.net:30001/signalrapi", "MesMessagingHub");

            service.Init(url);

            await service.ConnectAsync();
            if (service.IsConnected)
            {
                ////string pu = "0271f782-8433-454b-b4fa-ac3f8aba1f79";//PPDLEVE2
                ////string pu = "CC3769E3-298A-4701-9E53-FE86285FB435";//PPDLEVE1
                ////string pu = "2A6E669D-B038-418D-9077-3DF08CA1193F";//PDPSEDA HEAVY1 USENDEV
                ////string pu = "6E857CC4-335C-46F2-88DA-A1F024EF48FF";//mixer75la

                ////string pu = "3A39037D-396C-42F1-A33F-149D7060FFF9";//pack dev GEB001-1
                ////string pu = "3dc6d19f-891d-42c8-9797-9ec5ee7a84cd";//dev bmi011-2

                ////string pu = "340EC096-BEBF-4BA4-9280-640388B2793E";//dvv WDLIGHT3
                ////string pu = "E5E1AE1C-5062-4F82-AC9E-5F24847E3D30";//dvv WDHEAVY1
                ////string pu = "EDA34C87-BAF1-4DF7-A7C2-EBF8A7839F51";//dvv WDLIGHT1
                ////string pu = "116dd86c-314e-46e7-bc3b-f3af7b25f45b";//dvv BVU004
                ////string pu = "B91CA566-BBA4-46A1-A6D9-6314ED929A03";// dvv MIX02

                ////string pu = "EDA34C87-BAF1-4DF7-A7C2-EBF8A7839F51";// dvv WDLIGHT1
                ////string pu = "E5E1AE1C-5062-4F82-AC9E-5F24847E3D30";// dvv WDHEAVY1
                string pu = "68D998F9-5307-456D-8D7B-A91D86ED0393";// USEN dvv WDHEAVY3

                ////string pu = "FC14CA0B-7232-4E9F-A3B0-73B41758B660";//brdi rel 

                ////string pu = "8F86AA5F-3EB8-445A-8E35-A59480EE472B";//usen dvv pri_pack 
                ////string pu = "8a195e42-277b-4ff0-9ce6-38c38fd56a3e";//brdi dvv pri_pack 

                await service.JoinChannelAsync(pu);
                Console.WriteLine("You are connected...");

                var keepGoing = true;
                do
                {
                    PrintMenu();
                    var text = Console.ReadLine();
                    int i = 0;
                    if (string.Equals(text, "exit", StringComparison.InvariantCultureIgnoreCase))
                    {
                        keepGoing = false;
                    }
                    else if (!string.IsNullOrEmpty(text) && Int32.TryParse(text, out i))
                    {
                        try
                        {
                            Console.WriteLine("Enter PED id");
                            var text1 = Console.ReadLine();
                            await service.SendMessageAsync(i, pu, text1);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                while (keepGoing);
            }
            else
            {
                Console.WriteLine("You are not connected...");
            }
        }

        private static void PrintMenu()
        {
            Console.WriteLine("case 1:ProductionUnitStatusUpdated");
            Console.WriteLine("case 2:ProductionUnitAllocate");
            Console.WriteLine("case 3:ProductionUnitDeallocate");
            Console.WriteLine("case 4:PingRoom");
            Console.WriteLine(@"type ""exit"" to close execution");
        }

        //private static void Service_OnReceivedMessage(object sender, MessageEventArgs e)
        //{
        //    Console.WriteLine("Message" + e.Message);
        //}

        //private static void Service_OnEnteredOrExited(object sender, MessageEventArgs e)
        //{
        //    Console.WriteLine("JoinedBy " + e.Message);
        //}
    }
}
