using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Utility;
using Server.Game.Data.Managers;
using Server.Game.Data.Sync.Server;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Server.Game.Data.Models;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_ACCEPT_REQUEST_REQ : GameClientPacket
    {
        private List<long> PlayerIds = new List<long>();
        private int result;
        public PROTOCOL_CS_ACCEPT_REQUEST_REQ(GameClient client, byte[] data)
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
                if (Clan.Id > 0 && (Player.ClanAccess >= 1 && Player.ClanAccess <= 2 || Player.PlayerId == Clan.OwnerId))
                {
                    List<Account> ClanPlayers = ClanManager.GetClanPlayers(Clan.Id, -1, true);
                    if (ClanPlayers.Count >= Clan.MaxPlayers)
                    {
                        result = -1;
                        return;
                    }
                    for (int i = 0; i < PlayerIds.Count; i++)
                    {
                        Account Member = AccountManager.GetAccount(PlayerIds[i], 31);
                        if (Member != null && ClanPlayers.Count < Clan.MaxPlayers && Member.ClanId == 0 && DaoManagerSQL.GetRequestClanId(Member.PlayerId) > 0)
                        {
                            using (PROTOCOL_CS_MEMBER_INFO_CHANGE_ACK packet = new PROTOCOL_CS_MEMBER_INFO_CHANGE_ACK(Member))
                            {
                                ClanManager.SendPacket(packet, ClanPlayers);
                            }
                            Member.ClanId = Player.ClanId;
                            Member.ClanDate = uint.Parse(DateTimeUtil.Now("yyyyMMdd"));
                            Member.ClanAccess = 3;
                            SendClanInfo.Load(Member, null, 3);
                            ComDiv.UpdateDB("accounts", "player_id", Member.PlayerId, new string[] { "clan_access", "clan_id", "clan_date" }, Member.ClanAccess, Member.ClanId, Member.ClanDate);
                            DaoManagerSQL.DeleteClanInviteDB(Player.ClanId, Member.PlayerId);
                            if (Member.IsOnline)
                            {
                                Member.SendPacket(new PROTOCOL_CS_MEMBER_INFO_ACK(ClanPlayers), false);
                                RoomModel r = Member.Room;
                                if (r != null)
                                {
                                    r.SendPacketToPlayers(new PROTOCOL_ROOM_GET_SLOTONEINFO_ACK(Member, Clan));
                                }
                                Member.SendPacket(new PROTOCOL_CS_ACCEPT_REQUEST_RESULT_ACK(Clan, ClanPlayers.Count + 1), false);
                            }
                            if (DaoManagerSQL.GetMessagesCount(Member.PlayerId) < 100)
                            {
                                MessageModel msg = CreateMessage(Clan, Member.PlayerId, Client.PlayerId);
                                if (msg != null && Member.IsOnline)
                                {
                                    Member.SendPacket(new PROTOCOL_MESSENGER_NOTE_RECEIVE_ACK(msg), false);
                                }
                            }
                            result++;
                            ClanPlayers.Add(Member);
                        }
                    }
                    ClanPlayers = null;
                }
                else
                {
                    result = -1;
                }
                Client.SendPacket(new PROTOCOL_CS_ACCEPT_REQUEST_ACK((uint)result));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_CS_ACCEPT_REQUEST_RESULT_REQ: {ex.Message}", LoggerType.Error, ex);
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
                ClanNote = NoteMessageClan.InviteAccept
            };
            return DaoManagerSQL.CreateMessage(owner, msg) ? msg : null;
        }
    }
}