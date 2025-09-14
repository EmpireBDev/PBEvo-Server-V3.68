using Plugin.Core.Network;
using Plugin.Core.Utility;
using Server.Game.Data.Managers;
using System;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_CLIENT_ENTER_ACK : GameServerPacket
    {
        private readonly int type, clanId;
        public PROTOCOL_CS_CLIENT_ENTER_ACK(int clanId, int type)
        {
            this.clanId = clanId;
            this.type = type;
        }
        public override void Write()
        {
            WriteH(1794);
            WriteD(0);
            WriteD(clanId);
            WriteD(type);
            if (clanId == 0 || type == 0)
            {
                WriteD(ClanManager.Clans.Count);
                WriteC(15);
                WriteH((ushort)Math.Ceiling(ClanManager.Clans.Count / 15d));
                WriteD(uint.Parse(DateTimeUtil.Now("MMddHHmmss")));
            }
        }
    }
}