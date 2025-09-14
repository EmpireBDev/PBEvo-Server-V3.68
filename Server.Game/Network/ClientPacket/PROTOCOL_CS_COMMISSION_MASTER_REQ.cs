using Plugin.Core;
using Plugin.Core.Managers;
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
    public class PROTOCOL_CS_COMMISSION_MASTER_REQ : GameClientPacket
    {
        private long memberId;
        private uint erro;
        public PROTOCOL_CS_COMMISSION_MASTER_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            memberId = ReadQ();
        }
        public override void Run()
        {
            try
            {
                Account p = Client.Player;
                if (p == null || p.ClanAccess != 1)
                {
                    return;
                }
                Account member = AccountManager.GetAccount(memberId, 31);
                int clanId = p.ClanId;
                if (member == null || member.ClanId != clanId)
                {
                    erro = 0x80000000;
                }
                else if (member.Rank > 10)
                {
                    ClanModel clan = ClanManager.GetClan(clanId);
                    if (clan.Id > 0 && clan.OwnerId == Client.PlayerId && member.ClanAccess == 2 && ComDiv.UpdateDB("system_clan", "owner_id", memberId, "id", clanId) && ComDiv.UpdateDB("accounts", "clan_access", 1, "player_id", memberId) && ComDiv.UpdateDB("accounts", "clan_access", 2, "player_id", p.PlayerId))
                    {
                        member.ClanAccess = 1;
                        p.ClanAccess = 2;
                        clan.OwnerId = memberId;
                        if (DaoManagerSQL.GetMessagesCount(member.PlayerId) < 100)
                        {
                            MessageModel msg = CreateMessage(clan, member.PlayerId, p.PlayerId);
                            if (msg != null && member.IsOnline)
                            {
                                member.SendPacket(new PROTOCOL_MESSENGER_NOTE_RECEIVE_ACK(msg), false);
                            }
                        }
                        if (member.IsOnline)
                        {
                            member.SendPacket(new PROTOCOL_CS_COMMISSION_MASTER_RESULT_ACK(), false);
                        }
                    }
                    else
                    {
                        erro = 2147487744;
                    }
                }
                else
                {
                    erro = 2147487928;
                }
                Client.SendPacket(new PROTOCOL_CS_COMMISSION_MASTER_ACK(erro));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_CS_COMMISSION_MASTER_REQ: {ex.Message}", LoggerType.Error, ex);
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
                ClanNote = NoteMessageClan.Master
            };
            return DaoManagerSQL.CreateMessage(owner, msg) ? msg : null;
        }
    }
}