using Plugin.Core.Managers;
using Plugin.Core.Models;
using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_SHOP_LIMITED_SALE_LIST_ACK : GameServerPacket
    {
        public PROTOCOL_SHOP_LIMITED_SALE_LIST_ACK()
        {
        }
        public override void Write()
        {
            WriteH(1096);
            WriteD(1);
            WriteD(1);
            WriteD(1);
            WriteD(1);
            WriteC(1);
            WriteD(63266001);
            WriteC(1);
            WriteD(2412052359);
            
            WriteC(1);
        }
    }
}