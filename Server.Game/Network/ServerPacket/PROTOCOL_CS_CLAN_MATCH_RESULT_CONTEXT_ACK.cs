using Plugin.Core.Network;
using System;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_CLAN_MATCH_RESULT_CONTEXT_ACK : GameServerPacket
    {
        private readonly int matchCount;
        public PROTOCOL_CS_CLAN_MATCH_RESULT_CONTEXT_ACK(int count)
        {
            matchCount = count;
        }
        public override void Write()
        {
            WriteH(1955);
            WriteC((byte)matchCount);
            WriteC(13);
            WriteC((byte)Math.Ceiling(matchCount / 13d));
        }
    }
}