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
    public class PROTOCOL_MESSENGER_NOTE_SEND_REQ : GameClientPacket
    {
        private int NameLength, TextLength;
        private string Name, Text;
        private uint erro;
        public PROTOCOL_MESSENGER_NOTE_SEND_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            NameLength = ReadC();
            TextLength = ReadC();
            Name = ReadU(NameLength * 2);
            Text = ReadU(TextLength * 2);
        }
        public override void Run()
        {
            try
            {
                //0x80001080 STR_TBL_NETWORK_DONT_SEND_MYSELF_MESSAGE
                //0x80001081 STR_TBL_NETWORK_FULL_SEND_MESSAGE_PER_DAY
                Account Player = Client.Player;
                if (Player == null || Player.Nickname.Length == 0 || Player.Nickname == Name)
                {
                    return;
                }
                Account Receiver = AccountManager.GetAccount(Name, 1, 0);
                if (Receiver != null)
                {
                    if (DaoManagerSQL.GetMessagesCount(Receiver.PlayerId) >= 100)
                    {
                        erro = 0x8000107F;
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
                    erro = 0x8000107E;
                }
                Client.SendPacket(new PROTOCOL_MESSENGER_NOTE_SEND_ACK(erro));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_MESSENGER_NOTE_SEND_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
        private MessageModel CreateMessage(string SenderName, long OwnerId, long SenderId)
        {
            MessageModel Message = new MessageModel(15)
            {
                SenderName = SenderName,
                SenderId = SenderId,
                Text = Text,
                State = NoteMessageState.Unreaded
            };
            if (!DaoManagerSQL.CreateMessage(OwnerId, Message))
            {
                erro = 0x80000000;
                return null;
            }
            return Message;
        }
    }
}