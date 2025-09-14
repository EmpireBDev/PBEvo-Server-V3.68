using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Managers;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_SHOP_GET_SAILLIST_REQ : GameClientPacket
    {
        private string MD5Hash;
        public PROTOCOL_SHOP_GET_SAILLIST_REQ(GameClient Client, byte[] Data)
        {
            Makeme(Client, Data);
        }
        public override void Read()
        {
            MD5Hash = ReadS(32);
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
                if (!Player.LoadedShop)
                {
                    Player.LoadedShop = true;
                    foreach (IEnumerable<ShopData> Item in ShopManager.ShopDataItems.Split(800))
                    {
                        foreach (ShopData Data in Item.ToList())
                        {
                            Client.SendPacket(new PROTOCOL_AUTH_SHOP_ITEMLIST_ACK(Data, ShopManager.TotalItems));
                        }
                    }
                    foreach (IEnumerable<ShopData> Good in ShopManager.ShopDataGoods.Split(50))
                    {
                        foreach (ShopData Data in Good.ToList())
                        {
                            Client.SendPacket(new PROTOCOL_AUTH_SHOP_GOODSLIST_ACK(Data, ShopManager.TotalGoods));
                        }
                    }
                    foreach (IEnumerable<ShopData> Repair in ShopManager.ShopDataItemRepairs.Split(100))
                    {
                        foreach (ShopData Data in Repair.ToList())
                        {
                            Client.SendPacket(new PROTOCOL_AUTH_SHOP_REPAIRLIST_ACK(Data, ShopManager.TotalRepairs));
                        }
                    }
                    foreach (IEnumerable<ShopData> BattleBox in ShopManager.ShopDataBattleBoxes.Split(100))
                    {
                        foreach (ShopData Data in BattleBox.ToList())
                        {
                            Client.SendPacket(new PROTOCOL_BATTLEBOX_GET_LIST_ACK(Data, ShopManager.TotalBoxes));
                        }
                    }
                    if (Player.CafePC == CafeEnum.None)
                    {
                        foreach (IEnumerable<ShopData> Matching in ShopManager.ShopDataMt1.Split(500))
                        {
                            foreach (ShopData Data in Matching.ToList())
                            {
                                Client.SendPacket(new PROTOCOL_AUTH_SHOP_MATCHINGLIST_ACK(Data, ShopManager.TotalMatching1));
                            }
                        }
                    }
                    else
                    {
                        foreach (IEnumerable<ShopData> Matching in ShopManager.ShopDataMt2.Split(500))
                        {
                            foreach (ShopData Data in Matching.ToList())
                            {
                                Client.SendPacket(new PROTOCOL_AUTH_SHOP_MATCHINGLIST_ACK(Data, ShopManager.TotalMatching2));
                            }
                        }
                    }
                }
                Client.SendPacket(new PROTOCOL_SHOP_TAG_INFO_ACK());
                //Client.SendPacket(new PROTOCOL_SHOP_ACCOUNT_LIMITED_SALE_ACK());
                //Client.SendPacket(new PROTOCOL_SHOP_LIMITED_SALE_LIST_ACK());
                //Client.SendPacket(new PROTOCOL_SHOP_LIMITED_SALE_SYNC_ACK());
                //Client.SendPacket(new PROTOCOL_SHOP_FLASH_SALE_LIST_ACK());
                if (Bitwise.ReadFile($"{Environment.CurrentDirectory}/Data/Raws/Shop.dat") == MD5Hash)
                {
                    Client.SendPacket(new PROTOCOL_SHOP_GET_SAILLIST_ACK(false));
                }
                else
                {
                    Client.SendPacket(new PROTOCOL_SHOP_GET_SAILLIST_ACK(true));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_SHOP_GET_SAILLIST_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}