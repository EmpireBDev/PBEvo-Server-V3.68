using Plugin.Core.Network;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_GM_LOG_LOBBY_ACK : GameServerPacket
    {
        private readonly Account Player;
        private readonly uint Error;
        public PROTOCOL_GM_LOG_LOBBY_ACK(uint Error, long PlayerId)
        {
            this.Error = Error;
            Player = AccountManager.GetAccount(PlayerId, true);
        }
        public override void Write()
        {
            WriteH(2685);
            WriteD(Error);
            WriteQ(Player.PlayerId);
        }
    }
}