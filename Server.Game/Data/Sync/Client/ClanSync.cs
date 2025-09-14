using Plugin.Core.Network;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;

namespace Server.Game.Data.Sync.Client
{
    public static class ClanSync
    {
        public static void Load(SyncClientPacket C)
        {
            long PlayerId = C.ReadQ();
            int Type = C.ReadC();
            Account Player = AccountManager.GetAccount(PlayerId, true);
            if (Player == null)
            {
                return;
            }
            if (Type == 3)
            {
                int ClanId = C.ReadD();
                int ClanAccess = C.ReadC();
                Player.ClanId = ClanId;
                Player.ClanAccess = ClanAccess;
            }
        }
    }
}