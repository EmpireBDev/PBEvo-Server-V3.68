using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_LOBBY_GET_ROOMINFOADD_ACK : GameServerPacket
    {
        private readonly RoomModel Room;
        public PROTOCOL_LOBBY_GET_ROOMINFOADD_ACK(RoomModel Room)
        {
            this.Room = Room;
        }
        public override void Write()
        {
            WriteH(3084);
            WriteC(0);
            WriteU(Room.LeaderName, 66);
            WriteC((byte)Room.KillTime);
            WriteC((byte)(Room.Rounds - 1));
            WriteH((ushort)Room.GetInBattleTime());
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