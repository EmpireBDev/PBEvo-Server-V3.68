using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_MISSION_ROUND_END_ACK : GameServerPacket
    {
        private readonly RoomModel Room;
        private readonly int Winner;
        private readonly RoundEndType Reason;
        public PROTOCOL_BATTLE_MISSION_ROUND_END_ACK(RoomModel Room, int Winner, RoundEndType Reason)
        {
            this.Room = Room;
            this.Winner = Winner;
            this.Reason = Reason;
        }
        public PROTOCOL_BATTLE_MISSION_ROUND_END_ACK(RoomModel Room, TeamEnum Winner, RoundEndType Reason)
        {
            this.Room = Room;
            this.Winner = (int)Winner;
            this.Reason = Reason;
        }
        public override void Write()
        {
            WriteH(4131);
            WriteC((byte)Winner);
            WriteC((byte)Reason);
            if (Room.IsDinoMode("DE"))
            {
                WriteH((ushort)Room.FRDino);
                WriteH((ushort)Room.CTDino);
            }
            else if (Room.RoomType == RoomCondition.DeathMatch || Room.RoomType == RoomCondition.FreeForAll)
            {
                WriteH((ushort)Room.FRKills);
                WriteH((ushort)Room.CTKills);
            }
            else
            {
                WriteH((ushort)Room.FRRounds);
                WriteH((ushort)Room.CTRounds);
            }
        }
    }
}