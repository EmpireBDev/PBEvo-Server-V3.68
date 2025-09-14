using Plugin.Core.Models;
using Plugin.Core.Network;
using Plugin.Core.XML;

namespace Server.Game.Data.Sync.Client
{
    public class ServerCache
    {
        public static void Load(SyncClientPacket C)
        {
            int ServerId = C.ReadD();
            int Count = C.ReadD();
            SChannelModel Servers = SChannelXML.GetServer(ServerId);
            if (Servers != null)
            {
                Servers.LastPlayers = Count;
            }
        }
    }
}
