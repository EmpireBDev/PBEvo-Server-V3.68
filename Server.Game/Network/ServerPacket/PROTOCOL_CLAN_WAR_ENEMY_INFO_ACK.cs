using Plugin.Core.Network;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CLAN_WAR_ENEMY_INFO_ACK : GameServerPacket
    {
        public readonly MatchModel match;
        public PROTOCOL_CLAN_WAR_ENEMY_INFO_ACK(MatchModel match)
        {
            this.match = match;
        }
        public override void Write()
        {
            WriteH(1574);
            WriteH((short)match.GetServerInfo());
            WriteC((byte)match.MatchId);
            WriteC((byte)match.FriendId);
            WriteC((byte)match.Training);
            WriteC((byte)match.GetCountPlayers());
            WriteD(match.Leader);
            WriteC(0);
            WriteD(match.Clan.Id);
            WriteC((byte)match.Clan.Rank);
            WriteD(match.Clan.Logo);
            WriteS(match.Clan.Name, 17);
            WriteT(match.Clan.Points);
            WriteC((byte)match.Clan.NameColor);
        }
    }
}