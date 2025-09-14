using Plugin.Core.Utility;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_QUEST_GET_INFO_ACK : GameServerPacket
    {
        private readonly Account player;
        public PROTOCOL_BASE_QUEST_GET_INFO_ACK(Account player)
        {
            this.player = player;
        }
        public override void Write()
        {
            WriteH(563);
            WriteC((byte)player.Mission.ActualMission);
            WriteC((byte)player.Mission.ActualMission);
            WriteC((byte)player.Mission.Card1);
            WriteC((byte)player.Mission.Card2);
            WriteC((byte)player.Mission.Card3);
            WriteC((byte)player.Mission.Card4);
            WriteB(ComDiv.GetMissionCardFlags(player.Mission.Mission1, player.Mission.List1));
            WriteB(ComDiv.GetMissionCardFlags(player.Mission.Mission2, player.Mission.List2));
            WriteB(ComDiv.GetMissionCardFlags(player.Mission.Mission3, player.Mission.List3));
            WriteB(ComDiv.GetMissionCardFlags(player.Mission.Mission4, player.Mission.List4));
            WriteC((byte)player.Mission.Mission1);
            WriteC((byte)player.Mission.Mission2);
            WriteC((byte)player.Mission.Mission3);
            WriteC((byte)player.Mission.Mission4);
        }
    }
}