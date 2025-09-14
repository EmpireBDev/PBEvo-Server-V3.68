using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Managers;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_AUTH_SHOP_GOODS_BUY_REQ : GameClientPacket
    {
        private List<CartGoods> ShopCart = new List<CartGoods>();
        public PROTOCOL_AUTH_SHOP_GOODS_BUY_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            int count = ReadH();
            for (int i = 0; i < count; i++)
            {
                CartGoods Cart = new CartGoods()
                {
                    GoodId = ReadD(),
                    BuyType = ReadC(),
                };
                ShopCart.Add(Cart);
                ReadQ();
            }
        }
        public override void Run()
        {
            try
            {
                Account Player = Client.Player;
                if (Player == null)
                {
                    return;
                }
                if (Player.Inventory.Items.Count >= 500)
                {
                    Client.SendPacket(new PROTOCOL_AUTH_SHOP_GOODS_BUY_ACK(2147487929));
                    return;
                }
                List<GoodsItem> Goods = ShopManager.GetGoods(ShopCart, out int Gold, out int Cash, out int Tags);
                if (Goods.Count == 0)
                {
                    Client.SendPacket(new PROTOCOL_AUTH_SHOP_GOODS_BUY_ACK(2147487767));
                }
                else if (0 > (Player.Gold - Gold) || 0 > (Player.Cash - Cash) || 0 > (Player.Tags - Tags))
                {
                    Client.SendPacket(new PROTOCOL_AUTH_SHOP_GOODS_BUY_ACK(2147487768));
                }
                else if (DaoManagerSQL.UpdateAccountValuable(Player.PlayerId, (Player.Gold - Gold), (Player.Cash - Cash), (Player.Tags - Tags)))
                {
                    Player.Gold -= Gold;
                    Player.Cash -= Cash;
                    Player.Tags -= Tags;
                    foreach (GoodsItem Good in Goods)
                    {
                        Client.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(1, Player, Good.Item));
                    }
                    Client.SendPacket(new PROTOCOL_AUTH_SHOP_GOODS_BUY_ACK(1, Goods, Player));
                }
                else
                {
                    Client.SendPacket(new PROTOCOL_AUTH_SHOP_GOODS_BUY_ACK(2147487769));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_AUTH_SHOP_GOODS_BUY_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}