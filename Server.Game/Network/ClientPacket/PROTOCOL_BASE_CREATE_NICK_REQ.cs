using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Filters;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Plugin.Core.Utility;
using Plugin.Core.XML;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_CREATE_NICK_REQ : GameClientPacket
    {
        private string Nickname;
        public PROTOCOL_BASE_CREATE_NICK_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            Nickname = ReadU(ReadC() * 2);
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
                if ((Player.Nickname.Length == 0 && !string.IsNullOrEmpty(Nickname)) && (Nickname.Length >= ConfigLoader.MinNickSize && Nickname.Length <= ConfigLoader.MaxNickSize))
                {
                    foreach (string Exception in NickFilter.Filters)
                    {
                        if (Nickname.Contains(Exception))
                        {
                            Client.SendPacket(new PROTOCOL_BASE_CREATE_NICK_ACK(0x80001013, null));
                            return;
                        }
                    }
                    if (!DaoManagerSQL.IsPlayerNameExist(Nickname))
                    {
                        if (AccountManager.UpdatePlayerName(Nickname, Player.PlayerId))
                        {
                            DaoManagerSQL.CreatePlayerNickHistory(Player.PlayerId, Player.Nickname, Nickname, "First nick choosed");
                            Player.Nickname = Nickname;
                            List<ItemsModel> Awards = TemplatePackXML.Awards;
                            if (Awards.Count > 0)
                            {
                                foreach (ItemsModel Item in Awards)
                                {
                                    Client.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(1, Player, Item));
                                }
                            }
                            Client.SendPacket(new PROTOCOL_BASE_CREATE_NICK_ACK(0, Player));
                            foreach (ItemsModel Item in GetCharasList(TemplatePackXML.Basics))
                            {
                                int Slots = Player.Character.Characters.Count;
                                CharacterModel Character = new CharacterModel()
                                {
                                    Id = Item.Id,
                                    Name = Item.Name,
                                    Slot = Slots++,
                                    CreateDate = uint.Parse(DateTimeUtil.Now("yyMMddHHmm")),
                                    PlayTime = 0
                                };
                                Player.Character.AddCharacter(Character);
                                Client.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(1, Player, Item));
                                if (DaoManagerSQL.CreatePlayerCharacter(Character, Player.PlayerId))
                                {
                                    Client.SendPacket(new PROTOCOL_CHAR_CREATE_CHARA_ACK(0, 3, Character, Player));
                                }
                                else
                                {
                                    Client.SendPacket(new PROTOCOL_CHAR_CREATE_CHARA_ACK(0x80000000, -1, null, null));
                                }
                            }
                            Client.SendPacket(new PROTOCOL_SHOP_PLUS_POINT_ACK(Player.Gold, Player.Gold, 4));
                            Client.SendPacket(new PROTOCOL_BASE_QUEST_GET_INFO_ACK(Player));
                            Client.SendPacket(new PROTOCOL_AUTH_FRIEND_INFO_ACK(Player.Friend.Friends));
                        }
                        else
                        {
                            Client.SendPacket(new PROTOCOL_BASE_CREATE_NICK_ACK(0x80001013, null));
                        }
                    }
                    else
                    {
                        Client.SendPacket(new PROTOCOL_BASE_CREATE_NICK_ACK(0x80000113, null));
                    }
                }
                else
                {
                    Client.SendPacket(new PROTOCOL_BASE_CREATE_NICK_ACK(0x80000113, null));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_LOBBY_CREATE_NICK_NAME_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
        private List<ItemsModel> GetCharasList(List<ItemsModel> CharaList)
        {
            List<ItemsModel> Charas = new List<ItemsModel>();
            lock (CharaList)
            {
                foreach (ItemsModel Item in CharaList)
                {
                    if (Item != null && ComDiv.GetIdStatics(Item.Id, 1) == 6)
                    {
                        Charas.Add(Item);
                    }
                }
            }
            return Charas;
        }
    }
}