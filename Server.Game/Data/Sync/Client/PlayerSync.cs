using Plugin.Core.Network;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;

namespace Server.Game.Data.Sync.Client
{
    public static class PlayerSync
    {
        public static void Load(SyncClientPacket C)
        {
            long PlayerId = C.ReadQ();
            int Type = C.ReadC();
            int Rank = C.ReadC();
            int Gold = C.ReadD();
            int Cash = C.ReadD();
            int Tags = C.ReadD();
            Account Player = AccountManager.GetAccount(PlayerId, true);
            if (Player == null)
            {
                return;
            }
            if (Type == 0)
            {
                Player.Rank = Rank;
                Player.Gold = Gold;
                Player.Cash = Cash;
                Player.Tags = Tags;
            }
        }
    }
}