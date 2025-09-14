using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_MESSENGER_NOTE_RECEIVE_ACK : GameServerPacket
    {
        private readonly MessageModel Message;
        public PROTOCOL_MESSENGER_NOTE_RECEIVE_ACK(MessageModel Message)
        {
            this.Message = Message;
        }
        public override void Write()
        {
            WriteH(907);
            WriteD((uint)Message.ObjectId);
            WriteQ(Message.SenderId);
            if (Message.Type == NoteMessageType.ClanAsk)
            {
                WriteC(6);
            }
            else
            {
                WriteC((byte)Message.Type);
            }
            WriteC((byte)Message.State);
            WriteC((byte)Message.DaysRemaining);
            WriteD(Message.ClanId);
            WriteC((byte)(Message.SenderName.Length + 1));
            WriteC((byte)(Message.Type == NoteMessageType.Insert || Message.Type == NoteMessageType.ClanAsk || Message.Type == NoteMessageType.Clan && Message.ClanNote != NoteMessageClan.None ? 0 : (Message.Text.Length + 1)));
            WriteN(Message.SenderName, Message.SenderName.Length + 2, "UTF-16LE");
            WriteB(NoteClanData(Message));
        }
        public byte[] NoteClanData(MessageModel Message)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
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
                return S.ToArray();
            }
        }
    }
}
