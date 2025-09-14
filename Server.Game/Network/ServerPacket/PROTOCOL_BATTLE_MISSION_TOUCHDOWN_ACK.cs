using Plugin.Core.Models;
using Plugin.Core.Network;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_MISSION_TOUCHDOWN_ACK : GameServerPacket
    {
        private readonly RoomModel room;
        private readonly SlotModel slot;
        public PROTOCOL_BATTLE_MISSION_TOUCHDOWN_ACK(RoomModel room, SlotModel slot)
        {
            this.room = room;
            this.slot = slot;
        }
        public override void Write()
        {
            WriteH(4155);
            WriteH((ushort)room.FRDino);
            WriteH((ushort)room.CTDino);
            WriteD(slot.Id);
            WriteH((short)slot.PassSequence);
        }
    }
}