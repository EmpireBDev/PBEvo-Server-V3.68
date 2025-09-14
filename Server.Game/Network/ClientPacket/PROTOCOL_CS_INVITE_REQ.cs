using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Managers;
using Server.Game.Network.ServerPacket;
using System;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Plugin.Core.SQL;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_INVITE_REQ : GameClientPacket
    {
        private uint erro;
        public PROTOCOL_CS_INVITE_REQ(GameClient client, byte[] data)
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
                Account f = TempBuffer.Get();
                if (f != null)
                {
                    if (f.ClanId == 0 && Player.ClanId != 0)
                    {
                        SendBoxMessage(f, Player.ClanId);
                    }
                    else
                    {
                        erro = 0x80000000;
                    }
                }
                else
                {
                    erro = 0x80000000;
                }
                Client.SendPacket(new PROTOCOL_CS_INVITE_ACK(erro));
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        private void SendBoxMessage(Account player, int clanId)
        {
            if (DaoManagerSQL.GetMessagesCount(player.PlayerId) >= 100)
            {
                erro = 0x80000000;
            }
            else
            {
                MessageModel msg = CreateMessage(clanId, player.PlayerId, Client.PlayerId);
                if (msg != null && player.IsOnline)
                {
                    player.SendPacket(new PROTOCOL_MESSENGER_NOTE_RECEIVE_ACK(msg), false);
                }
            }
        }
        private MessageModel CreateMessage(int clanId, long owner, long senderId)
        {
            MessageModel msg = new MessageModel(15)
            {
                SenderName = ClanManager.GetClan(clanId).Name,
                ClanId = clanId,
                SenderId = senderId,
                Type = NoteMessageType.ClanAsk,
                State = NoteMessageState.Unreaded,
                ClanNote = NoteMessageClan.Invite
            };
            return DaoManagerSQL.CreateMessage(owner, msg) ? msg : null;
        }
    }
}