using Plugin.Core.Network;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_MISSION_GENERATOR_INFO_ACK : GameServerPacket
    {
        private readonly RoomModel Room;
        public PROTOCOL_BATTLE_MISSION_GENERATOR_INFO_ACK(RoomModel Room)
        {
            this.Room = Room;
        }
        public override void Write()
        {
            WriteH(4143);
            WriteH((ushort)Room.Bar1);
            WriteH((ushort)Room.Bar2);
            for (int i = 0; i < 16; i++)
            {
                WriteH(Room.Slots[i].DamageBar1);
            }
        }
    }
}