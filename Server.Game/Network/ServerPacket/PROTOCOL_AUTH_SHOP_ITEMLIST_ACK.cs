using Plugin.Core.Managers;
using Plugin.Core.Models;
using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_SHOP_ITEMLIST_ACK : GameServerPacket
    {
        private readonly int Total;
        private readonly ShopData Data;
        public PROTOCOL_AUTH_SHOP_ITEMLIST_ACK(ShopData Data, int Total)
        {
            this.Data = Data;
            this.Total = Total;
        }
        public override void Write()
        {
            WriteH(1038);
            WriteD(Total);
            WriteD(Data.ItemsCount);
            WriteD(Data.Offset);
            WriteB(Data.Buffer);
            WriteD(800);
        }
    }
}