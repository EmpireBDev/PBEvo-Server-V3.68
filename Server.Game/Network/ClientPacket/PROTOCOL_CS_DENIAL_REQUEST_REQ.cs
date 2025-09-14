using Plugin.Core;
using Plugin.Core.Managers;
using Plugin.Core.Enums;
using Server.Game.Data.Managers;
using Server.Game.Network.ServerPacket;
using System;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Server.Game.Data.Models;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_DENIAL_REQUEST_REQ : GameClientPacket
    {
        private List<long> PlayerIds = new List<long>();
        private int result;
        public PROTOCOL_CS_DENIAL_REQUEST_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            int countPlayers = ReadC();
            for (int i = 0; i < countPlayers; i++)
            {
                long playerId = ReadQ();
                PlayerIds.Add(playerId);
            }
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
                ClanModel Clan = ClanManager.GetClan(Player.ClanId);
                if (Clan.Id > 0 && (Player.ClanAccess >= 1 && Player.ClanAccess <= 2 || Clan.OwnerId == Player.PlayerId))
                {
                    for (int i = 0; i < PlayerIds.Count; i++)
                    {
                        long PlayerId = PlayerIds[i];
                        if (DaoManagerSQL.DeleteClanInviteDB(Clan.Id, PlayerId))
                        {
                            if (DaoManagerSQL.GetMessagesCount(PlayerId) < 100)
                            {
                                MessageModel msg = CreateMessage(Clan, PlayerId, Player.PlayerId);
                                if (msg != null)
                                {
                                    Account pK = AccountManager.GetAccount(PlayerId, 31);
                                    if (pK != null && pK.IsOnline)
                                    {
                                        pK.SendPacket(new PROTOCOL_MESSENGER_NOTE_RECEIVE_ACK(msg), false);
                                    }
                                }
                            }
                            result++;
                        }
                    }
                }
                Client.SendPacket(new PROTOCOL_CS_DENIAL_REQUEST_ACK(result));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_CS_DENIAL_REQUEST_REQ: {ex.Message}", LoggerType.Error, ex);
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
                State = NoteMessageState.Unreaded,
                ClanNote = NoteMessageClan.InviteDenial
            };
            return DaoManagerSQL.CreateMessage(owner, msg) ? msg : null;
        }
    }
}