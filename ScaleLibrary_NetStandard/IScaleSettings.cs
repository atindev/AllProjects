using System.Net.Sockets;
using System.Threading.Tasks;

namespace ScaleLibrary_NetStandard
{
    public interface IScaleSettings
    {
        TcpClient Connect(string ipAddress, int port);
        Task<string> ZeroScale();
        Task<string> TareScale();
        //string[] SendCommand(string[] arrCmd);
        Task<string> GetWeight();
        Task<string> ClearScale();
    }
}
