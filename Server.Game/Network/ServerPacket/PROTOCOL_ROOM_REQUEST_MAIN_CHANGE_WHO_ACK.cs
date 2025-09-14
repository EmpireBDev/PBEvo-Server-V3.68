using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_WHO_ACK : GameServerPacket
    {
        private readonly uint slot;
        public PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_WHO_ACK(uint slot)
        {
            this.slot = slot;
        }
        public PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_WHO_ACK(int slot)
        {
            this.slot = (uint)slot;
        }
        public override void Write()
        {
            WriteH(3880);
            WriteD(slot);
        }
    }
}