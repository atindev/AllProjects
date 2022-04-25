using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace XamTrial
{
    public class Scale
    {
        TcpClient scale;
        IList<string> response;
        string ipAddress;
        int port;
        string result = string.Empty;

        public Scale(string _ipAddress, int _port)
        {
            ipAddress = _ipAddress;
            port = _port;
            Connect(ipAddress, port);
        }

        public TcpClient Connect(string ipAddress, int port)
        {
            if (scale == null)
            {
                scale = new TcpClient(ipAddress, port);
            }
            return scale;
        }
        public void DisConnect()
        {
            try
            {
                scale.Close();
            }
            catch
            {
                scale = null;
            }
        }
        public async Task<string> ZeroScale()
        {
            string arrCmd = "Z";
            string[] response;
            response = await SendCommand(arrCmd);
            string ss = string.Join("", response);
            DisConnect();
            return result;
        }
        public async Task<string> TareScale()
        {
            string arrCmd = "T";
            string[] response;
            response = await SendCommand(arrCmd);
            string ss = string.Join("", response);
            DisConnect();
            return result;
        }
        public async Task<string> ClearScale()
        {
            string arrCmd = "C";
            string[] response;
            response = await SendCommand(arrCmd);
            string ss = string.Join("", response);
            DisConnect();
            return result;
        }
        public async Task<string> GetWeight()
        {
            //ClearScale();
            string arrCmd = "S";
            string[] response;
            response = await SendCommand(arrCmd);
            string ss = string.Join("", response);
            DisConnect();
            return ss;
        }

        public async Task<string[]> SendCommand(string arrCmd)
        {
            if (scale.Connected == false)
                Connect(ipAddress, port);

            NetworkStream stream = scale.GetStream();
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(arrCmd);
            stream.Flush();
            stream.Write(bytesToSend, 0, bytesToSend.Length);

            byte[] readBytes = new byte[scale.ReceiveBufferSize];
            int bytess = await stream.ReadAsync(readBytes, 0, scale.ReceiveBufferSize);
            string s = (Encoding.ASCII.GetString(readBytes, 0, bytess));
            return s.Split(new string[] { "" }, options: System.StringSplitOptions.None);
        }
    }
}
