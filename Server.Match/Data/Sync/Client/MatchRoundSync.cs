using Plugin.Core.Network;
using Server.Match.Data.Managers;
using Server.Match.Data.Models;

namespace Server.Match.Data.Sync.Client
{
    public class MatchRoundSync
    {
        public static void Load(SyncClientPacket C)
        {
            uint UniqueRoomId = C.ReadUD();
            uint RoomSeed = C.ReadUD();
            int Round = C.ReadC();
            RoomModel Room = RoomsManager.GetRoom(UniqueRoomId, RoomSeed);
            if (Room != null)
            {
                Room.ServerRound = Round;
            }
        }
    }
}
