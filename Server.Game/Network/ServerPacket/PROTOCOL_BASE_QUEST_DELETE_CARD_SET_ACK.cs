using Plugin.Core.Utility;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_QUEST_DELETE_CARD_SET_ACK : GameServerPacket
    {
        private readonly uint Error;
        private readonly Account Player;
        public PROTOCOL_BASE_QUEST_DELETE_CARD_SET_ACK(uint Error, Account Player)
        {
            this.Error = Error;
            this.Player = Player;
        }
        public override void Write()
        {
            WriteH(575);
            WriteD(Error);
            if (Error == 0)
            {
                WriteC((byte)Player.Mission.ActualMission);
                WriteC((byte)Player.Mission.Card1);
                WriteC((byte)Player.Mission.Card2);
                WriteC((byte)Player.Mission.Card3);
                WriteC((byte)Player.Mission.Card4);
                WriteB(ComDiv.GetMissionCardFlags(Player.Mission.Mission1, Player.Mission.List1));
                WriteB(ComDiv.GetMissionCardFlags(Player.Mission.Mission2, Player.Mission.List2));
                WriteB(ComDiv.GetMissionCardFlags(Player.Mission.Mission3, Player.Mission.List3));
                WriteB(ComDiv.GetMissionCardFlags(Player.Mission.Mission4, Player.Mission.List4));
                WriteC((byte)Player.Mission.Mission1);
                WriteC((byte)Player.Mission.Mission2);
                WriteC((byte)Player.Mission.Mission3);
                WriteC((byte)Player.Mission.Mission4);
            }
        }
    }
}