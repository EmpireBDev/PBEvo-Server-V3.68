using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BATTLE_SENDPING_REQ : GameClientPacket
    {
        private byte[] Slots;
        private int ReadyPlayers;
        public PROTOCOL_BATTLE_SENDPING_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            Slots = ReadB(16);
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
                if (Room != null && Room.GetSlot(Player.SlotId, out SlotModel Slot))
                {
                    if (Slot != null && Slot.State >= SlotState.BATTLE_READY)
                    {
                        if (Room.State == RoomState.Battle)
                        {
                            Room.Ping = Slots[Room.Leader];
                        }
                        using (PROTOCOL_BATTLE_SENDPING_ACK Packet = new PROTOCOL_BATTLE_SENDPING_ACK(Slots))
                        {
                            List<Account> Players = Room.GetAllPlayers(SlotState.READY, 1);
                            if (Players.Count == 0)
                            {
                                return;
                            }
                            byte[] Data = Packet.GetCompleteBytes("PROTOCOL_BATTLE_SENDPING_REQ");
                            foreach (Account Member in Players)
                            {
                                SlotModel SlotM = Room.GetSlot(Member.SlotId);
                                if (SlotM != null && SlotM.State >= SlotState.BATTLE_READY)
                                {
                                    Member.SendCompletePacket(Data, Packet.GetType().Name);
                                }
                                else
                                {
                                    ++ReadyPlayers;
                                }
                            }
                        }
                        if (ReadyPlayers == 0)
                        {
                            Room.SpawnReadyPlayers();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BATTLE_SENDPING_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}