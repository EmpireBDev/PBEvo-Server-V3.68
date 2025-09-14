using Plugin.Core.Network;
using Plugin.Core.Utility;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_SHOP_LEAVE_ACK : GameServerPacket
    {
        public PROTOCOL_SHOP_LEAVE_ACK()
        {
        }
        public override void Write()
        {
            WriteH(1028);
            WriteH(0);
            WriteD(0); //Erro | uint.Parse(DateTimeUtil.Now("yyMMddHHmm"))
        }
    }
}