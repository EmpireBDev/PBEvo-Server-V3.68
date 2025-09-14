using Plugin.Core.Network;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;

namespace Server.Game.Data.Sync.Client
{
    public class AuthLogin
    {
        public static void Load(SyncClientPacket C)
        {
            long PlayerId = C.ReadQ();
            Account Player = AccountManager.GetAccount(PlayerId, true);
            if (Player != null)
            {
                Player.SendPacket(new PROTOCOL_AUTH_ACCOUNT_KICK_ACK(1));
                Player.SendPacket(new PROTOCOL_SERVER_MESSAGE_ERROR_ACK(0x80001000));
                Player.Close(1000);
            }
        }
    }
}
