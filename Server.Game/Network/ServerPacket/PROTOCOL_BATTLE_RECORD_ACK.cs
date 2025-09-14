using Plugin.Core.Models;
using Plugin.Core.Network;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_RECORD_ACK : GameServerPacket
    {
        private readonly RoomModel Room;
        public PROTOCOL_BATTLE_RECORD_ACK(RoomModel Room)
        {
            this.Room = Room;
        }
        public override void Write()
        {
            WriteH(4139);
            WriteH((ushort)Room.FRKills);
            WriteH((ushort)Room.FRDeaths);
            WriteH((ushort)Room.FRAssists);
            WriteH((ushort)Room.CTKills);
            WriteH((ushort)Room.CTDeaths);
            WriteH((ushort)Room.CTAssists);
            foreach (SlotModel Slot in Room.Slots)
            {
                WriteH((ushort)Slot.AllKills);
                WriteH((ushort)Slot.AllDeaths);
                WriteH((ushort)Slot.AllAssists);
            }
        }
    }
}