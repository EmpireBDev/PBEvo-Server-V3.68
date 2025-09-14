using Plugin.Core.Utility;
using Server.Auth.Data.Models;
using System.Collections.Generic;

namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_CS_MEMBER_INFO_ACK : AuthServerPacket
    {
        private readonly List<Account> Players;
        public PROTOCOL_CS_MEMBER_INFO_ACK(List<Account> Players)
        {
            this.Players = Players;
        }
        public override void Write()
        {
            WriteH(1869);
            WriteC((byte)Players.Count);
            foreach (Account Member in Players)
            {
                WriteC((byte)(Member.Nickname.Length + 1));
                WriteN(Member.Nickname, Member.Nickname.Length + 2, "UTF-16LE");
                WriteQ(Member.PlayerId);
                WriteQ(ComDiv.GetClanStatus(Member.Status, Member.IsOnline));
                WriteC((byte)Member.Rank);
                WriteC((byte)Member.NickColor);
                //new
                WriteD(Member.Statistic.Clan.Matches); //member.Statistic.Clan need to be checked. Sometimes it detected Null so MemberList PAGE NOT SHOWS PLAYER.
                WriteD(Member.Statistic.Clan.MatchWins); //member.Statistic.Clan need to be checked. Sometimes it detected Null so MemberList PAGE NOT SHOWS PLAYER.
                WriteD(Member.NameCard);
                WriteC((byte)0); //UNK: wesley said this is bonus.nickEffect
                WriteD(0); //Medals of the week
                WriteD(0); //Medals of the month
                WriteD(0); //Medal Gained ( total )
            }
        }
    }
}
