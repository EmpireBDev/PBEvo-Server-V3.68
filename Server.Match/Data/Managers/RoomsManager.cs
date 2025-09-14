using Plugin.Core.Enums;
using Server.Match.Data.Models;
using Server.Match.Data.Utils;
using System.Collections.Generic;

namespace Server.Match.Data.Managers
{
    public class RoomsManager
    {
        public static List<AssistModel> Assists = new List<AssistModel>();
        private readonly static List<RoomModel> List = new List<RoomModel>();
        public static RoomModel CreateOrGetRoom(uint UniqueRoomId, uint Seed)
        {
            lock (List)
            {
                foreach (RoomModel Room in List)
                {
                    if (Room.UniqueRoomId == UniqueRoomId && Room.RoomSeed == Seed)
                    {
                        return Room;
                    }
                }
                int serverId = AllUtils.GetRoomInfo(UniqueRoomId, 2), channelId = AllUtils.GetRoomInfo(UniqueRoomId, 1), roomId = AllUtils.GetRoomInfo(UniqueRoomId, 0);
                RoomModel roomNew = new RoomModel(serverId)
                {
                    UniqueRoomId = UniqueRoomId,
                    RoomSeed = Seed,
                    RoomId = roomId,
                    ChannelId = channelId,
                    MapId = (MapIdEnum)AllUtils.GetSeedInfo(Seed, 2),
                    RoomType = (RoomCondition)AllUtils.GetSeedInfo(Seed, 0),
                    Rule = (MapRules)AllUtils.GetSeedInfo(Seed, 1)
                };
                List.Add(roomNew);
                return roomNew;
            }
        }
        public static RoomModel GetRoom(uint UniqueRoomId, uint Seed)
        {
            lock (List)
            {
                foreach (RoomModel Room in List)
                {
                    if (Room != null && Room.UniqueRoomId == UniqueRoomId && Room.RoomSeed == Seed)
                    {
                        return Room;
                    }
                }
                return null;
            }
        }
        public static void RemoveRoom(uint UniqueRoomId, uint Seed)
        {
            lock (List)
            {
                for (int i = 0; i < List.Count; ++i)
                {
                    RoomModel Room = List[i];
                    if (Room.UniqueRoomId == UniqueRoomId && Room.RoomSeed == Seed)
                    {
                        List.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        public static void RemoveAssists(uint UniqueRoomId, uint Seed)
        {
            lock (Assists)
            {
                int RoomId = GetRoom(UniqueRoomId, Seed).RoomId;
                AssistModel Assist = Assists.Find(x => x.RoomId == RoomId);
                if (Assist != null && Assists.Remove(Assist))
                {
                    foreach (AssistModel AssistFix in Assists.FindAll(x => x.RoomId == RoomId))
                    {
                        Assists.Remove(AssistFix);
                    }
                }
            }
        }
    }
}