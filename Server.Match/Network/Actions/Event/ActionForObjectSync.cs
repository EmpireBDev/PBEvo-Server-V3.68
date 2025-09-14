using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Match.Data.Models;
using Server.Match.Data.Models.Event.Event;

namespace Server.Match.Network.Actions.Event
{
    public class ActionForObjectSync
    {
        public static ActionObjectInfo ReadInfo(ActionModel Action, SyncClientPacket C, bool GenLog)
        {
            ActionObjectInfo Info = new ActionObjectInfo()
            {
                Unk1 = C.ReadC(),
                Unk2 = C.ReadC()
            };
            if (GenLog)
            {
                CLogger.Print($"Slot: {Action.Slot} ActionForObjectSync: Unk ({Info.Unk1};{Info.Unk2})", LoggerType.Warning);
            }
            return Info;
        }
        public static void WriteInfo(SyncServerPacket S, ActionObjectInfo Info)
        {
            S.WriteC(Info.Unk1);
            S.WriteC(Info.Unk2);
        }
    }
}
