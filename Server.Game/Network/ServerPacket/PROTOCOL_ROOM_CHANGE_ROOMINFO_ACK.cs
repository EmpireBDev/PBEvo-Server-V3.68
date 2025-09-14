using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_CHANGE_ROOMINFO_ACK : GameServerPacket
    {
        private readonly RoomModel Room;
        private readonly bool IsBotMode;
        public PROTOCOL_ROOM_CHANGE_ROOMINFO_ACK(RoomModel Room)
        {
            this.Room = Room;
            if (Room != null)
            {
                IsBotMode = Room.IsBotMode();
            }
        }
        public PROTOCOL_ROOM_CHANGE_ROOMINFO_ACK(RoomModel Room, bool IsBotMode)
        {
            this.Room = Room;
            this.IsBotMode = IsBotMode;
        }
        public override void Write()
        {
            WriteH(3857);
            WriteD(Room.RoomId);
            WriteU(Room.Name, 46);
            WriteC((byte)Room.MapId);
            WriteC((byte)Room.Rule);
            WriteC((byte)Room.Stage);
            WriteC((byte)Room.RoomType);
            WriteC((byte)Room.State);
            WriteC((byte)Room.GetCountPlayers());
            WriteC((byte)Room.GetSlotCount());
            WriteC((byte)Room.Ping);
            WriteH((ushort)Room.WeaponsFlag);
            WriteD(Room.GetFlag());
            WriteH(0);
            WriteD(Room.NewInt);
            WriteC(0);
            WriteU(Room.LeaderName, 66);
            WriteD(Room.KillTime);
            WriteC(Room.Limit);
            WriteC(Room.WatchRuleFlag);
            WriteH(Room.BalanceType);
            WriteB(Room.RandomMaps);
            WriteC(Room.CountdownIG);
            WriteB(Room.LeaderAddr);
            WriteC(Room.KillCam);
            WriteH(0);
            if (IsBotMode)
            {
                WriteC(Room.AiCount);
                WriteC(Room.AiLevel);
            }
        }
    }
}
