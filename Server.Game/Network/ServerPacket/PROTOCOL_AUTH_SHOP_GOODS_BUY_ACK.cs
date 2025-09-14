using Plugin.Core.Models;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using System.Collections.Generic;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_SHOP_GOODS_BUY_ACK : GameServerPacket
    {
        private readonly List<GoodsItem> Goods;
        private readonly Account Player;
        private readonly uint Error;
        public PROTOCOL_AUTH_SHOP_GOODS_BUY_ACK(uint Error, List<GoodsItem> Goods, Account Player)
        {
            this.Error = Error;
            this.Player = Player;
            this.Goods = Goods;
        }
        public PROTOCOL_AUTH_SHOP_GOODS_BUY_ACK(uint Error)
        {
            this.Error = Error;
        }
        public override void Write()
        {
            WriteH(1044);
            WriteH(0);
            if (Error == 1)
            {
                WriteC((byte)Goods.Count);
                foreach (GoodsItem Item in Goods)
                {
                    WriteD(0);
                    WriteD(Item.Id);
                    WriteC(0);
                }
                WriteD(Player.Cash);
                WriteD(Player.Gold);
                WriteD(uint.Parse(DateTimeUtil.Now("yyMMddHHmm")));
            }
            else
            {
                WriteD(Error);
            }
        }
    }
}