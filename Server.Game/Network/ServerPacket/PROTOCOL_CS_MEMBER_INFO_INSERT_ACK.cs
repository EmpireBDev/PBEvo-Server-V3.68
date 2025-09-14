using Plugin.Core.Network;
using Plugin.Core.Utility;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_MEMBER_INFO_INSERT_ACK : GameServerPacket
    {
        private readonly Account player;
        private readonly ulong status;
        public PROTOCOL_CS_MEMBER_INFO_INSERT_ACK(Account player)
        {
            this.player = player;
            status = ComDiv.GetClanStatus(player.Status, player.IsOnline);
        }
        public override void Write()
        {
            WriteH(1871);
            WriteC((byte)(player.Nickname.Length + 1));
            WriteN(player.Nickname, player.Nickname.Length + 2, "UTF-16LE");
            WriteQ(player.PlayerId);
            WriteQ(status);
            WriteC((byte)player.Rank);
            WriteC((byte)player.NickColor);
        }
    }
}
