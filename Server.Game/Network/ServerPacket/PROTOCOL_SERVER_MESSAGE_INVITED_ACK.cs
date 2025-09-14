using Plugin.Core.Network;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_SERVER_MESSAGE_INVITED_ACK : GameServerPacket
    {
        private readonly Account sender;
        private readonly RoomModel room;
        public PROTOCOL_SERVER_MESSAGE_INVITED_ACK(Account sender, RoomModel room)
        {
            this.sender = sender;
            this.room = room;
        }
        public override void Write()
        {
            WriteH(2565);
            WriteU(sender.Nickname, 66);
            WriteD(room.RoomId);
            WriteQ(sender.PlayerId);
            WriteS(room.Password, 4);
        }
    }
}