using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_CHECK_MAIN_ACK : GameServerPacket
    {
        private readonly uint slot;
        public PROTOCOL_ROOM_CHECK_MAIN_ACK(uint slot)
        {
            this.slot = slot;
        }
        public override void Write()
        {
            WriteH(3882);
            WriteD(slot);
        }
    }
}