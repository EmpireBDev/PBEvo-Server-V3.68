using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Managers;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_AUTH_SHOP_GOODS_GIFT_REQ : GameClientPacket
    {
        private string Message, TargetName;
        private List<CartGoods> ShopCart = new List<CartGoods>();
        public PROTOCOL_AUTH_SHOP_GOODS_GIFT_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            int count = ReadC();
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
            Message = ReadU(ReadC() * 2);
            TargetName = ReadU(ReadC() * 2);
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
                Account TargetUser = AccountManager.GetAccount(TargetName, 1, 0);
                if (TargetUser != null && TargetUser.IsOnline && Player.Nickname != TargetName)
                {
                    if (TargetUser.Inventory.Items.Count >= 500)
                    {
                        Client.SendPacket(new PROTOCOL_AUTH_SHOP_GOODS_BUY_ACK(2147487929));
                        return;
                    }
                    List<GoodsItem> Goods = ShopManager.GetGoods(ShopCart, out int gold, out int cash, out int tags);
                    if (Goods.Count == 0)
                    {
                        Client.SendPacket(new PROTOCOL_AUTH_SHOP_GOODS_BUY_ACK(2147487767));
                    }
                    else if (0 > (Player.Gold - gold) || 0 > (Player.Cash - cash) || 0 > (Player.Tags - tags))
                    {
                        Client.SendPacket(new PROTOCOL_AUTH_SHOP_GOODS_BUY_ACK(2147487768));
                    }
                    else if (DaoManagerSQL.UpdateAccountValuable(Player.PlayerId, (Player.Gold - gold), (Player.Cash - cash), (Player.Tags - tags)))
                    {
                        Player.Gold -= gold;
                        Player.Cash -= cash;
                        Player.Tags -= tags;
                        if (DaoManagerSQL.GetMessagesCount(TargetUser.PlayerId) >= 100)
                        {
                            Client.SendPacket(new PROTOCOL_AUTH_SHOP_GOODS_BUY_ACK(2147487929));
                            return;
                        }
                        else
                        {
                            MessageModel MessageTarget = CreateMessage(Player.Nickname, TargetUser.PlayerId, Client.PlayerId);
                            if (MessageTarget != null)
                            {
                                TargetUser.SendPacket(new PROTOCOL_MESSENGER_NOTE_RECEIVE_ACK(MessageTarget), false);
                            }
                            foreach (GoodsItem Good in Goods)
                            {
                                TargetUser.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(1, TargetUser, Good.Item), false);
                            }
                            Client.SendPacket(new PROTOCOL_AUTH_SHOP_GOODS_GIFT_ACK(1, Goods, TargetUser));
                        }
                        Client.SendPacket(new PROTOCOL_AUTH_GET_POINT_CASH_ACK(0, Player));
                    }
                    else
                    {
                        Client.SendPacket(new PROTOCOL_AUTH_SHOP_GOODS_BUY_ACK(2147487769));
                    }
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
        private MessageModel CreateMessage(string senderName, long owner, long senderId)
        {
            MessageModel msg = new MessageModel(15)
            {
                SenderName = senderName,
                SenderId = senderId,
                Text = Message,
                State = NoteMessageState.Unreaded
            };
            if (!DaoManagerSQL.CreateMessage(owner, msg))
            {
                Client.SendPacket(new PROTOCOL_AUTH_SHOP_GOODS_BUY_ACK(0x80000000));
                return null;
            }
            return msg;
        }
    }
}
