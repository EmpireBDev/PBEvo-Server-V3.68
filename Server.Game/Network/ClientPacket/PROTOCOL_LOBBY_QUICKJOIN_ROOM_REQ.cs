using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_LOBBY_QUICKJOIN_ROOM_REQ : GameClientPacket
    {
        private int Select;
        private List<RoomModel> Rooms = new List<RoomModel>();
        private List<QuickstartModel> Quicks = new List<QuickstartModel>();
        private QuickstartModel QuickSelect;
        public PROTOCOL_LOBBY_QUICKJOIN_ROOM_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            Select = ReadC();
            for (int i = 0; i < 3; i++)
            {
                QuickstartModel Quick = new QuickstartModel()
                {
                    MapId = ReadC(),
                    Rule = ReadC(),
                    StageOptions = ReadC(),
                    Type = ReadC()
                };
                Quicks.Add(Quick);
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
                Player.Quickstart.Quickjoins[Select] = Quicks[Select];
                ComDiv.UpdateDB("player_quickstarts", "owner_id", Player.PlayerId, new string[] { $"list{Select}_map_id", $"list{Select}_map_rule", $"list{Select}_map_stage", $"list{Select}_map_type" }, Quicks[Select].MapId, Quicks[Select].Rule, Quicks[Select].StageOptions, Quicks[Select].Type);
                if (Player.Nickname.Length > 0 && Player.Room == null && Player.Match == null && Player.GetChannel(out ChannelModel Channel))
                {
                    lock (Channel.Rooms)
                    {
                        foreach (RoomModel Room in Channel.Rooms)
                        {
                            if (Room.RoomType == RoomCondition.Tutorial)
                            {
                                continue;
                            }
                            QuickSelect = Quicks[Select];
                            if (QuickSelect.MapId == (int)Room.MapId && QuickSelect.Rule == (int)Room.Rule && QuickSelect.StageOptions == (int)Room.Stage && QuickSelect.Type == (int)Room.RoomType && Room.Password.Length == 0 && Room.Limit == 0 && (!Room.KickedPlayers.Contains(Player.PlayerId) || Player.HaveGMLevel()))
                            {
                                foreach (SlotModel Slot in Room.Slots)
                                {
                                    if (Slot.PlayerId == 0 && Slot.State == SlotState.EMPTY)
                                    {
                                        Rooms.Add(Room);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (Rooms.Count == 0)
                {
                    Client.SendPacket(new PROTOCOL_LOBBY_QUICKJOIN_ROOM_ACK(0x80000000, Quicks, null, null));
                }
                else
                {
                    RoomModel Room = Rooms[new Random().Next(Rooms.Count)];
                    if (Room != null && Room.GetLeader() != null && Room.AddPlayer(Player) >= 0)
                    {
                        Player.ResetPages();
                        using (PROTOCOL_ROOM_GET_SLOTONEINFO_ACK packet = new PROTOCOL_ROOM_GET_SLOTONEINFO_ACK(Player))
                        {
                            Room.SendPacketToPlayers(packet, Player.PlayerId);
                        }
                        Room.UpdateSlotsInfo();
                        Client.SendPacket(new PROTOCOL_ROOM_JOIN_ACK(0, Player));
                        Client.SendPacket(new PROTOCOL_LOBBY_QUICKJOIN_ROOM_ACK(0, Quicks, Room, QuickSelect));
                    }
                    else
                    {
                        Client.SendPacket(new PROTOCOL_LOBBY_QUICKJOIN_ROOM_ACK(0x80000000, null, null, null));
                    }
                }
                Rooms = null;
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_LOBBY_QUICKJOIN_ROOM_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
