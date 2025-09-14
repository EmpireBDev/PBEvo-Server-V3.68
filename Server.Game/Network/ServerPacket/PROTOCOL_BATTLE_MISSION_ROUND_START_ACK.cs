using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Game.Data.Models;
using Server.Game.Data.Utils;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_MISSION_ROUND_START_ACK : GameServerPacket
    {
        private readonly RoomModel Room;
        public PROTOCOL_BATTLE_MISSION_ROUND_START_ACK(RoomModel Room)
        {
            this.Room = Room;
        }
        public override void Write()
        {
            WriteH(4129);
            WriteC((byte)Room.Rounds);
            WriteD(Room.GetInBattleTimeLeft());
            WriteH(AllUtils.GetSlotsFlag(Room, true, false));
            WriteC(0);
            WriteH((ushort)Room.FRRounds);
            WriteH((ushort)Room.CTRounds);
        }
    }
}