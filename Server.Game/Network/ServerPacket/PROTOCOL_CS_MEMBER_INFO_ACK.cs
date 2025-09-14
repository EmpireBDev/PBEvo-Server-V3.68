using Plugin.Core.Network;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using System.Collections.Generic;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_MEMBER_INFO_ACK : GameServerPacket
    {
        private readonly List<Account> players;
        public PROTOCOL_CS_MEMBER_INFO_ACK(List<Account> players)
        {
            this.players = players;
        }
        public override void Write()
        {
            WriteH(1869);
            WriteC((byte)players.Count);
            for (int i = 0; i < players.Count; i++)
            {
                Account member = players[i];
                WriteC((byte)(member.Nickname.Length + 1));
                WriteN(member.Nickname, member.Nickname.Length + 2, "UTF-16LE");
                WriteQ(member.PlayerId);
                WriteQ(ComDiv.GetClanStatus(member.Status, member.IsOnline));
                WriteC((byte)member.Rank);
                WriteC((byte)member.NickColor);
            }
        }
    }
}