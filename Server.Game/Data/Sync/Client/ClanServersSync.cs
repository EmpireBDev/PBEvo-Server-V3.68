using Plugin.Core.Models;
using Plugin.Core.Network;
using Server.Game.Data.Managers;

namespace Server.Game.Data.Sync.Client
{
    public class ClanServersSync
    {
        public static void Load(SyncClientPacket C)
        {
            int type = C.ReadC();
            int clanId = C.ReadD();
            long ownerId;
            uint date;
            string name, info;
            ClanModel clanCache = ClanManager.GetClan(clanId);
            if (type == 0)
            {
                if (clanCache != null)
                {
                    return;
                }
                ownerId = C.ReadQ();
                date = C.ReadUD();
                name = C.ReadS(C.ReadC());
                info = C.ReadS(C.ReadC());
                ClanModel clan = new ClanModel { Id = clanId, Name = name, OwnerId = ownerId, Logo = 0, Info = info, CreationDate = date };
                ClanManager.AddClan(clan);
            }
            else
            {
                if (clanCache != null)
                {
                    ClanManager.RemoveClan(clanCache);
                }
            }
        }
    }
}