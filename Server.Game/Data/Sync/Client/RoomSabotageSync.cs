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
    public class RoomSabotageSync
    {
        public static void Load(SyncClientPacket C)
        {
            int RoomId = C.ReadH();
            int ChannelId = C.ReadH();
            int ServerId = C.ReadH();
            byte KillerIdx = C.ReadC();
            ushort RedObjective = C.ReadUH();
            ushort BlueObjective = C.ReadUH();
            int BarNumber = C.ReadC();
            ushort Damage = C.ReadUH();
            if (C.ToArray().Length > 16)
            {
                CLogger.Print($"Invalid Sabotage (Length > 16): {C.ToArray().Length}", LoggerType.Warning);
            }
            ChannelModel Channel = ChannelsXML.GetChannel(ServerId, ChannelId);
            if (Channel == null)
            {
                return;
            }
            RoomModel Room = Channel.GetRoom(RoomId);
            if (Room != null && Room.RoundTime.Timer == null && Room.State == RoomState.Battle && !Room.SwapRound && Room.GetSlot(KillerIdx, out SlotModel killer))
            {
                Room.Bar1 = RedObjective;
                Room.Bar2 = BlueObjective;
                RoomCondition type = Room.RoomType;
                int times = 0;
                if (BarNumber == 1)
                {
                    killer.DamageBar1 += Damage;
                    times += killer.DamageBar1 / 600;
                }
                else if (BarNumber == 2)
                {
                    killer.DamageBar2 += Damage;
                    times += killer.DamageBar2 / 600;
                }
                killer.EarnedEXP = times;
                if (type == RoomCondition.Destroy)
                {
                    using (PROTOCOL_BATTLE_MISSION_GENERATOR_INFO_ACK packet = new PROTOCOL_BATTLE_MISSION_GENERATOR_INFO_ACK(Room))
                    {
                        Room.SendPacketToPlayers(packet, SlotState.BATTLE, 0);
                    }
                    if (Room.Bar1 == 0)
                    {
                        EndRound(Room, TeamEnum.CT_TEAM);
                    }
                    else if (Room.Bar2 == 0)
                    {
                        EndRound(Room, TeamEnum.FR_TEAM);
                    }
                }
                else if (type == RoomCondition.Defense)
                {
                    using (PROTOCOL_BATTLE_MISSION_DEFENCE_INFO_ACK packet = new PROTOCOL_BATTLE_MISSION_DEFENCE_INFO_ACK(Room))
                    {
                        Room.SendPacketToPlayers(packet, SlotState.BATTLE, 0);
                    }
                    if (Room.Bar1 == 0 && Room.Bar2 == 0)
                    {
                        EndRound(Room, TeamEnum.FR_TEAM);
                    }
                }
            }
        }
        public static void EndRound(RoomModel room, TeamEnum winner)
        {
            room.SwapRound = true;
            if (winner == TeamEnum.FR_TEAM)
            {
                room.FRRounds++;
            }
            else if (winner == TeamEnum.CT_TEAM)
            {
                room.CTRounds++;
            }
            AllUtils.BattleEndRound(room, winner, RoundEndType.Normal);
        }
    }
}
