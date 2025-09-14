using Plugin.Core.Utility;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_SHOP_LIMITED_SALE_SYNC_ACK : GameServerPacket
    {
        public PROTOCOL_SHOP_LIMITED_SALE_SYNC_ACK()
        {
        }
        public override void Write()
        {
            WriteH(1098);
            WriteC(1);
            WriteD(uint.Parse(DateTimeUtil.Now("yyMMddHHmm")));
        }
    }
}