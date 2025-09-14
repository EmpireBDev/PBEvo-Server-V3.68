using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_COMMISSION_REGULAR_RESULT_ACK : GameServerPacket
    {
        public PROTOCOL_CS_COMMISSION_REGULAR_RESULT_ACK()
        {
        }
        public override void Write()
        {
            WriteH(1865);
        }
    }
}