using Plugin.Core.Models;
using Plugin.Core.Network;
using Plugin.Core.XML;

namespace Server.Auth.Data.Sync.Client
{
    public class ServerCache
    {
        public static void Load(SyncClientPacket C)
        {
            int ServerId = C.ReadD();
            int Count = C.ReadD();
            SChannelModel Server = SChannelXML.GetServer(ServerId);
            if (Server != null)
            {
                Server.LastPlayers = Count;
            }
        }
    }
}
