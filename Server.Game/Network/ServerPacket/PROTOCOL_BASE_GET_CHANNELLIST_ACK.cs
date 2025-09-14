using Plugin.Core.Models;
using Server.Game.Data.Models;
using System.Collections.Generic;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_GET_CHANNELLIST_ACK : GameServerPacket
    {
        private readonly SChannelModel SChannel;
        private readonly List<ChannelModel> Channels;
        public PROTOCOL_BASE_GET_CHANNELLIST_ACK(SChannelModel SChannel, List<ChannelModel> Channels)
        {
            this.SChannel = SChannel;
            this.Channels = Channels;
        }
        public override void Write()
        {
            WriteH(541);
            WriteH(0);
            WriteC(0);
            WriteC((byte)Channels.Count);
            foreach (ChannelModel Channel in Channels)
            {
                WriteH((ushort)Channel.Players.Count);
            }
            WriteH(310);
            WriteH((ushort)SChannel.ChannelPlayers);
            WriteC((byte)Channels.Count);
            WriteC(0);
        }
    }
}