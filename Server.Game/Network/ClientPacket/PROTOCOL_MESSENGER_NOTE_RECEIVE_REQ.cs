using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_MESSENGER_NOTE_RECEIVE_REQ : GameClientPacket
    {
        private long ReceiverId;
        private string MessageText;
        private uint Error;
        public PROTOCOL_MESSENGER_NOTE_RECEIVE_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            ReceiverId = ReadQ();
            MessageText = ReadU(ReadC() * 2);
        }
        public override void Run()
        {
            try
            {
                Account Player = Client.Player;
                if (Player == null || Client.PlayerId == ReceiverId)
                {
                    return;
                }
                Account Receiver = AccountManager.GetAccount(ReceiverId, 31);
                if (Receiver != null)
                {
                    if (DaoManagerSQL.GetMessagesCount(Receiver.PlayerId) >= 100)
                    {
                        Error = 2147487871;
                    }
                    else
                    {
                        MessageModel Message = CreateMessage(Player.Nickname, Receiver.PlayerId, Client.PlayerId);
                        if (Message != null)
                        {
                            Receiver.SendPacket(new PROTOCOL_MESSENGER_NOTE_RECEIVE_ACK(Message), false);
                        }
                    }
                }
                else
                {
                    Error = 2147487870;
                }
                Client.SendPacket(new PROTOCOL_MESSENGER_NOTE_SEND_ACK(Error));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_MESSENGER_NOTE_RECEIVE_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
        private MessageModel CreateMessage(string SenderName, long OwnerId, long SenderId)
        {
            MessageModel Message = new MessageModel(15)
            {
                SenderName = SenderName,
                SenderId = SenderId,
                Text = MessageText,
                State = NoteMessageState.Unreaded
            };
            if (!DaoManagerSQL.CreateMessage(OwnerId, Message))
            {
                Error = 0x80000000;
                return null;
            }
            return Message;
        }
    }
}