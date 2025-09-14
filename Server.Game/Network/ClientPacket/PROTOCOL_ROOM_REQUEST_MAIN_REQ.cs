using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_ROOM_REQUEST_MAIN_REQ : GameClientPacket
    {
        public PROTOCOL_ROOM_REQUEST_MAIN_REQ(GameClient client, byte[] data)
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
                RoomModel r = Player.Room;
                if (r != null)
                {
                    if (r.State != RoomState.Ready || r.Leader == Player.SlotId)
                    {
                        return;
                    }
                    List<Account> Players = r.GetAllPlayers();
                    if (Players.Count == 0)
                    {
                        return;
                    }

                    if ((int)Player.Access >= 4)
                    {
                        ChangeLeader(r, Players, Player.SlotId);
                    }
                    else
                    {
                        if (!r.RequestHost.Contains(Player.PlayerId))
                        {
                            r.RequestHost.Add(Player.PlayerId);
                            if (r.RequestHost.Count() < (Players.Count / 2) + 1)
                            {
                                using (PROTOCOL_ROOM_REQUEST_MAIN_ACK packet = new PROTOCOL_ROOM_REQUEST_MAIN_ACK(Player.SlotId))
                                {
                                    SendPacketToRoom(packet, Players);
                                }
                            }
                        }
                        if (r.RequestHost.Count() >= (Players.Count / 2) + 1)
                        {
                            ChangeLeader(r, Players, Player.SlotId);
                        }
                    }
                }
                else
                {
                    Client.SendPacket(new PROTOCOL_ROOM_REQUEST_MAIN_ACK(0x80000000));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_ROOM_REQUEST_MAIN_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
        private void ChangeLeader(RoomModel r, List<Account> players, int slotId)
        {
            r.SetNewLeader(slotId, SlotState.EMPTY, -1, false);
            using (PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_WHO_ACK packet = new PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_WHO_ACK(slotId))
            {
                SendPacketToRoom(packet, players);
            }
            r.UpdateSlotsInfo();
            r.RequestHost.Clear();
        }
        private void SendPacketToRoom(GameServerPacket Packet, List<Account> Players)
        {
            byte[] Data = Packet.GetCompleteBytes("PROTOCOL_ROOM_REQUEST_MAIN_REQ");
            foreach (Account Member in Players)
            {
                Member.SendCompletePacket(Data, Packet.GetType().Name);
            }
        }
    }
}