using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_LOBBY_GET_ROOMLIST_ACK : GameServerPacket
    {
        private readonly int RoomPage;
        private readonly int PlayerPage;
        private readonly int AllPlayers;
        private readonly int AllRooms;
        private readonly int CountRoom;
        private readonly int CountPlayer;
        private readonly byte[] Rooms;
        private readonly byte[] Players;
        public PROTOCOL_LOBBY_GET_ROOMLIST_ACK(int AllRooms, int AllPlayers, int RoomPage, int PlayerPage, int CountRoom, int CountPlayer, byte[] Rooms, byte[] Players)
        {
            this.AllRooms = AllRooms;
            this.AllPlayers = AllPlayers;
            this.RoomPage = RoomPage;
            this.PlayerPage = PlayerPage;
            this.Rooms = Rooms;
            this.Players = Players;
            this.CountRoom = CountRoom;
            this.CountPlayer = CountPlayer;
        }
        public override void Write()
        {
            WriteH(3078);
            WriteD(AllRooms);
            WriteD(RoomPage);
            WriteD(CountRoom);
            WriteB(Rooms);
            WriteD(AllPlayers);
            WriteD(PlayerPage);
            WriteD(CountPlayer);
            WriteB(Players);
        }
    }
}