using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Match.Data.Models;
using Server.Match.Data.Models.Event;

namespace Server.Match.Network.Actions.Event
{
    public class FireNHitDataOnObject
    {
        public static FireNHitDataObjectInfo ReadInfo(ActionModel Action, SyncClientPacket C, bool GenLog)
        {
            FireNHitDataObjectInfo Info = new FireNHitDataObjectInfo()
            {
                Portal = C.ReadUH()
            };
            if (GenLog)
            {
                CLogger.Print($"Slot: {Action.Slot} passed on the portal [{Info.Portal}]", LoggerType.Warning);
            }
            return Info;
        }
        public static void WriteInfo(SyncServerPacket S, FireNHitDataObjectInfo Info)
        {
            S.WriteH(Info.Portal);
        }
    }
}