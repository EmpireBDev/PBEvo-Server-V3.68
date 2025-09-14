using Plugin.Core.Enums;
using Plugin.Core.Network;
using Plugin.Core.Utility;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_MEMBER_INFO_CHANGE_ACK : GameServerPacket
    {
        private readonly Account Player;
        private readonly ulong Status;
        public PROTOCOL_CS_MEMBER_INFO_CHANGE_ACK(Account Player)
        {
            this.Player = Player;
            if (Player != null)
            {
                Status = ComDiv.GetClanStatus(Player.Status, Player.IsOnline);
            }
        }
        public PROTOCOL_CS_MEMBER_INFO_CHANGE_ACK(Account Player, FriendState State)
        {
            this.Player = Player;
            if (Player != null)
            {
                Status = State == 0 ? ComDiv.GetClanStatus(Player.Status, Player.IsOnline) : ComDiv.GetClanStatus(State);
            }
        }
        public override void Write()
        {
            WriteH(1875);
            WriteQ(Player.PlayerId);
            WriteQ(Status);
        }
    }
}