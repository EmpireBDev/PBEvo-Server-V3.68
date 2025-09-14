using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Managers;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CHAR_CREATE_CHARA_REQ : GameClientPacket
    {
        private string Name;
        private List<CartGoods> ShopCart = new List<CartGoods>();
        public PROTOCOL_CHAR_CREATE_CHARA_REQ(GameClient Client, byte[] Buffer)
        {
            Makeme(Client, Buffer);
        }
        public override void Read()
        {
            ReadC();
            Name = ReadU(ReadC() * 2);
            ReadC();
            CartGoods Cart = new CartGoods()
            {
                GoodId = ReadD(),
                BuyType = ReadC()
            };
            ReadC();
            ShopCart.Add(Cart);
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
                if (Player.Inventory.Items.Count >= 500 || Player.Character.Characters.Count >= 64)
                {
                    Client.SendPacket(new PROTOCOL_CHAR_CREATE_CHARA_ACK(0x800010B9, -1, null, null));
                    return;
                }
                List<GoodsItem> Goods = ShopManager.GetGoods(ShopCart, out int Gold, out int Cash, out int Tags);
                if (Goods.Count == 0)
                {
                    Client.SendPacket(new PROTOCOL_CHAR_CREATE_CHARA_ACK(0x80001017, -1, null, null));
                }
                else if (0 > (Player.Gold - Gold) || 0 > (Player.Cash - Cash) || 0 > (Player.Tags - Tags))
                {
                    Client.SendPacket(new PROTOCOL_CHAR_CREATE_CHARA_ACK(0x80001018, -1, null, null));
                }
                else if (DaoManagerSQL.UpdateAccountValuable(Player.PlayerId, (Player.Gold - Gold), (Player.Cash - Cash), (Player.Tags - Tags)))
                {
                    Player.Gold -= Gold;
                    Player.Cash -= Cash;
                    Player.Tags -= Tags;
                    CharacterModel Chara = GetCharaCache(Goods, Player.Character.Characters.Count);
                    if (Chara != null)
                    {
                        Player.Character.AddCharacter(Chara);
                        if (Player.Character.GetCharacter(Chara.Id) != null)
                        {
                            DaoManagerSQL.CreatePlayerCharacter(Chara, Player.PlayerId);
                        }
                    }
                    foreach (GoodsItem Good in Goods)
                    {
                        Client.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(1, Player, Good.Item));
                    }
                    Client.SendPacket(new PROTOCOL_CHAR_CREATE_CHARA_ACK(0, 1, Chara, Player));
                }
                else
                {
                    Client.SendPacket(new PROTOCOL_CHAR_CREATE_CHARA_ACK(0x80001019, -1, null, null));
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        private CharacterModel GetCharaCache(List<GoodsItem> Goods, int Slot)
        {
            foreach (GoodsItem Good in Goods)
            {
                if (Good != null && Good.Item.Id != 0)
                {
                    CharacterModel Chara = new CharacterModel()
                    {
                        Id = Good.Item.Id,
                        Slot = Slot++,
                        Name = Name,
                        CreateDate = uint.Parse(DateTimeUtil.Now("yyMMddHHmm")),
                        PlayTime = 0
                    };
                    return Chara;
                }
            }
            return null;
        }
    }
}
