using Plugin.Core.Network;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_MISSION_TOUCHDOWN_COUNT_ACK : GameServerPacket
    {
        private readonly RoomModel Room;
        public PROTOCOL_BATTLE_MISSION_TOUCHDOWN_COUNT_ACK(RoomModel Room)
        {
            this.Room = Room;
        }
        public override void Write()
        {
            WriteH(4159);
            WriteH((ushort)Room.FRDino);
            WriteH((ushort)Room.CTDino);
        }
    }
}