using Server.Game.Data.Models;
using System.Net;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_CHANGE_ROOM_OPTIONINFO_ACK : GameServerPacket
    {
        private readonly RoomModel Room;
        public PROTOCOL_ROOM_CHANGE_ROOM_OPTIONINFO_ACK(RoomModel Room)
        {
            this.Room = Room;
        }
        public override void Write()
        {
            WriteH(3874);
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
