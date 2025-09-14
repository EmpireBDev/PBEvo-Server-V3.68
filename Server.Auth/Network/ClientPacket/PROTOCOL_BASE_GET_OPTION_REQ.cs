using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Plugin.Core.SQL;
using Server.Auth.Data.Models;
using Server.Auth.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Auth.Network.ClientPacket
{
    public class PROTOCOL_BASE_GET_OPTION_REQ : AuthClientPacket
    {
        public PROTOCOL_BASE_GET_OPTION_REQ(AuthClient client, byte[] data)
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
                if (!Player.MyConfigsLoaded)
                {
                    //Client.SendPacket(new PROTOCOL_BASE_GET_OPTION_ACK(0, Player.Config));
                    if (Player.Friend.Friends.Count > 0)
                    {
                        Client.SendPacket(new PROTOCOL_AUTH_FRIEND_INFO_ACK(Player.Friend.Friends));
                    }
                }
                List<MessageModel> Messages = DaoManagerSQL.GetMessages(Player.PlayerId);
                if (Messages.Count > 0)
                {
                    DaoManagerSQL.RecycleMessages(Player.PlayerId, Messages);
                    if (Messages.Count > 0)
                    {
                        int Page = (int)Math.Ceiling(Messages.Count / 25d), PageIndex = 0, LastPageIndex = 0;
                        if (LastPageIndex >= Page)
                        {
                            LastPageIndex = 0;
                        }
                        byte[] HeaderArray = GetMessageHeaderData(LastPageIndex, ref PageIndex, Messages);
                        byte[] BodyArray = GetMessageBodyData(LastPageIndex, ref PageIndex, Messages);
                        Client.SendPacket(new PROTOCOL_MESSENGER_NOTE_LIST_ACK(Messages.Count, LastPageIndex++, HeaderArray, BodyArray));
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BASE_GET_OPTION_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
        private byte[] GetMessageHeaderData(int page, ref int count, List<MessageModel> List)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                for (int i = (page * 25); i < List.Count; i++)
                {
                    WriteMessageHeaderData(List[i], S);
                    if (++count == 25)
                    {
                        break;
                    }
                }
                return S.ToArray();
            }
        }
        private void WriteMessageHeaderData(MessageModel Message, SyncServerPacket S)
        {
            S.WriteD((uint)Message.ObjectId);
            S.WriteQ(Message.SenderId);
            S.WriteC((byte)Message.Type);
            S.WriteC((byte)Message.State);
            S.WriteC((byte)Message.DaysRemaining);
            S.WriteD(Message.ClanId);
        }
        private byte[] GetMessageBodyData(int page, ref int count, List<MessageModel> List)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                for (int i = (page * 25); i < List.Count; i++)
                {
                    WriteMessageBodyData(List[i], S);
                    if (++count == 25)
                    {
                        break;
                    }
                }
                return S.ToArray();
            }
        }
        private void WriteMessageBodyData(MessageModel Message, SyncServerPacket S)
        {
            S.WriteC((byte)(Message.SenderName.Length + 1));
            S.WriteC((byte)(Message.Type == NoteMessageType.Insert || Message.Type == NoteMessageType.ClanAsk || Message.Type == NoteMessageType.Clan && Message.ClanNote != 0 ? 0 : (Message.Text.Length + 1)));
            S.WriteN(Message.SenderName, Message.SenderName.Length + 2, "UTF-16LE");
            if (Message.Type == NoteMessageType.ClanAsk || Message.Type == NoteMessageType.Clan)
            {
                if (Message.ClanNote >= NoteMessageClan.JoinAccept && Message.ClanNote <= NoteMessageClan.Secession)
                {
                    S.WriteH((short)(Message.Text.Length + 1));
                    S.WriteH((short)Message.ClanNote);
                    S.WriteN(Message.Text, Message.Text.Length + 1, "UTF-16LE");
                }
                else if (Message.ClanNote == NoteMessageClan.None)
                {
                    S.WriteN(Message.Text, Message.Text.Length + 2, "UTF-16LE");
                }
                else
                {
                    S.WriteH(3);
                    S.WriteD((int)Message.ClanNote);
                    if (Message.ClanNote != NoteMessageClan.Master || Message.ClanNote != NoteMessageClan.Staff || Message.ClanNote != NoteMessageClan.Regular)
                    {
                        S.WriteH(0);
                    }
                }
            }
            else
            {
                S.WriteN(Message.Text, Message.Text.Length + 2, "UTF-16LE");
            }
        }
    }
}
