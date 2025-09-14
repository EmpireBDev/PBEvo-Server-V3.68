using Server.Game.Data.Models;
using System.Net;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_CREATE_ACK : GameServerPacket
    {
        private readonly RoomModel Room;
        private readonly uint Error;
        public PROTOCOL_ROOM_CREATE_ACK(uint Error, RoomModel Room)
        {
            this.Error = Error;
            this.Room = Room;
        }
        public override void Write()
        {
            WriteH(3842);
            WriteD(Error == 0 ? (uint)Room.RoomId : Error);
            if (Error == 0)
            {
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
            }
        }
    }
}