using Plugin.Core.Network;
using Plugin.Core.Utility;
using System;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_CLIENT_CLAN_CONTEXT_ACK : GameServerPacket
    {
        private readonly int clansCount;
        public PROTOCOL_CS_CLIENT_CLAN_CONTEXT_ACK(int count)
        {
            clansCount = count;
        }
        public override void Write()
        {
            WriteH(1800);
            WriteD(clansCount);
            WriteC(15);
            WriteH((ushort)Math.Ceiling(clansCount / 15d));
            WriteD(uint.Parse(DateTimeUtil.Now("MMddHHmmss")));
        }
    }
}