using Plugin.Core.Network;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CLAN_WAR_CREATE_TEAM_ACK : GameServerPacket
    {
        private readonly uint erro;
        private readonly MatchModel mt;
        public PROTOCOL_CLAN_WAR_CREATE_TEAM_ACK(uint erro, MatchModel mt = null)
        {
            this.erro = erro;
            this.mt = mt;
        }
        public override void Write()
        {
            WriteH(6919);
            WriteD(erro);
            if (erro == 0)
            {
                WriteH((short)mt.MatchId);
                WriteH((short)mt.GetServerInfo());
                WriteH((short)mt.GetServerInfo());
                WriteC((byte)mt.State);
                WriteC((byte)mt.FriendId);
                WriteC((byte)mt.Training);
                WriteC((byte)mt.GetCountPlayers());
                WriteD(mt.Leader);
                WriteC(0);
            }
        }
    }
}