using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Utility;
using Server.Game.Data.Managers;
using Server.Game.Data.Sync.Server;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Plugin.Core.SQL;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_DEPORTATION_REQ : GameClientPacket
    {
        private List<long> PlayerIds = new List<long>();
        private uint result;
        public PROTOCOL_CS_DEPORTATION_REQ(GameClient client, byte[] data)
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
                Account Player = Client.Player;
                if (Player == null)
                {
                    return;
                }
                ClanModel Clan = ClanManager.GetClan(Player.ClanId);
                if (Clan.Id == 0 || !(Player.ClanAccess >= 1 && Player.ClanAccess <= 2 || Clan.OwnerId == Client.PlayerId))
                {
                    result = 2147487833;
                    return;
                }
                List<Account> ClanPlayers = ClanManager.GetClanPlayers(Clan.Id, -1, true);
                for (int i = 0; i < PlayerIds.Count; i++)
                {
                    Account member = AccountManager.GetAccount(PlayerIds[i], 31);
                    if (member != null && member.ClanId == Clan.Id && member.Match == null && ComDiv.UpdateDB("accounts", "player_id", member.PlayerId, new string[] { "clan_id", "clan_access" }, 0, 0) && ComDiv.UpdateDB("player_stat_clans", "owner_id", member.PlayerId, new string[] { "clan_matches", "clan_match_wins" }, 0, 0))
                    {
                        using (PROTOCOL_CS_MEMBER_INFO_DELETE_ACK packet = new PROTOCOL_CS_MEMBER_INFO_DELETE_ACK(member.PlayerId))
                        {
                            ClanManager.SendPacket(packet, ClanPlayers, member.PlayerId);
                        }
                        member.ClanId = 0;
                        member.ClanAccess = 0;
                        SendClanInfo.Load(member, null, 0);
                        if (DaoManagerSQL.GetMessagesCount(member.PlayerId) < 100)
                        {
                            MessageModel msg = CreateMessage(Clan, member.PlayerId, Client.PlayerId);
                            if (msg != null && member.IsOnline)
                            {
                                member.SendPacket(new PROTOCOL_MESSENGER_NOTE_RECEIVE_ACK(msg), false);
                            }
                        }
                        if (member.IsOnline)
                        {
                            member.SendPacket(new PROTOCOL_CS_DEPORTATION_RESULT_ACK(), false);
                        }
                        result++;
                        ClanPlayers.Remove(member);
                    }
                    else
                    {
                        result = 2147487833;
                        break;
                    }
                }
                Client.SendPacket(new PROTOCOL_CS_DEPORTATION_ACK(result));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_CS_DEPORTATION_REQ: {ex.Message}", LoggerType.Error, ex);
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
                ClanNote = NoteMessageClan.Deportation
            };
            return DaoManagerSQL.CreateMessage(owner, msg) ? msg : null;
        }
    }
}
