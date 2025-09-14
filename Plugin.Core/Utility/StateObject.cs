using System.Net;
using System.Net.Sockets;

namespace Plugin.Core.Utility
{
    public class StateObject
    {
        public Socket WorkSocket = null;
        public EndPoint LocalPoint = null;
        public const int BufferSize = (8 * 1024);
        public byte[] Buffer = new byte[BufferSize];
    }
}
