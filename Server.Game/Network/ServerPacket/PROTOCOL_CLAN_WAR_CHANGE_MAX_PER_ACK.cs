using Plugin.Core.Network;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CLAN_WAR_CHANGE_MAX_PER_ACK : GameServerPacket
    {
        public readonly MatchModel match;
        public readonly Account Player;
        public PROTOCOL_CLAN_WAR_CHANGE_MAX_PER_ACK(MatchModel match, Account Player)
        {
            this.match = match;
            this.Player = Player;
        }
        public override void Write()
        {
            WriteH(6927);
            WriteH((short)match.MatchId);
            WriteH((ushort)match.GetServerInfo());
            WriteH((ushort)match.GetServerInfo());
            WriteC((byte)match.State);
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
            if (Player != null)
            {
                WriteC((byte)Player.Rank);
                WriteS(Player.Nickname, 33);
                WriteQ(Player.PlayerId);
                WriteC((byte)match.Slots[match.Leader].State);
            }
            else
            {
                WriteB(new byte[43]);
            }
        }
    }
}