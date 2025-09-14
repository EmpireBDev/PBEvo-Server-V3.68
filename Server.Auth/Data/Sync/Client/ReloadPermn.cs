using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Managers.Events;
using Plugin.Core.Network;
using Plugin.Core.XML;

namespace Server.Auth.Data.Sync.Client
{
    public class ReloadPermn
    {
        public static void Load(SyncClientPacket C)
        {
            int Index = C.ReadC();
            if (Index == 1)
            {
                EventVisitSync.Reload();
                EventLoginSync.Reload();
                EventMapSync.Reload();
                EventPlaytimeSync.Reload();
                EventQuestSync.Reload();
                EventRankUpSync.Reload();
                EventXmasSync.Reload();
                CLogger.Print("All Events Successfully Reloaded!", LoggerType.Command);
            }
            else if (Index == 2)
            {
                PermissionXML.Load();
                CLogger.Print("Permission Successfully Reloaded!", LoggerType.Command);
            }
            CLogger.Print($"Updating null part: {Index}", LoggerType.Command);
        }
    }
}
