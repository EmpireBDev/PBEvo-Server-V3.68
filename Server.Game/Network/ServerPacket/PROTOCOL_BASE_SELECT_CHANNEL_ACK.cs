using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_SELECT_CHANNEL_ACK : GameServerPacket
    {
        private readonly int ServerId;
        private readonly ushort ChannelId;
        private readonly uint Error;
        public PROTOCOL_BASE_SELECT_CHANNEL_ACK(uint Error, int ServerId, int ChannelId)
        {
            this.Error = Error;
            this.ServerId = ServerId;
            this.ChannelId = (ushort)ChannelId;
        }
        public override void Write()
        {
            WriteH(543);
            WriteD(0);
            WriteD(Error);
            if (Error == 0)
            {
                WriteD(ServerId);
                WriteH(ChannelId);
            }
        }
    }
}