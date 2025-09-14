using Plugin.Core.Network;
using Server.Match.Data.Managers;
using Server.Match.Data.Models;

namespace Server.Match.Data.Sync.Client
{
    public class RemovePlayerSync
    {
        public static void Load(SyncClientPacket C)
        {
            uint UniqueRoomId = C.ReadUD();
            uint RoomSeed = C.ReadUD();
            int SlotId = C.ReadC();
            int InBattleCount = C.ReadC();
            RoomModel Room = RoomsManager.GetRoom(UniqueRoomId, RoomSeed);
            if (Room == null)
            {
                return;
            }
            if (InBattleCount == 0)
            {
                RoomsManager.RemoveAssists(UniqueRoomId, RoomSeed);
                RoomsManager.RemoveRoom(UniqueRoomId, RoomSeed);
            }
            else
            {
                PlayerModel Player = Room.GetPlayer(SlotId, false);
                if (Player != null)
                {
                    Player.ResetAllInfos();
                }
            }
        }
    }
}