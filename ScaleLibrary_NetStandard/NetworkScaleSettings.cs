using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ScaleLibrary_NetStandard
{
    public class NetworkScaleSettings : IScaleSettings
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
            string result = string.Empty;
            if (!Scale.Connected)
                Connect(IpAddress, Port);

            using (NetworkStream stream = Scale.GetStream())
            {
                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(arrCmd);
                stream.Flush();
                stream.Write(bytesToSend, 0, bytesToSend.Length);

                byte[] readBytes = new byte[Scale.ReceiveBufferSize];
                int bytess = await stream.ReadAsync(readBytes, 0, Scale.ReceiveBufferSize);
                result = (Encoding.ASCII.GetString(readBytes, 0, bytess));
            }
            DisConnect();
            return result;
        }
    }
}
