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

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_AUTH_SHOP_ITEM_CHANGE_DATA_REQ : GameClientPacket
    {
        private long ObjectId;
        private uint Value, Error = 0;
        private byte[] Info;
        private string Text;
        public PROTOCOL_AUTH_SHOP_ITEM_CHANGE_DATA_REQ(GameClient Client, byte[] Buffer)
        {
            Makeme(Client, Buffer);
        }
        public override void Read()
        {
            ObjectId = ReadUD();
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
                ItemsModel item = Player.Inventory.GetItem(ObjectId);
                if (item != null && item.Id > 1600000)
                {
                    int cuponId = ComDiv.CreateItemId(16, 0, ComDiv.GetIdStatics(item.Id, 3));
                    if (cuponId == 1610047 || cuponId == 1610051 || cuponId == 1600010)
                    {
                        Text = Bitwise.HexArrayToString(Info, Info.Length);
                    }
                    else if (cuponId == 1610052 || cuponId == 1600005)
                    {
                        Value = BitConverter.ToUInt32(Info, 0);
                    }
                    else if (Info.Length > 0)
                    {
                        Value = Info[0];
                    }
                    CreateCouponEffects(cuponId, Player);
                }
                else
                {
                    Error = 0x80000000;
                }
                Client.SendPacket(new PROTOCOL_AUTH_SHOP_ITEM_CHANGE_DATA_ACK(Error));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_AUTH_SHOP_ITEM_CHANGE_DATA_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
        private void CreateCouponEffects(int CouponId, Account Player)
        {
            if (CouponId == 1610051)
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
            else if (CouponId == 1610052)
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
            else if (CouponId == 1800047)
            {
                if (string.IsNullOrEmpty(Text) || Text.Length < ConfigLoader.MinNickSize || Text.Length > ConfigLoader.MaxNickSize || Player.Inventory.GetItem(1600010) != null)
                {
                    Error = 0x80000000;
                }
                else if (!DaoManagerSQL.IsPlayerNameExist(Text))
                {
                    if (ComDiv.UpdateDB("accounts", "nickname", Text, "player_id", Player.PlayerId))
                    {
                        DaoManagerSQL.CreatePlayerNickHistory(Player.PlayerId, Player.Nickname, Text, "Changed (Coupon)");
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
                    Error = 2147483923;
                }
            }
            else if (CouponId == 1600006)
            {
                if (ComDiv.UpdateDB("accounts", "nick_color", (int)Value, "player_id", Player.PlayerId))
                {
                    Player.NickColor = (int)Value;
                    Client.SendPacket(new PROTOCOL_BASE_INV_ITEM_DATA_ACK(Info.Length, Player));
                    Client.SendPacket(new PROTOCOL_BASE_GET_MYINFO_BASIC_ACK(Player));
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
            else if (CouponId == 1600187)
            {
                if (ComDiv.UpdateDB("player_bonus", "muzzle_color", (int)Value, "owner_id", Player.PlayerId))
                {
                    Player.Bonus.MuzzleColor = (int)Value;
                    Client.SendPacket(new PROTOCOL_BASE_INV_ITEM_DATA_ACK(Info.Length, Player));
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
            else if (CouponId == 1610085)
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
            else
            {
                CLogger.Print($"Coupon effect not found! Id: {CouponId}", LoggerType.Warning);
                Error = 0x80000000;
            }
        }
    }
}
