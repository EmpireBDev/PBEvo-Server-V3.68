using System.Net;

namespace Plugin.Core.Models
{
    public class Synchronize
    {
        public int RemotePort;
        public IPEndPoint Connection;
        public Synchronize(string Host, int Port)
        {
            Connection = new IPEndPoint(IPAddress.Parse(Host), Port);
        }
    }
}
