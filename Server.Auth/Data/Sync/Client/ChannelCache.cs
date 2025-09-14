using Plugin.Core.Network;
using Server.Auth.Data.Models;
using Server.Auth.Data.XML;

namespace Server.Auth.Data.Sync.Client
{
    public class ChannelCache
    {
        public static void Load(SyncClientPacket C)
        {
            int ServerId = C.ReadD();
            int ChannelId = C.ReadD();
            int CountPlayer = C.ReadD();
            ChannelModel Channel = ChannelsXML.GetChannel(ServerId, ChannelId);
            if (Channel != null)
            {
                Channel.TotalPlayers = CountPlayer;
            }
        }
    }
}
