using Plugin.Core.Utility;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_GIFTSHOP_LEAVE_ACK : GameServerPacket
    {
        public PROTOCOL_AUTH_GIFTSHOP_LEAVE_ACK()
        {
        }
        public override void Write()
        {
            WriteH(1083);
            WriteH(0);
            WriteD(uint.Parse(DateTimeUtil.Now("yyMMddHHmm")));
        }
    }
}
