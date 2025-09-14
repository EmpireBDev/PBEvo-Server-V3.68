using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Game.Data.Utils;
using Server.Game.Network.ServerPacket;
using Server.Game.Data.XML;
using Server.Game.Data.Models;
using Plugin.Core.Models;

namespace Server.Game.Data.Sync.Client
{
    public static class RoomPassPortal
    {
        public static void Load(SyncClientPacket C)
        {
            int RoomId = C.ReadH();
            int ChannelId = C.ReadH();
            int ServerId = C.ReadH();
            int SlotId = C.ReadC(); //player
            int PortalId = C.ReadC(); //portal
            if (C.ToArray().Length > 10)
            {
                CLogger.Print($"Invalid Portal (Length > 10): {C.ToArray().Length}", LoggerType.Warning);
            }
            ChannelModel Channel = ChannelsXML.GetChannel(ServerId, ChannelId);
            if (Channel == null)
            {
                return;
            }
            RoomModel Room = Channel.GetRoom(RoomId);
            if (Room != null && Room.RoundTime.Timer == null && Room.State == RoomState.Battle && Room.IsDinoMode("DE"))
            {
                SlotModel Slot = Room.GetSlot(SlotId);
                if (Slot != null && Slot.State == SlotState.BATTLE)
                {
                    ++Slot.PassSequence;
                    if (Slot.Team == 0)
                    {
                        Room.FRDino += 5;
                    }
                    else
                    {
                        Room.CTDino += 5;
                    }
                    CompleteMission(Room, Slot);
                    using (PROTOCOL_BATTLE_MISSION_TOUCHDOWN_ACK packet = new PROTOCOL_BATTLE_MISSION_TOUCHDOWN_ACK(Room, Slot))
                    {
                        using (PROTOCOL_BATTLE_MISSION_TOUCHDOWN_COUNT_ACK packet2 = new PROTOCOL_BATTLE_MISSION_TOUCHDOWN_COUNT_ACK(Room))
                        {
                            Room.SendPacketToPlayers(packet, packet2, SlotState.BATTLE, 0);
                        }
                    }
                }
            }
        }
        private static void CompleteMission(RoomModel room, SlotModel slot)
        {
            MissionType mission = MissionType.NA;
            if (slot.PassSequence == 1)
            {
                mission = MissionType.TOUCHDOWN;
            }
            else if (slot.PassSequence == 2)
            {
                mission = MissionType.TOUCHDOWN_ACE_ATTACKER;
            }
            else if (slot.PassSequence == 3)
            {
                mission = MissionType.TOUCHDOWN_HATTRICK;
            }
            else if (slot.PassSequence >= 4)
            {
                mission = MissionType.TOUCHDOWN_GAME_MAKER;
            }
            if (mission != MissionType.NA)
            {
                AllUtils.CompleteMission(room, slot, mission, 0);
            }
        }
    }
}