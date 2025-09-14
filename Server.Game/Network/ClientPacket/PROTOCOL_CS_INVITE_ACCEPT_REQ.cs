using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Utility;
using Server.Game.Data.Managers;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Plugin.Core.SQL;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_INVITE_ACCEPT_REQ : GameClientPacket
    {
        private int clanId, type;
        public PROTOCOL_CS_INVITE_ACCEPT_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            clanId = ReadD();
            type = ReadC();
        }
        public override void Run()
        {
            Account Player = Client.Player;
            if (Player == null || Player.Nickname.Length == 0)
            {
                return;
            }
            ClanModel Clan = ClanManager.GetClan(clanId);
            List<Account> clanPlayers = ClanManager.GetClanPlayers(clanId, -1, true);
            if (Clan.Id == 0)
            {
                Client.SendPacket(new PROTOCOL_CS_INVITE_ACCEPT_ACK(2147487835));
            }
            else if (Player.ClanId > 0)
            {
                Client.SendPacket(new PROTOCOL_CS_INVITE_ACCEPT_ACK(2147487832));
            }
            else if (Clan.MaxPlayers <= clanPlayers.Count)
            {
                Client.SendPacket(new PROTOCOL_CS_INVITE_ACCEPT_ACK(2147487830));
            }
            else if (type == 0 || type == 1)
            {
                try
                {
                    uint erro = 0;
                    Account master = AccountManager.GetAccount(Clan.OwnerId, 31);
                    if (master != null)
                    {
                        if (DaoManagerSQL.GetMessagesCount(Clan.OwnerId) < 100)
                        {
                            MessageModel msg = CreateMessage(Clan, Player.Nickname, Client.PlayerId);
                            if (msg != null && master.IsOnline)
                            {
                                master.SendPacket(new PROTOCOL_MESSENGER_NOTE_RECEIVE_ACK(msg), false);
                            }
                        }
                        if (type == 1)
                        {
                            uint date = uint.Parse(DateTimeUtil.Now("yyyyMMdd"));
                            if (ComDiv.UpdateDB("accounts", "player_id", Player.PlayerId, new string[] { "clan_id", "clan_access", "clan_date" }, Clan.Id, 3, (long)date))
                            {
                                using (PROTOCOL_CS_MEMBER_INFO_INSERT_ACK packet = new PROTOCOL_CS_MEMBER_INFO_INSERT_ACK(Player))
                                {
                                    ClanManager.SendPacket(packet, clanPlayers);
                                }
                                Player.ClanId = Clan.Id;
                                Player.ClanDate = date;
                                Player.ClanAccess = 3;
                                Client.SendPacket(new PROTOCOL_CS_MEMBER_INFO_ACK(clanPlayers));
                                RoomModel room = Player.Room;
                                if (room != null)
                                {
                                    room.SendPacketToPlayers(new PROTOCOL_ROOM_GET_SLOTONEINFO_ACK(Player, Clan));
                                }
                                Client.SendPacket(new PROTOCOL_CS_ACCEPT_REQUEST_RESULT_ACK(Clan, master, clanPlayers.Count + 1));
                            }
                            else
                            {
                                erro = 0x80000000;
                            }
                        }
                    }
                    else
                    {
                        erro = 0x80000000;
                    }
                    Client.SendPacket(new PROTOCOL_MESSENGER_NOTE_SEND_ACK(erro));
                }
                catch (Exception Ex)
                {
                    CLogger.Print(Ex.Message, LoggerType.Error, Ex);
                }
            }
        }
        private MessageModel CreateMessage(ClanModel clan, string player, long senderId)
        {
            MessageModel msg = new MessageModel(15)
            {
                SenderName = clan.Name,
                SenderId = senderId,
                ClanId = clan.Id,
                Type = NoteMessageType.Clan,
                Text = player,
                State = NoteMessageState.Unreaded,
                ClanNote = type == 0 ? NoteMessageClan.JoinDenial : NoteMessageClan.JoinAccept
            };
            return DaoManagerSQL.CreateMessage(clan.OwnerId, msg) ? msg : null;
        }
    }
}