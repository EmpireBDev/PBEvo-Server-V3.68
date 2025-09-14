using Plugin.Core.Network;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_LEAVEP2PSERVER_ACK : GameServerPacket
    {
        private readonly RoomModel room;
        public PROTOCOL_BATTLE_LEAVEP2PSERVER_ACK(RoomModel room)
        {
            this.room = room;
        }
        public override void Write()
        {
            WriteH(4125);
            WriteD(room.Leader);
        }
    }
}