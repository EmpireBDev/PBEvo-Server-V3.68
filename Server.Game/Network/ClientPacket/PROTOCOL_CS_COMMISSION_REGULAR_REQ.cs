using Plugin.Core;
using Plugin.Core.Managers;
using Plugin.Core.Enums;
using Plugin.Core.Utility;
using Server.Game.Data.Managers;
using Server.Game.Data.Sync.Server;
using Server.Game.Network.ServerPacket;
using System;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Plugin.Core.SQL;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_COMMISSION_REGULAR_REQ : GameClientPacket
    {
        private List<long> PlayerIds = new List<long>();
        private uint result;
        public PROTOCOL_CS_COMMISSION_REGULAR_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            int CountPlayers = ReadC();
            for (int i = 0; i < CountPlayers; i++)
            {
                long PlayerId = ReadQ();
                PlayerIds.Add(PlayerId);
            }
        }
        public override void Run()
        {
            try
            {
                Account player = Client.Player;
                if (player == null)
                {
                    return;
                }
                ClanModel clan = ClanManager.GetClan(player.ClanId);
                if (clan.Id == 0 || !(player.ClanAccess >= 1 && player.ClanAccess <= 2 || clan.OwnerId == Client.PlayerId))
                {
                    result = 2147487833;
                    return;
                }
                for (int i = 0; i < PlayerIds.Count; i++)
                {
                    Account member = AccountManager.GetAccount(PlayerIds[i], 31);
                    if (member != null && member.ClanId == clan.Id && member.ClanAccess == 2 && ComDiv.UpdateDB("accounts", "clan_access", 3, "player_id", member.PlayerId))
                    {
                        member.ClanAccess = 3;
                        SendClanInfo.Load(member, null, 3);
                        if (DaoManagerSQL.GetMessagesCount(member.PlayerId) < 100)
                        {
                            MessageModel msg = CreateMessage(clan, member.PlayerId, Client.PlayerId);
                            if (msg != null && member.IsOnline)
                            {
                                member.SendPacket(new PROTOCOL_MESSENGER_NOTE_RECEIVE_ACK(msg), false);
                            }
                        }
                        if (member.IsOnline)
                        {
                            member.SendPacket(new PROTOCOL_CS_COMMISSION_REGULAR_RESULT_ACK(), false);
                        }
                        result++;
                    }
                }
                Client.SendPacket(new PROTOCOL_CS_COMMISSION_REGULAR_ACK(result));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_CS_COMMISSION_REGULAR_REQ: {ex.Message}", LoggerType.Error, ex);
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
                ClanNote = NoteMessageClan.Regular
            };
            return DaoManagerSQL.CreateMessage(owner, msg) ? msg : null;
        }
    }
}