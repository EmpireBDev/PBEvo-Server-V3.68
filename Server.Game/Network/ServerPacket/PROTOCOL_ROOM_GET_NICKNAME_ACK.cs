using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_GET_NICKNAME_ACK : GameServerPacket
    {
        private readonly int SlotIdx;
        private readonly string name;
        public PROTOCOL_ROOM_GET_NICKNAME_ACK(int SlotIdx, string name)
        {
            this.SlotIdx = SlotIdx;
            this.name = name;
        }
        public override void Write()
        {
            WriteH(3855);
            WriteD(SlotIdx);
            WriteU(name, 66);
        }
    }
}
