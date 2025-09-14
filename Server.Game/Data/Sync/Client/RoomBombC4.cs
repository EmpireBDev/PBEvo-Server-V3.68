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
    public static class RoomBombC4
    {
        public static void Load(SyncClientPacket C)
        {
            int RoomId = C.ReadH();
            int ChannelId = C.ReadH();
            int ServerId = C.ReadH();
            int Type = C.ReadC();
            int SlotIdx = C.ReadC();
            byte Zone = 0;
            ushort Unk = 0;
            float X = 0, Y = 0, Z = 0;
            if (Type == 0)
            {
                Zone = C.ReadC();
                X = C.ReadT();
                Y = C.ReadT();
                Z = C.ReadT();
                Unk = C.ReadUH();
                if (C.ToArray().Length > 25)
                {
                    CLogger.Print($"Invalid Bomb (Length > 25): {C.ToArray().Length}", LoggerType.Warning);
                }
            }
            else if (Type == 1)
            {
                if (C.ToArray().Length > 10)
                {
                    CLogger.Print($"Invalid Bomb Type[1] (Length > 10): {C.ToArray().Length}", LoggerType.Warning);
                }
            }
            ChannelModel Channel = ChannelsXML.GetChannel(ServerId, ChannelId);
            if (Channel == null)
            {
                return;
            }
            RoomModel Room = Channel.GetRoom(RoomId);
            if (Room != null && Room.RoundTime.Timer == null && Room.State == RoomState.Battle)
            {
                SlotModel Slot = Room.GetSlot(SlotIdx);
                if (Slot == null || Slot.State != SlotState.BATTLE)
                {
                    return;
                }
                if (Type == 0)
                {
                    InstallBomb(Room, Slot, Zone, Unk, X, Y, Z);
                }
                else if (Type == 1)
                {
                    UninstallBomb(Room, Slot);
                }
            }
        }
        public static void InstallBomb(RoomModel Room, SlotModel Slot, byte Zone, ushort Unk, float X, float Y, float Z)
        {
            if (Room.ActiveC4)
            {
                return;
            }
            using (PROTOCOL_BATTLE_MISSION_BOMB_INSTALL_ACK packet = new PROTOCOL_BATTLE_MISSION_BOMB_INSTALL_ACK(Slot.Id, Zone, Unk, X, Y, Z))
            {
                Room.SendPacketToPlayers(packet, SlotState.BATTLE, 0);
            }
            if (Room.RoomType != RoomCondition.Tutorial)
            {
                Room.ActiveC4 = true;
                Slot.Objects++;
                AllUtils.CompleteMission(Room, Slot, MissionType.C4_PLANT, 0);
                Room.StartBomb();
            }
            else
            {
                Room.ActiveC4 = true;
            }
        }
        public static void UninstallBomb(RoomModel room, SlotModel slot)
        {
            if (!room.ActiveC4)
            {
                return;
            }
            using (PROTOCOL_BATTLE_MISSION_BOMB_UNINSTALL_ACK packet = new PROTOCOL_BATTLE_MISSION_BOMB_UNINSTALL_ACK(slot.Id))
            {
                room.SendPacketToPlayers(packet, SlotState.BATTLE, 0);
            }
            if (room.RoomType != RoomCondition.Tutorial)
            {
                slot.Objects++;
                room.CTRounds++;
                AllUtils.CompleteMission(room, slot, MissionType.C4_DEFUSE, 0);
                AllUtils.BattleEndRound(room, TeamEnum.CT_TEAM, RoundEndType.Uninstall);
            }
            else
            {
                room.ActiveC4 = false;
            }
        }
    }
}