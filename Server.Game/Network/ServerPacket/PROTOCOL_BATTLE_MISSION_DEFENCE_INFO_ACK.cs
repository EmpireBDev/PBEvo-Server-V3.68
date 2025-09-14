using Plugin.Core.Network;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_MISSION_DEFENCE_INFO_ACK : GameServerPacket
    {
        private readonly RoomModel room;
        public PROTOCOL_BATTLE_MISSION_DEFENCE_INFO_ACK(RoomModel room)
        {
            this.room = room;
        }
        public override void Write()
        {
            WriteH(4157);
            WriteH((ushort)room.Bar1);
            WriteH((ushort)room.Bar2);
            for (int i = 0; i < 16; i++)
            {
                WriteH(room.Slots[i].DamageBar1);
            }
            for (int i = 0; i < 16; i++)
            {
                WriteH(room.Slots[i].DamageBar2);
            }
        }
    }
}