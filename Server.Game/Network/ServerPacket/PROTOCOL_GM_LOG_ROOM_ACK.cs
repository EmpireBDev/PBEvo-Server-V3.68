using Plugin.Core.Network;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_GM_LOG_ROOM_ACK : GameServerPacket
    {
        private readonly Account player;
        private readonly uint error;
        public PROTOCOL_GM_LOG_ROOM_ACK(uint error, Account player)
        {
            this.error = error;
            this.player = player;
        }
        public override void Write()
        {
            WriteH(2687);
            WriteD(error);
            WriteQ(player.PlayerId);
        }
    }
}