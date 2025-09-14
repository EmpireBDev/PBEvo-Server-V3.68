using Plugin.Core.Network;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_REQUEST_INFO_ACK : GameServerPacket
    {
        private readonly string Text;
        private readonly uint Error;
        private readonly Account Player;
        public PROTOCOL_CS_REQUEST_INFO_ACK(long PlayerId, string Text)
        {
            this.Text = Text;
            Player = AccountManager.GetAccount(PlayerId, 31);
            if (Player == null || Text == null)
            {
                Error = 0x80000000;
            }
        }
        public override void Write()
        {
            WriteH(1845);
            WriteD(Error);
            if (Error == 0)
            {
                WriteQ(Player.PlayerId);
                WriteU(Player.Nickname, 66);
                WriteC((byte)Player.Rank);
                WriteD(Player.Statistic.Basic.KillsCount);
                WriteD(Player.Statistic.Basic.DeathsCount);
                WriteD(Player.Statistic.Basic.Matches);
                WriteD(Player.Statistic.Basic.MatchWins);
                WriteD(Player.Statistic.Basic.MatchLoses);
                WriteN(Text, Text.Length + 2, "UTF-16LE");
            }
        }
    }
}
