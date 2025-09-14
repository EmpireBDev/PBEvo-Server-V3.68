using Plugin.Core.Models;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using System.Collections.Generic;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_SHOP_GOODS_GIFT_ACK : GameServerPacket
    {
        private readonly Account Player;
        private readonly List<GoodsItem> Goods;
        private readonly uint Error;
        public PROTOCOL_AUTH_SHOP_GOODS_GIFT_ACK(uint Error, List<GoodsItem> Goods, Account Player)
        {
            this.Error = Error;
            this.Goods = Goods;
            this.Player = Player;
        }
        public PROTOCOL_AUTH_SHOP_GOODS_GIFT_ACK(uint Error)
        {
            this.Error = Error;
        }
        public override void Write()
        {
            WriteH(1046);
            WriteH(0);
            if (Error == 1)
            {
                WriteC((byte)Goods.Count);
                for (int i = 0; i < Goods.Count; i++)
                {
                    GoodsItem Item = Goods[i];
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
