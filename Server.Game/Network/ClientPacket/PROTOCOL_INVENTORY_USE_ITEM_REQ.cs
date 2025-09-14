using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Plugin.Core.Utility;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Data.Utils;
using Server.Game.Network.ServerPacket;
using System;
using System.Text;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_INVENTORY_USE_ITEM_REQ : GameClientPacket
    {
        private long ObjectId;
        private uint Value, Error = 1;
        private byte[] Info;
        private string Text;
        public PROTOCOL_INVENTORY_USE_ITEM_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            ObjectId = ReadD();
            Info = ReadB(ReadC());
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
                ItemsModel Item = Player.Inventory.GetItem(ObjectId);
                if (Item != null && Item.Id > 1700000)
                {
                    int cuponId = ComDiv.CreateItemId(16, 0, ComDiv.GetIdStatics(Item.Id, 3));
                    uint cuponDays = Convert.ToUInt32(DateTimeUtil.Now().AddDays(ComDiv.GetIdStatics(Item.Id, 2)).ToString("yyMMddHHmm"));
                    if (cuponId == 1600047 || cuponId == 1600051 || cuponId == 1600010)
                    {
                        Text = Bitwise.HexArrayToString(Info, Info.Length);
                    }
                    else if (cuponId == 1600052 || cuponId == 1600005)
                    {
                        Value = BitConverter.ToUInt32(Info, 0);
                    }
                    else if (Info.Length > 0)
                    {
                        Value = Info[0];
                    }
                    CreateCouponEffects(cuponId, cuponDays, Player);
                }
                else
                {
                    Error = 0x80000000;
                }
                Client.SendPacket(new PROTOCOL_AUTH_SHOP_ITEM_AUTH_ACK(Error, Item, Player));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_INVENTORY_USE_ITEM_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
        private void CreateCouponEffects(int CouponId, uint CouponDays, Account Player)
        {
            if (CouponId == 1600051)
            {
                if (string.IsNullOrEmpty(Text) || Text.Length > 16)
                {
                    Error = 0x80000000;
                }
                else
                {
                    ClanModel Clan = ClanManager.GetClan(Player.ClanId);
                    if (Clan.Id > 0 && Clan.OwnerId == Client.PlayerId)
                    {
                        if (!ClanManager.IsClanNameExist(Text) && ComDiv.UpdateDB("system_clan", "name", Text, "id", Player.ClanId))
                        {
                            Clan.Name = Text;
                            using (PROTOCOL_CS_REPLACE_NAME_RESULT_ACK packet = new PROTOCOL_CS_REPLACE_NAME_RESULT_ACK(Text))
                            {
                                ClanManager.SendPacket(packet, Player.ClanId, -1, true, true);
                            }
                        }
                        else
                        {
                            Error = 0x80000000;
                        }
                    }
                    else
                    {
                        Error = 0x80000000;
                    }
                }
            }
            else if (CouponId == 1600052)
            {
                ClanModel Clan = ClanManager.GetClan(Player.ClanId);
                if (Clan.Id > 0 && Clan.OwnerId == Client.PlayerId && !ClanManager.IsClanLogoExist(Value) && DaoManagerSQL.UpdateClanLogo(Player.ClanId, Value))
                {
                    Clan.Logo = Value;
                    using (PROTOCOL_CS_REPLACE_MARK_RESULT_ACK packet = new PROTOCOL_CS_REPLACE_MARK_RESULT_ACK(Value))
                    {
                        ClanManager.SendPacket(packet, Player.ClanId, -1, true, true);
                    }
                }
                else
                {
                    Error = 0x80000000;
                }
            }
            else if (CouponId == 1600047)
            {
                if (string.IsNullOrEmpty(Text) || Text.Length < ConfigLoader.MinNickSize || Text.Length > ConfigLoader.MaxNickSize || Player.Inventory.GetItem(1600010) != null)
                {
                    Error = 0x80000000;
                }
                else if (!DaoManagerSQL.IsPlayerNameExist(Text))
                {
                    if (ComDiv.UpdateDB("accounts", "nickname", Text, "player_id", Player.PlayerId))
                    {
                        DaoManagerSQL.CreatePlayerNickHistory(Player.PlayerId, Player.Nickname, Text, "Nickname changed (Item)");
                        Player.Nickname = Text;
                        Client.SendPacket(new PROTOCOL_AUTH_CHANGE_NICKNAME_ACK(Player.Nickname));
                        Client.SendPacket(new PROTOCOL_BASE_GET_MYINFO_BASIC_ACK(Player));
                        if (Player.Room != null)
                        {
                            using (PROTOCOL_ROOM_GET_NICKNAME_ACK packet = new PROTOCOL_ROOM_GET_NICKNAME_ACK(Player.SlotId, Player.Nickname))
                            {
                                Player.Room.SendPacketToPlayers(packet);
                            }
                            Player.Room.UpdateSlotsInfo();
                        }
                        if (Player.ClanId > 0)
                        {
                            using (PROTOCOL_CS_MEMBER_INFO_CHANGE_ACK packet = new PROTOCOL_CS_MEMBER_INFO_CHANGE_ACK(Player))
                            {
                                ClanManager.SendPacket(packet, Player.ClanId, -1, true, true);
                            }
                        }
                        AllUtils.SyncPlayerToFriends(Player, true);
                    }
                    else
                    {
                        Error = 0x80000000;
                    }
                }
                else
                {
                    Error = 0x80000113;
                }
            }
            else if (CouponId == 1600006)
            {
                if (ComDiv.UpdateDB("accounts", "nick_color", (int)Value, "player_id", Player.PlayerId))
                {
                    Player.NickColor = (int)Value;
                    Client.SendPacket(new PROTOCOL_BASE_INV_ITEM_DATA_ACK(Info.Length, Player));
                    Client.SendPacket(new PROTOCOL_BASE_GET_MYINFO_BASIC_ACK(Player));
                    Client.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(1, Player, new ItemsModel(CouponId, "Name Color [Active]", ItemEquipType.Temporary, CouponDays)));
                    if (Player.Room != null)
                    {
                        using (PROTOCOL_ROOM_GET_COLOR_NICK_ACK packet = new PROTOCOL_ROOM_GET_COLOR_NICK_ACK(Player.SlotId, Player.NickColor))
                        {
                            Player.Room.SendPacketToPlayers(packet);
                        }
                        Player.Room.UpdateSlotsInfo();
                    }
                }
                else
                {
                    Error = 0x80000000;
                }
            }
            else if (CouponId == 1600187)
            {
                if (ComDiv.UpdateDB("player_bonus", "muzzle_color", (int)Value, "owner_id", Player.PlayerId))
                {
                    Player.Bonus.MuzzleColor = (int)Value;
                    Client.SendPacket(new PROTOCOL_BASE_INV_ITEM_DATA_ACK(Info.Length, Player));
                    Client.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(1, Player, new ItemsModel(CouponId, "Muzzle Color [Active]", ItemEquipType.Temporary, CouponDays)));
                    if (Player.Room != null)
                    {
                        using (PROTOCOL_ROOM_GET_COLOR_MUZZLE_FLASH_ACK packet = new PROTOCOL_ROOM_GET_COLOR_MUZZLE_FLASH_ACK(Player.SlotId, Player.Bonus.MuzzleColor))
                        {
                            Player.Room.SendPacketToPlayers(packet);
                        }
                        Player.Room.UpdateSlotsInfo();
                    }
                }
                else
                {
                    Error = 0x80000000;
                }
            }
            else if (CouponId == 1600193)
            {
                ClanModel Clan = ClanManager.GetClan(Player.ClanId);
                if (Clan.Id > 0 && Clan.OwnerId == Client.PlayerId)
                {
                    if (ComDiv.UpdateDB("system_clan", "effect", (int)Value, "id", Player.ClanId))
                    {
                        Clan.Effect = (int)Value;
                        using (PROTOCOL_CS_REPLACE_MARKEFFECT_RESULT_ACK packet = new PROTOCOL_CS_REPLACE_MARKEFFECT_RESULT_ACK((int)Value))
                        {
                            ClanManager.SendPacket(packet, Player.ClanId, -1, true, true);
                        }
                        Client.SendPacket(new PROTOCOL_BASE_GET_MYINFO_BASIC_ACK(Player));
                    }
                    else
                    {
                        Error = 0x80000000;
                    }
                }
                else
                {
                    Error = 0x80000000;
                }
            }
            else if (CouponId == 1600009)
            {
                if ((int)Value >= 51 || (int)Value < Player.Rank - 10 || (int)Value > Player.Rank + 10)
                {
                    Error = 0x80000000;
                }
                else if (ComDiv.UpdateDB("player_bonus", "fake_rank", (int)Value, "owner_id", Player.PlayerId))
                {
                    Player.Bonus.FakeRank = (int)Value;
                    Client.SendPacket(new PROTOCOL_BASE_INV_ITEM_DATA_ACK(Info.Length, Player));
                    Client.SendPacket(new PROTOCOL_BASE_GET_MYINFO_BASIC_ACK(Player));
                    Client.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(1, Player, new ItemsModel(CouponId, "Fake Rank [Active]", ItemEquipType.Temporary, CouponDays)));
                    if (Player.Room != null)
                    {
                        using (PROTOCOL_ROOM_GET_RANK_ACK packet = new PROTOCOL_ROOM_GET_RANK_ACK(Player.SlotId, Player.GetRank()))
                        {
                            Player.Room.SendPacketToPlayers(packet);
                        }
                        Player.Room.UpdateSlotsInfo();
                    }
                }
                else
                {
                    Error = 0x80000000;
                }
            }
            else if (CouponId == 1600010)
            {
                if (string.IsNullOrEmpty(Text) || Text.Length < ConfigLoader.MinNickSize || Text.Length > ConfigLoader.MaxNickSize)
                {
                    Error = 0x80000000;
                }
                else if (ComDiv.UpdateDB("player_bonus", "fake_nick", Player.Nickname, "owner_id", Player.PlayerId) && ComDiv.UpdateDB("accounts", "nickname", Text, "player_id", Player.PlayerId))
                {
                    Player.Bonus.FakeNick = Player.Nickname;
                    Player.Nickname = Text;
                    Client.SendPacket(new PROTOCOL_BASE_INV_ITEM_DATA_ACK(Info.Length, Player));
                    Client.SendPacket(new PROTOCOL_AUTH_CHANGE_NICKNAME_ACK(Player.Nickname));
                    Client.SendPacket(new PROTOCOL_BASE_GET_MYINFO_BASIC_ACK(Player));
                    Client.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(1, Player, new ItemsModel(CouponId, "Fake Nick [Active]", ItemEquipType.Temporary, CouponDays)));
                    if (Player.Room != null)
                    {
                        using (PROTOCOL_ROOM_GET_NICKNAME_ACK packet = new PROTOCOL_ROOM_GET_NICKNAME_ACK(Player.SlotId, Player.Nickname))
                        {
                            Player.Room.SendPacketToPlayers(packet);
                        }
                        Player.Room.UpdateSlotsInfo();
                    }
                }
                else
                {
                    Error = 0x80000000;
                }
            }
            else if (CouponId == 1600014)
            {
                if (ComDiv.UpdateDB("player_bonus", "crosshair_color", (int)Value, "owner_id", Player.PlayerId))
                {
                    Player.Bonus.CrosshairColor = (int)Value;
                    Client.SendPacket(new PROTOCOL_BASE_INV_ITEM_DATA_ACK(Info.Length, Player));
                    Client.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(1, Player, new ItemsModel(CouponId, "Crosshair Color [Active]", ItemEquipType.Temporary, CouponDays)));
                }
                else
                {
                    Error = 0x80000000;
                }
            }
            else if (CouponId == 1600005)
            {
                ClanModel Clan = ClanManager.GetClan(Player.ClanId);
                if (Clan.Id > 0 && Clan.OwnerId == Client.PlayerId && ComDiv.UpdateDB("system_clan", "name_color", (int)Value, "id", Clan.Id))
                {
                    Clan.NameColor = (int)Value;
                    Client.SendPacket(new PROTOCOL_CS_REPLACE_COLOR_NAME_RESULT_ACK(Clan.NameColor));
                    Client.SendPacket(new PROTOCOL_BASE_GET_MYINFO_BASIC_ACK(Player));
                }
                else
                {
                    Error = 0x80000000;
                }
            }
            else if (CouponId == 1600085)
            {
                if (Player.Room != null)
                {
                    Account pR = Player.Room.GetPlayerBySlot((int)Value);
                    if (pR != null)
                    {
                        Client.SendPacket(new PROTOCOL_ROOM_GET_USER_ITEM_ACK(pR));
                    }
                    else
                    {
                        Error = 0x80000000;
                    }
                }
                else
                {
                    Error = 0x80000000;
                }
            }
            else if (CouponId == 1600183) 
            {
                if (!string.IsNullOrWhiteSpace(Text) && Text.Length <= 60 && !string.IsNullOrWhiteSpace(Player.Nickname))
                {
                    GameXender.Client.SendPacketToAllClients(new PROTOCOL_BASE_UNKNOWN_PACKET_1803_ACK(Player.Nickname, Text));
                }
                else
                {
                    Error = 0x80000000;
                }
            }
            else
            {
                CLogger.Print($"Coupon effect not found! Id: {CouponId}", LoggerType.Warning);
                Error = 0x80000000;
            }
        }
    }
}