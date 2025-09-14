using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Plugin.Core.Utility;
using Plugin.Core.XML;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_AUTH_SHOP_DELETE_ITEM_REQ : GameClientPacket
    {
        private long objId;
        private uint erro = 1;
        public PROTOCOL_AUTH_SHOP_DELETE_ITEM_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            objId = ReadUD();
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
                ItemsModel Item = Player.Inventory.GetItem(objId);
                PlayerBonus Bonus = Player.Bonus;
                if (Item == null)
                {
                    erro = 0x80000000;
                }
                else if (ComDiv.GetIdStatics(Item.Id, 1) == 16)
                {
                    if (Bonus == null)
                    {
                        Client.SendPacket(new PROTOCOL_AUTH_SHOP_DELETE_ITEM_ACK(0x80000000));
                        return;
                    }
                    bool changed = Bonus.RemoveBonuses(Item.Id);
                    if (!changed)
                    {
                        if (Item.Id == 1600014)
                        {
                            if (ComDiv.UpdateDB("player_bonus", "crosshair_color", 4, "owner_id", Player.PlayerId))
                            {
                                Bonus.CrosshairColor = 4;
                                Client.SendPacket(new PROTOCOL_BASE_INV_ITEM_DATA_ACK(0, Player));
                                Client.SendPacket(new PROTOCOL_BASE_GET_MYINFO_BASIC_ACK(Player));
                            }
                            else
                            {
                                erro = 0x80000000;
                            }
                        }
                        else if (Item.Id == 1600010)
                        {
                            if (Bonus.FakeNick.Length == 0)
                            {
                                erro = 0x80000000;
                            }
                            else
                            {
                                if (ComDiv.UpdateDB("accounts", "nickname", Bonus.FakeNick, "player_id", Player.PlayerId) && ComDiv.UpdateDB("player_bonus", "fake_nick", "", "owner_id", Player.PlayerId))
                                {
                                    Player.Nickname = Bonus.FakeNick;
                                    Bonus.FakeNick = "";
                                    Client.SendPacket(new PROTOCOL_BASE_INV_ITEM_DATA_ACK(0, Player));
                                    Client.SendPacket(new PROTOCOL_AUTH_CHANGE_NICKNAME_ACK(Player.Nickname));
                                    Client.SendPacket(new PROTOCOL_BASE_GET_MYINFO_BASIC_ACK(Player));
                                    RoomModel room = Player.Room;
                                    if (room != null)
                                    {
                                        using (PROTOCOL_ROOM_GET_NICKNAME_ACK packet = new PROTOCOL_ROOM_GET_NICKNAME_ACK(Player.SlotId, Player.Nickname))
                                        {
                                            room.SendPacketToPlayers(packet);
                                        }
                                        room.UpdateSlotsInfo();
                                    }
                                }
                                else
                                {
                                    erro = 0x80000000;
                                }
                            }
                        }
                        else if (Item.Id == 1600009)
                        {
                            if (ComDiv.UpdateDB("player_bonus", "fake_rank", 55, "owner_id", Player.PlayerId))
                            {
                                Bonus.FakeRank = 55;
                                Client.SendPacket(new PROTOCOL_BASE_INV_ITEM_DATA_ACK(0, Player));
                                Client.SendPacket(new PROTOCOL_BASE_GET_MYINFO_BASIC_ACK(Player));
                                RoomModel room = Player.Room;
                                if (room != null)
                                {
                                    using (PROTOCOL_ROOM_GET_RANK_ACK packet = new PROTOCOL_ROOM_GET_RANK_ACK(Player.SlotId, Bonus.MuzzleColor))
                                    {
                                        room.SendPacketToPlayers(packet);
                                    }
                                    room.UpdateSlotsInfo();
                                }
                            }
                            else
                            {
                                erro = 0x80000000;
                            }
                        }
                        else if (Item.Id == 1600187)
                        {
                            if (ComDiv.UpdateDB("player_bonus", "muzzle_color", 0, "owner_id", Player.PlayerId))
                            {
                                Bonus.MuzzleColor = 0;
                                Client.SendPacket(new PROTOCOL_BASE_INV_ITEM_DATA_ACK(0, Player));
                                RoomModel room = Player.Room;
                                if (room != null)
                                {
                                    using (PROTOCOL_ROOM_GET_COLOR_MUZZLE_FLASH_ACK packet = new PROTOCOL_ROOM_GET_COLOR_MUZZLE_FLASH_ACK(Player.SlotId, Bonus.MuzzleColor))
                                    {
                                        room.SendPacketToPlayers(packet);
                                    }
                                    room.UpdateSlotsInfo();
                                }
                            }
                            else
                            {
                                erro = 0x80000000;
                            }
                        }
                        else if (Item.Id == 1600006)
                        {
                            if (ComDiv.UpdateDB("accounts", "nick_color", 0, "owner_id", Player.PlayerId))
                            {
                                Player.NickColor = 0;
                                Client.SendPacket(new PROTOCOL_BASE_INV_ITEM_DATA_ACK(0, Player));
                                Client.SendPacket(new PROTOCOL_BASE_GET_MYINFO_BASIC_ACK(Player));
                                RoomModel room = Player.Room;
                                if (room != null)
                                {
                                    using (PROTOCOL_ROOM_GET_COLOR_NICK_ACK packet = new PROTOCOL_ROOM_GET_COLOR_NICK_ACK(Player.SlotId, Player.NickColor))
                                    {
                                        room.SendPacketToPlayers(packet);
                                    }
                                    room.UpdateSlotsInfo();
                                }
                            }
                            else
                            {
                                erro = 0x80000000;
                            }
                        }
                    }
                    else
                    {
                        DaoManagerSQL.UpdatePlayerBonus(Player.PlayerId, Bonus.Bonuses, Bonus.FreePass);
                    }
                    CouponFlag cupom = CouponEffectXML.GetCouponEffect(Item.Id);
                    if (cupom != null && cupom.EffectFlag > 0 && Player.Effects.HasFlag(cupom.EffectFlag))
                    {
                        Player.Effects -= cupom.EffectFlag;
                        DaoManagerSQL.UpdateCouponEffect(Player.PlayerId, Player.Effects);
                    }
                }
                if (erro == 1 && Item != null)
                {
                    if (DaoManagerSQL.DeletePlayerInventoryItem(Item.ObjectId, Player.PlayerId))
                    {
                        Player.Inventory.RemoveItem(Item);
                    }
                    else
                    {
                        erro = 0x80000000;
                    }
                }
                Client.SendPacket(new PROTOCOL_AUTH_SHOP_DELETE_ITEM_ACK(erro, objId));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_AUTH_SHOP_DELETE_ITEM_ACK: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
