using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.XML;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_ROOM_CHANGE_SLOT_REQ : GameClientPacket
    {
        private int SlotInfo;
        private uint Error;
        public PROTOCOL_ROOM_CHANGE_SLOT_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            SlotInfo = ReadD();
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
                RoomModel Room = Player.Room;
                if (Room != null && Room.Leader == Player.SlotId && Room.GetSlot((SlotInfo & 0xFFFFFFF), out SlotModel Slot))
                {
                    switch (Slot.State)
                    {
                        case SlotState.EMPTY:
                        {
                            Room.ChangeSlotState(Slot, SlotState.CLOSE, true);
                            break;
                        }
                        case SlotState.CLOSE:
                        {
                            MapMatch Match = SystemMapXML.GetMapLimit((int)Room.MapId, (int)Room.Rule);
                            if (Match != null && (SlotInfo & 0x10000000) == 0x10000000 && Slot.Id < Match.Limit)
                            {
                                Room.ChangeSlotState(Slot, SlotState.EMPTY, true);
                            }
                            break;
                        }
                        case SlotState.UNKNOWN:
                        case SlotState.SHOP:
                        case SlotState.INFO:
                        case SlotState.CLAN:
                        case SlotState.INVENTORY:
                        case SlotState.GACHA:
                        case SlotState.GIFTSHOP:
                        case SlotState.NORMAL:
                        case SlotState.READY:
                        {
                            Account Member = Room.GetPlayerBySlot(Slot);
                            if (Member != null && !Member.AntiKickGM)
                            {
                                if ((Slot.State != SlotState.READY && (Room.ChannelType == ChannelType.Clan && Room.State != RoomState.CountDown || Room.ChannelType != ChannelType.Clan) || Slot.State == SlotState.READY && (Room.ChannelType == ChannelType.Clan && Room.State == RoomState.Ready || Room.ChannelType != ChannelType.Clan)))
                                {
                                    Member.SendPacket(new PROTOCOL_SERVER_MESSAGE_KICK_PLAYER_ACK());
                                    Room.RemovePlayer(Member, Slot, false);
                                }
                            }
                            break;
                        }
                        case SlotState.LOAD:
                        case SlotState.RENDEZVOUS:
                        case SlotState.PRESTART:
                        case SlotState.BATTLE_LOAD:
                        case SlotState.BATTLE_READY:
                        case SlotState.BATTLE:
                        {
                            Error = 0x80000401;
                            break;
                        }
                    }
                }
                else
                {
                    Error = 0x80000401;
                }
                Client.SendPacket(new PROTOCOL_ROOM_CHANGE_SLOT_ACK(Error));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_ROOM_CHANGE_SLOT_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}