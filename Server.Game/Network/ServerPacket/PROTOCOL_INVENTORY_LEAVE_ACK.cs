using Plugin.Core.Network;
using Plugin.Core.Utility;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_INVENTORY_LEAVE_ACK : GameServerPacket
    {
        public PROTOCOL_INVENTORY_LEAVE_ACK()
        {
        }
        public override void Write()
        {
            WriteH(3332);
            WriteH(0);
            WriteD(0); //Erro | uint.Parse(DateTimeUtil.Now("yyMMddHHmm"))
        }
    }
}