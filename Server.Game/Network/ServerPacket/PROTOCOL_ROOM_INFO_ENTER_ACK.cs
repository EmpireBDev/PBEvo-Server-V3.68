using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_INFO_ENTER_ACK : GameServerPacket
    {
        public PROTOCOL_ROOM_INFO_ENTER_ACK()
        {
        }
        public override void Write()
        {
            WriteH(3928);
            WriteD(0);
            WriteC(67);
        }
    }
}