using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SCALE_ASPNETCORE
{
    public class NetworkScaleSettings2
    {
        private IPAddress IpAddress { get; set; }
        private int Port { get; set; }

        public NetworkScaleSettings2(string _ipAddress, int _port)
        {
            IpAddress = IPAddress.Parse(_ipAddress);
            Port = _port;
            //Connect(IpAddress, Port);
        }

        public async Task<string> ZeroScale()
        {
            string arrCmd = "Z";
            return (await SendCommand(arrCmd));
        }
        public async Task<string> TareScale()
        {
            string arrCmd = "T";
            return (await SendCommand(arrCmd));
        }
        public async Task<string> ClearScale()
        {
            string arrCmd = "C";
            return (await SendCommand(arrCmd));
        }

        public async Task<string> GetWeight()
        {
            string arrCmd = "S";
            return (await SendCommand(arrCmd));
        }

        private async Task<string> SendCommand(string arrCmd)
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];

            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPEndPoint remoteEP = new IPEndPoint(IpAddress, port: Port);

                // Create a TCP/IP  socket.  
                using (Socket sender = new Socket(IpAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
                {
                    // Connect the socket to the remote endpoint. Catch any errors.  
                    try
                    {
                        sender.Connect(remoteEP);

                        //Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());

                        // Encode the data string into a byte array.  
                        byte[] msg = Encoding.ASCII.GetBytes($"{arrCmd }" + '\n');

                        // Send the data through the socket.  
                        sender.Send(msg);

                        // Receive the response from the remote device.  
                        int bytesRec = sender.Receive(bytes);
                        string result = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                        // Release the socket.  
                        sender.Shutdown(SocketShutdown.Both);
                        sender.Close();

                        return result;
                    }
                    catch (ArgumentNullException ane)
                    {
                        return "0";
                        //Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                    }
                    catch (SocketException se)
                    {
                        return "0";
                        //Console.WriteLine("SocketException : {0}", se.ToString());
                    }
                    catch (Exception e)
                    {
                        return "0";
                        //Console.WriteLine("Unexpected exception : {0}", e.ToString());
                    }
                    finally
                    {
                        sender.Dispose();
                        sender = null;
                    }
                }
            }
            catch (Exception e)
            {
                return "0";
                //Console.WriteLine(e.ToString());
            }
        }
    }
}
