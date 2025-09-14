using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Managers;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Plugin.Core.Utility;
using Plugin.Core.XML;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_AUTH_SHOP_USE_GIFTCOUPON_REQ : GameClientPacket
    {
        private string Token;
        public PROTOCOL_AUTH_SHOP_USE_GIFTCOUPON_REQ(GameClient Client, byte[] Buffer)
        {
            Makeme(Client, Buffer);
        }
        public override void Read()
        {
            Token = ReadS(ReadC());
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
                TicketModel Ticket = RedeemCodeXML.GetTicket(Token);
                if (Ticket != null)
                {
                    int UsedTicketByPlayer = ComDiv.CountDB($"SELECT COUNT(used_count) FROM base_redeem_history WHERE used_token = '{Ticket.Token}'");
                    if (UsedTicketByPlayer >= Ticket.TicketCount)
                    {
                        Client.SendPacket(new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK($"Oops! The Ticket has limit reached!"));
                    }
                    else
                    {
                        int UsedTicketByPlayerRation = DaoManagerSQL.GetUsedTicket(Player.PlayerId, Ticket.Token);
                        if (UsedTicketByPlayerRation < Ticket.PlayerRation)
                        {
                            UsedTicketByPlayerRation += 1;
                            if (Ticket.Type.HasFlag(TicketType.ITEM))
                            {
                                List<GoodsItem> Rewards = GetGoods(Ticket);
                                if (Rewards.Count > 0)
                                {
                                    if (ValidateGiftCoupon(Player, Ticket, UsedTicketByPlayerRation))
                                    {
                                        foreach (GoodsItem Good in Rewards)
                                        {
                                            Client.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(1, Player, Good.Item));
                                        }
                                        Client.SendPacket(new PROTOCOL_AUTH_SHOP_USE_GIFTCOUPON_ACK(0));
                                    }
                                }
                            }
                            if (Ticket.Type.HasFlag(TicketType.VALUE))
                            {
                                if (Ticket.GoldReward != 0 || Ticket.CashReward != 0)
                                {
                                    if (ValidateGiftCoupon(Player, Ticket, UsedTicketByPlayerRation))
                                    {
                                        if (DaoManagerSQL.UpdateAccountValuable(Player.PlayerId, (Player.Gold + Ticket.GoldReward), (Player.Cash + Ticket.CashReward), (Player.Tags + Ticket.TagsReward)))
                                        {
                                            Player.Gold += Ticket.GoldReward;
                                            Player.Cash += Ticket.CashReward;
                                            Player.Tags += Ticket.TagsReward;
                                        }
                                        Client.SendPacket(new PROTOCOL_AUTH_GET_POINT_CASH_ACK(0, Player));
                                        Client.SendPacket(new PROTOCOL_AUTH_SHOP_USE_GIFTCOUPON_ACK(0));
                                    }
                                }
                            }
                        }
                        else
                        {
                            Client.SendPacket(new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK($"Oops! You can only use this for {Ticket.PlayerRation} times!"));
                            Client.SendPacket(new PROTOCOL_AUTH_SHOP_USE_GIFTCOUPON_ACK(0x80000000));
                        }
                    }
                }
                else
                {
                    Client.SendPacket(new PROTOCOL_AUTH_SHOP_USE_GIFTCOUPON_ACK(0x80000000));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_AUTH_SHOP_USE_GIFTCOUPON_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
        public List<GoodsItem> GetGoods(TicketModel Ticket)
        {
            List<GoodsItem> Goods = new List<GoodsItem>();
            if (Ticket.Rewards.Count == 0)
            {
                return Goods;
            }
            foreach (int GoodId in Ticket.Rewards)
            {
                GoodsItem Good = ShopManager.GetGood(GoodId);
                if (Good != null)
                {
                    Goods.Add(Good);
                }
            }
            return Goods;
        }
        private bool ValidateGiftCoupon(Account Player, TicketModel Ticket, int UsedTicketByPlayerRation)
        {
            if (!DaoManagerSQL.IsTicketUsedByPlayer(Player.PlayerId, Ticket.Token))
            {
                return DaoManagerSQL.CreatePlayerRedeemHistory(Player.PlayerId, Ticket.Token, UsedTicketByPlayerRation);
            }
            else
            {
                return ComDiv.UpdateDB("base_redeem_history", "owner_id", Player.PlayerId, "used_token", Ticket.Token, new string[] { "used_count" }, UsedTicketByPlayerRation);
            }
        }
    }
}
