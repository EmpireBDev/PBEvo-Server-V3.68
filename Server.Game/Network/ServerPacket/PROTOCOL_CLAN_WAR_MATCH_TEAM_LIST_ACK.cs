using Server.Game.Data.Models;
using System.Collections.Generic;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CLAN_WAR_MATCH_TEAM_LIST_ACK : GameServerPacket
    {
        private readonly List<MatchModel> matchs;
        private readonly int matchId, MatchCount;
        public PROTOCOL_CLAN_WAR_MATCH_TEAM_LIST_ACK(List<MatchModel> matchs, int matchId)
        {
            this.matchId = matchId;
            this.matchs = matchs;
            MatchCount = (matchs.Count - 1);
        }
        public override void Write()
        {
            WriteH(6917);
            WriteH((ushort)MatchCount);
            if (MatchCount > 0)
            {
                WriteH(1);
                WriteH(0);
                WriteC((byte)MatchCount);
                foreach (MatchModel m in matchs)
                {
                    if (m.MatchId == matchId)
                    {
                        continue;
                    }
                    WriteH((short)m.MatchId);
                    WriteH((short)m.GetServerInfo());
                    WriteH((short)m.GetServerInfo());
                    WriteC((byte)m.State);
                    WriteC((byte)m.FriendId);
                    WriteC((byte)m.Training);
                    WriteC((byte)m.GetCountPlayers());
                    WriteD(m.Leader);
                    WriteC(0);
                    WriteD(m.Clan.Id);
                    WriteC((byte)m.Clan.Rank);
                    WriteD(m.Clan.Logo);
                    WriteS(m.Clan.Name, 17);
                    WriteT(m.Clan.Points);
                    WriteC((byte)m.Clan.NameColor);
                    Account p = m.GetLeader();
                    if (p != null)
                    {
                        WriteC((byte)p.Rank);
                        WriteS(p.Nickname, 33);
                        WriteQ(p.PlayerId);
                        WriteC((byte)m.Slots[m.Leader].State);
                    }
                    else
                    {
                        WriteB(new byte[43]);
                    }
                }
            }
        }
    }
}