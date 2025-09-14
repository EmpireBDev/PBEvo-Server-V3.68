using Plugin.Core.Enums;
using Plugin.Core.Utility;
using Server.Auth.Data.Models;

namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_CS_MEMBER_INFO_CHANGE_ACK : AuthServerPacket
    {
        private readonly ulong Status;
        private readonly Account Player;
        public PROTOCOL_CS_MEMBER_INFO_CHANGE_ACK(Account Player)
        {
            this.Player = Player;
            Status = ComDiv.GetClanStatus(Player.Status, Player.IsOnline);
        }
        public PROTOCOL_CS_MEMBER_INFO_CHANGE_ACK(Account Player, FriendState State)
        {
            this.Player = Player;
            Status = State == 0 ? ComDiv.GetClanStatus(Player.Status, Player.IsOnline) : ComDiv.GetClanStatus(State);
        }
        public override void Write()
        {
            WriteH(1875);
            WriteQ(Player.PlayerId);
            WriteQ(Status);
        }
    }
}
