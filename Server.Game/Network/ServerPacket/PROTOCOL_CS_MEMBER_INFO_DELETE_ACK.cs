using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_MEMBER_INFO_DELETE_ACK : GameServerPacket
    {
        private readonly long PlayerId;
        public PROTOCOL_CS_MEMBER_INFO_DELETE_ACK(long PlayerId)
        {
            this.PlayerId = PlayerId;
        }
        public override void Write()
        {
            WriteH(1873);
            WriteQ(PlayerId);
        }
    }
}