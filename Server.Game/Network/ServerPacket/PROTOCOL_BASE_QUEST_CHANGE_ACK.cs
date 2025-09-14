using Plugin.Core.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_QUEST_CHANGE_ACK : GameServerPacket
    {
        private readonly int MissionId, Value;
        public PROTOCOL_BASE_QUEST_CHANGE_ACK(int progress, MissionCardModel card)
        {
            MissionId = card.MissionBasicId;
            if (card.MissionLimit == progress)
            {
                MissionId += 240;
            }
            Value = progress;
        }
        public override void Write()
        {
            WriteH(567);
            WriteC((byte)MissionId);
            WriteC((byte)Value);
        }
    }
}
