using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Managers.Events;
using Plugin.Core.Network;

namespace Server.Game.Data.Sync.Client
{
    public class EventInfo
    {
        public static void LoadEventInfo(SyncClientPacket C)
        {
            int Type = C.ReadC();
            if (ReloadEvent(Type))
            {
                CLogger.Print($"Refresh event; Type: {Type};", LoggerType.Command);
            }
        }
        private static bool ReloadEvent(int SlotId)
        {
            if (SlotId == 0)
            {
                EventVisitSync.Reload();
                return true;
            }
            else if (SlotId == 1)
            {
                EventLoginSync.Reload();
                return true;
            }
            else if (SlotId == 2)
            {
                EventMapSync.Reload();
                return true;
            }
            else if (SlotId == 3)
            {
                EventPlaytimeSync.Reload();
                return true;
            }
            else if (SlotId == 4)
            {
                EventQuestSync.Reload();
                return true;
            }
            else if (SlotId == 5)
            {
                EventRankUpSync.Reload();
                return true;
            }
            else if (SlotId == 6)
            {
                EventXmasSync.Reload();
                return true;
            }
            return false;
        }
    }
}
