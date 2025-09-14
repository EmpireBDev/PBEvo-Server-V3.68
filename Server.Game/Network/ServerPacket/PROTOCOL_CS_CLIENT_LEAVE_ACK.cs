using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_CLIENT_LEAVE_ACK : GameServerPacket
    {
        public PROTOCOL_CS_CLIENT_LEAVE_ACK()
        {
        }
        public override void Write()
        {
            WriteH(1796);
            WriteD(0);
        }
    }
}