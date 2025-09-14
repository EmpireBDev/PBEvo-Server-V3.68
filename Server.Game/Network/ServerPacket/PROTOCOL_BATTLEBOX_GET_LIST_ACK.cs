using Plugin.Core.Managers;
using Plugin.Core.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLEBOX_GET_LIST_ACK : GameServerPacket
    {
        private readonly int Total;
        private readonly ShopData Data;
        public PROTOCOL_BATTLEBOX_GET_LIST_ACK(ShopData Data, int Total)
        {
            this.Data = Data;
            this.Total = Total;
        }
        public override void Write()
        {
            WriteH(7426);
            WriteD(Total);
            WriteC(0); // ? new
            WriteD(Data.ItemsCount);
            WriteD(Data.Offset);
            WriteB(Data.Buffer);
            WriteD(100);
        }
    }
}
