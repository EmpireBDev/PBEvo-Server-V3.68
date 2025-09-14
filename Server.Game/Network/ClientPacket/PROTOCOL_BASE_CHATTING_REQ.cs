using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Server.Game.Data.Utils;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_CHATTING_REQ : GameClientPacket
    {
        private string Text;
        private ChattingType Type;
        public PROTOCOL_BASE_CHATTING_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            Type = (ChattingType)ReadH();
            Text = ReadU(ReadH() * 2);
        }
        public override void Run()
        {
            try
            {
                Account Player = Client.Player;
                if (Player == null || string.IsNullOrEmpty(Text) || Text.Length > 60 || Player.Nickname.Length == 0)
                {
                    return;
                }
                RoomModel Room = Player.Room;
                SlotModel Sender;
                switch (Type)
                {
                    case ChattingType.Team:
                    {
                        if (Room == null)
                        {
                            return;
                        }
                        Sender = Room.Slots[Player.SlotId];
                        int[] Array = Room.GetTeamArray(Sender.Team);
                        using (PROTOCOL_ROOM_CHATTING_ACK Packet = new PROTOCOL_ROOM_CHATTING_ACK((int)Type, Sender.Id, Player.UseChatGM(), Text))
                        {
                            byte[] Data = Packet.GetCompleteBytes("PROTOCOL_BASE_CHATTING_REQ-1");
                            lock (Room.Slots)
                            {
                                foreach (int SlotIdx in Array)
                                {
                                    SlotModel Receiver = Room.Slots[SlotIdx];
                                    if (Receiver != null)
                                    {
                                        Account PlayerRecv = Room.GetPlayerBySlot(Receiver);
                                        if (PlayerRecv != null && AllUtils.SlotValidMessage(Sender, Receiver))
                                        {
                                            PlayerRecv.SendCompletePacket(Data, Packet.GetType().Name);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    }
                    case ChattingType.All:
                    case ChattingType.Lobby:
                    {
                        if (Room != null)
                        {
                            if (!AllUtils.ServerCommands(Player, Text))
                            {
                                Sender = Room.Slots[Player.SlotId];
                                using (PROTOCOL_ROOM_CHATTING_ACK Packet = new PROTOCOL_ROOM_CHATTING_ACK((int)Type, Sender.Id, Player.UseChatGM(), Text))
                                {
                                    byte[] Data = Packet.GetCompleteBytes("PROTOCOL_BASE_CHATTING_REQ-2");
                                    lock (Room.Slots)
                                    {
                                        foreach (SlotModel Receiver in Room.Slots)
                                        {
                                            Account PlayerRecv = Room.GetPlayerBySlot(Receiver);
                                            if (PlayerRecv != null && AllUtils.SlotValidMessage(Sender, Receiver))
                                            {
                                                PlayerRecv.SendCompletePacket(Data, Packet.GetType().Name);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            ChannelModel Channel = Player.GetChannel();
                            if (Channel == null)
                            {
                                return;
                            }
                            if (!AllUtils.ServerCommands(Player, Text))
                            {
                                using (PROTOCOL_LOBBY_CHATTING_ACK packet = new PROTOCOL_LOBBY_CHATTING_ACK(Player, Text))
                                {
                                    Channel.SendPacketToWaitPlayers(packet);
                                }
                            }
                        }
                        break;
                    }
                    case ChattingType.None:
                    case ChattingType.Whisper:
                    case ChattingType.Reply:
                    case ChattingType.Clan:
                    case ChattingType.Match:
                    case ChattingType.ClanMemberPage:
                    {
                        break;
                    }
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}