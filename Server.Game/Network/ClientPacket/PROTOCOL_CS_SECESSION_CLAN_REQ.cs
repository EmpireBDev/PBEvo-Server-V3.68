using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Utility;
using Server.Game.Data.Managers;
using Server.Game.Network.ServerPacket;
using System;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Plugin.Core.SQL;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_SECESSION_CLAN_REQ : GameClientPacket
    {
        private uint erro;
        public PROTOCOL_CS_SECESSION_CLAN_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
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
                if (Player.ClanId > 0)
                {
                    ClanModel clan = ClanManager.GetClan(Player.ClanId);
                    if (clan.Id > 0 && clan.OwnerId != Player.PlayerId)
                    {
                        if (ComDiv.UpdateDB("accounts", "player_id", Player.PlayerId, new string[] { "clan_id", "clan_access" }, 0, 0) && ComDiv.UpdateDB("player_stat_clans", "owner_id", Player.PlayerId, new string[] { "clan_matches", "clan_match_wins" }, 0, 0))
                        {
                            using (PROTOCOL_CS_MEMBER_INFO_DELETE_ACK packet = new PROTOCOL_CS_MEMBER_INFO_DELETE_ACK(Player.PlayerId))
                            {
                                ClanManager.SendPacket(packet, Player.ClanId, Player.PlayerId, true, true);
                            }
                            long ownerId = clan.OwnerId;
                            if (DaoManagerSQL.GetMessagesCount(ownerId) < 100)
                            {
                                MessageModel Message = CreateMessage(clan, Player);
                                if (Message != null)
                                {
                                    Account member = AccountManager.GetAccount(ownerId, 31);
                                    if (member != null && member.IsOnline)
                                    {
                                        member.SendPacket(new PROTOCOL_MESSENGER_NOTE_RECEIVE_ACK(Message), false);
                                    }
                                }
                            }
                            Player.ClanId = 0;
                            Player.ClanAccess = 0;
                        }
                        else
                        {
                            erro = 0x8000106B;
                        }
                    }
                    else
                    {
                        erro = 2147487838;
                    }
                }
                else
                {
                    erro = 2147487835;
                }
                Client.SendPacket(new PROTOCOL_CS_SECESSION_CLAN_ACK(erro));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_CS_SECESSION_CLAN_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }

        private MessageModel CreateMessage(ClanModel clan, Account sender)
        {
            MessageModel msg = new MessageModel(15)
            {
                SenderName = clan.Name,
                SenderId = sender.PlayerId,
                ClanId = clan.Id,
                Type = NoteMessageType.Clan,
                Text = sender.Nickname,
                State = NoteMessageState.Unreaded,
                ClanNote = NoteMessageClan.Secession
            };
            return DaoManagerSQL.CreateMessage(clan.OwnerId, msg) ? msg : null;
        }
    }
}