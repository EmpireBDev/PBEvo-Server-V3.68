using Plugin.Core.Network;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_CHANGE_DIFFICULTY_LEVEL_ACK : GameServerPacket
    {
        private readonly RoomModel room;
        public PROTOCOL_BATTLE_CHANGE_DIFFICULTY_LEVEL_ACK(RoomModel room)
        {
            this.room = room;
        }
        public override void Write()
        {
            WriteH(4149);
            WriteC(room.IngameAiLevel);
            for (int i = 0; i < 16; i++)
            {
                WriteD(room.Slots[i].AiLevel);
            }
        }
    }
}