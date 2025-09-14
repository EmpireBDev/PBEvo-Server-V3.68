using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Match.Data.Models;
using Server.Match.Data.Models.Event;

namespace Server.Match.Network.Actions.Event
{
    public class FireDataOnObject
    {
        public static FireDataObjectInfo ReadInfo(ActionModel Action, SyncClientPacket C, bool GenLog)
        {
            FireDataObjectInfo Info = new FireDataObjectInfo()
            {
                ShotId = C.ReadUH()
            };
            if (GenLog)
            {
                CLogger.Print($"Slot: {Action.Slot} Fire Data Index: [{Info.ShotId}]", LoggerType.Warning);
            }
            return Info;
        }
        public static void WriteInfo(SyncServerPacket S, FireDataObjectInfo Info)
        {
            S.WriteH(Info.ShotId);
        }
    }
}