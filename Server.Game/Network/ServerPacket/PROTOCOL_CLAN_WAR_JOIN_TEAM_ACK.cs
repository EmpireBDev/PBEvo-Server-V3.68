using Plugin.Core.Models;
using Plugin.Core.Network;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CLAN_WAR_JOIN_TEAM_ACK : GameServerPacket
    {
        private readonly MatchModel m;
        private readonly uint erro;
        public PROTOCOL_CLAN_WAR_JOIN_TEAM_ACK(uint erro, MatchModel m = null)
        {
            this.erro = erro;
            this.m = m;
        }
        public override void Write()
        {
            WriteH(6921);
            WriteD(erro);
            if (erro == 0)
            {
                WriteH((short)m.MatchId);
                WriteH((ushort)m.GetServerInfo());
                WriteH((ushort)m.GetServerInfo());
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
                for (int i = 0; i < m.Training; i++)
                {
                    SlotMatch s = m.Slots[i];
                    Account pS = m.GetPlayerBySlot(s);
                    if (pS != null)
                    {
                        WriteC((byte)pS.Rank);
                        WriteS(pS.Nickname, 33);
                        WriteQ(pS.PlayerId);
                        WriteC((byte)s.State);
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