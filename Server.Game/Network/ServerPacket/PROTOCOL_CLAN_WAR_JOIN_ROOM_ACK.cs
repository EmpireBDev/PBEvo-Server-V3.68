using Plugin.Core.Network;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CLAN_WAR_JOIN_ROOM_ACK : GameServerPacket
    {
        private readonly MatchModel match;
        private readonly int roomId, team;
        public PROTOCOL_CLAN_WAR_JOIN_ROOM_ACK(MatchModel match, int roomId, int team)
        {
            this.match = match;
            this.roomId = roomId;
            this.team = team;
        }
        public override void Write()
        {
            WriteH(1566);
            WriteD(roomId);
            WriteH((ushort)team);
            WriteH((ushort)match.GetServerInfo());
        }
    }
}