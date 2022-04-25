using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SCALE_ASPNETCORE
{
    public class NetworkScaleSettings
    {
        private TcpClient Scale { get; set; }
        private string IpAddress { get; set; }
        private int Port { get; set; }

        public NetworkScaleSettings(string _ipAddress, int _port)
        {
            IpAddress = _ipAddress;
            Port = _port;
            Connect(IpAddress, Port);
        }

        public TcpClient Connect(string ipAddress, int port)
        {
            if (Scale == null)
            {
                try
                {
                    Scale = new TcpClient(ipAddress, port);
                }
                catch (Exception ex)
                {
                    Scale = null;
                    throw;
                }
            }
            else if (!Scale.Connected)
            {
                Scale.Dispose();
                Scale = new TcpClient(ipAddress, port);
            }
            return Scale;
        }
        public void DisConnect()
        {
            try
            {
                Scale.Close();
            }
            catch
            {
                Scale = null;
            }
            finally
            {
                Scale.Dispose();
                Scale = null;
            }
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
            try
            {
                string result = string.Empty;
                //if (Scale == null || !Scale.Connected)
                //{
                //    Connect(IpAddress, Port);
                //}

                //using (Stream stream = Scale.GetStream())
                //{
                //    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(arrCmd);

                //    StreamWriter s = new StreamWriter(stream);
                //    s.Flush();
                //    s.Write($"{arrCmd }" + '\n');
                //    s.Flush();
                //    //stream.Write(bytesToSend, 0, bytesToSend.Length);

                //    //byte[] readBytes = new byte[Scale.ReceiveBufferSize];
                //    //int bytess = await stream.ReadAsync(readBytes, 0, Scale.ReceiveBufferSize);
                //    //result = (Encoding.ASCII.GetString(readBytes, 0, bytess));
                //    //Thread.Sleep(1000);
                //    //stream = scale.GetStream();
                //    string readlinevalue;
                //    using (StreamReader reader = new StreamReader(stream, Encoding.ASCII))
                //    {
                //        result = reader.ReadLine();
                //    }
                //}
                if (Scale == null || !Scale.Connected)
                {
                    Connect(IpAddress, Port);
                }
                using (NetworkStream stream = Scale.GetStream())
                {
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes($"{arrCmd }" + '\n');
                    stream.Flush();
                    await stream.WriteAsync(bytesToSend, 0, bytesToSend.Length);
                    stream.Flush();

                    byte[] readBytes = new byte[Scale.ReceiveBufferSize];
                    int bytess = await stream.ReadAsync(readBytes, 0, Scale.ReceiveBufferSize);
                    result = (Encoding.ASCII.GetString(readBytes, 0, bytess));
                    result = result.Replace(" ", "");
                    result = result.Substring(2, result.LastIndexOf('g') - 1);
                    //"S S      0.185 kg\r\n"
                }
                DisConnect();
                return result;
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
    }
}
