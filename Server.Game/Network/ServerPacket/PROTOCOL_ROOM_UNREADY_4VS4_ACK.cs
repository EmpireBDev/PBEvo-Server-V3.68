using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_UNREADY_4VS4_ACK : GameServerPacket
    {
        public PROTOCOL_ROOM_UNREADY_4VS4_ACK()
        {
        }
        public override void Write()
        {
            WriteH(3888);
            WriteD(0);
        }
    }
}