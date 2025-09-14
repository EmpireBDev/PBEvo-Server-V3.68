using Plugin.Core.Network;
using Server.Game.Data.Models;
using System.Collections.Generic;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_CLAN_MATCH_RESULT_LIST_ACK : GameServerPacket
    {
        private readonly List<MatchModel> c;
        private readonly int erro;
        public PROTOCOL_CS_CLAN_MATCH_RESULT_LIST_ACK(int erro, List<MatchModel> c)
        {
            this.erro = erro;
            this.c = c;
        }
        public override void Write()
        {
            WriteH(1957);
            WriteC((byte)(erro == 0 ? c.Count : erro));
            if (erro > 0 || c.Count == 0)
            {
                return;
            }
            WriteC(1);
            WriteC(0);
            WriteC((byte)c.Count);
            for (int i = 0; i < c.Count; i++)
            {
                MatchModel m = c[i];
                WriteH((short)m.MatchId);
                WriteH((ushort)m.GetServerInfo());
                WriteH((ushort)m.GetServerInfo());
                WriteC((byte)m.State);
                WriteC((byte)m.FriendId);
                WriteC((byte)m.Training);
                WriteC((byte)m.GetCountPlayers());
                WriteC(0);
                WriteD(m.Leader);
                Account p = m.GetLeader();
                if (p != null)
                {
                    WriteC((byte)p.Rank);
                    WriteU(p.Nickname, 66);
                    WriteQ(p.PlayerId);
                    WriteC((byte)m.Slots[m.Leader].State);
                }
                else
                {
                    WriteB(new byte[76]);
                }
            }
        }
    }
}