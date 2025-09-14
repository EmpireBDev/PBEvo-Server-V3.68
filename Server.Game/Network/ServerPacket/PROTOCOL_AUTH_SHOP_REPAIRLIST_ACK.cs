using Plugin.Core.Managers;
using Plugin.Core.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_SHOP_REPAIRLIST_ACK : GameServerPacket
    {
        private readonly int Total;
        private readonly ShopData Data;
        public PROTOCOL_AUTH_SHOP_REPAIRLIST_ACK(ShopData Data, int Total)
        {
            this.Data = Data;
            this.Total = Total;
        }
        public override void Write()
        {
            WriteH(1070);
            WriteD(Total);
            WriteD(Data.ItemsCount);
            WriteD(Data.Offset);
            WriteB(Data.Buffer);
            WriteD(100);
        }
    }
}