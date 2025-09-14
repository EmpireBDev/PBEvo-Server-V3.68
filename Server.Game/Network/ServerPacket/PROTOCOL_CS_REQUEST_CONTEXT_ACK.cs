using Plugin.Core.Network;
using Plugin.Core.SQL;
using Plugin.Core.Utility;
using System;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_REQUEST_CONTEXT_ACK : GameServerPacket
    {
        private readonly uint _erro;
        private readonly int invites;
        public PROTOCOL_CS_REQUEST_CONTEXT_ACK(int clanId)
        {
            if (clanId > 0)
            {
                invites = DaoManagerSQL.GetRequestClanInviteCount(clanId);
            }
            else
            {
                _erro = 4294967295;
            }
        }
        public override void Write()
        {
            WriteH(1841);
            WriteD(_erro);
            if (_erro == 0)
            {
                WriteC((byte)invites);
                WriteC(13);
                WriteC((byte)Math.Ceiling(invites / 13d));
                WriteD(uint.Parse(DateTimeUtil.Now("MMddHHmmss")));
            }
        }
    }
}