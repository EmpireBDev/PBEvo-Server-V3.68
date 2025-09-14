using Plugin.Core.Network;
using System;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CLAN_WAR_MATCH_TEAM_COUNT_ACK : GameServerPacket
    {
        private readonly int count;
        public PROTOCOL_CLAN_WAR_MATCH_TEAM_COUNT_ACK(int count)
        {
            this.count = count;
        }
        public override void Write()
        {
            WriteH(6915);
            WriteH((short)count);
            WriteC(13);
            WriteH((short)Math.Ceiling(count / 13d));
        }
    }
}