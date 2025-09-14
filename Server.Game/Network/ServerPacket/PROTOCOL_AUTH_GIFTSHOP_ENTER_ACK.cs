using Plugin.Core.Utility;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_GIFTSHOP_ENTER_ACK : GameServerPacket
    {
        public PROTOCOL_AUTH_GIFTSHOP_ENTER_ACK()
        {
        }
        public override void Write()
        {
            WriteH(1081);
            WriteC(0);
            WriteD(uint.Parse(DateTimeUtil.Now("yyMMddHHmm")));
        }
    }
}
