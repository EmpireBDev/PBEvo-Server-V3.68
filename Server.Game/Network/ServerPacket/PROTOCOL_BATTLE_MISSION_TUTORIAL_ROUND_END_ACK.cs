using Plugin.Core.Network;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_MISSION_TUTORIAL_ROUND_END_ACK : GameServerPacket
    {
        private readonly RoomModel Room;
        public PROTOCOL_BATTLE_MISSION_TUTORIAL_ROUND_END_ACK(RoomModel Room)
        {
            this.Room = Room;
        }
        public override void Write()
        {
            WriteH(4165);
            WriteC(3);
            WriteH((short)((Room.GetTimeByMask() * 60) - Room.GetInBattleTime()));
        }
    }
}