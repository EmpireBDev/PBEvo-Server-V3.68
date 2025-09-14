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
    public class PROTOCOL_CS_NOTE_REQ : GameClientPacket
    {
        private int type;
        private string message;
        public PROTOCOL_CS_NOTE_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            type = ReadC();
            message = ReadU(ReadC() * 2);
        }
        public override void Run()
        {
            try
            {
                Account player = Client.Player;
                if (message.Length > 120 || player == null)
                {
                    return;
                }
                ClanModel clan = ClanManager.GetClan(player.ClanId);
                int playersLoaded = 0;
                if (clan.Id > 0 && clan.OwnerId == Client.PlayerId)
                {
                    List<Account> players = ClanManager.GetClanPlayers(clan.Id, Client.PlayerId, true);
                    for (int i = 0; i < players.Count; i++)
                    {
                        Account member = players[i];
                        if ((type == 0 || member.ClanAccess == 2 && type == 1 || member.ClanAccess == 3 && type == 2) && DaoManagerSQL.GetMessagesCount(member.PlayerId) < 100)
                        {
                            ++playersLoaded;
                            MessageModel msg = CreateMessage(clan, member.PlayerId, Client.PlayerId);
                            if (msg != null && member.IsOnline)
                            {
                                member.SendPacket(new PROTOCOL_MESSENGER_NOTE_RECEIVE_ACK(msg), false);
                            }
                        }
                    }
                }
                Client.SendPacket(new PROTOCOL_CS_NOTE_ACK(playersLoaded));
                if (playersLoaded > 0)
                {
                    Client.SendPacket(new PROTOCOL_MESSENGER_NOTE_SEND_ACK(0));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_CS_NOTE_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
        private MessageModel CreateMessage(ClanModel clan, long owner, long senderId)
        {
            MessageModel msg = new MessageModel(15)
            {
                SenderName = clan.Name,
                SenderId = senderId,
                ClanId = clan.Id,
                Type = NoteMessageType.Clan,
                Text = message,
                State = NoteMessageState.Unreaded
            };
            return DaoManagerSQL.CreateMessage(owner, msg) ? msg : null;
        }
    }
}