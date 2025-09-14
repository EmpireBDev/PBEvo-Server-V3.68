using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Match.Data.Models;
using Server.Match.Data.Models.Event;

namespace Server.Match.Network.Actions.Event
{
    public class SoundPosRotation
    {
        public static SoundPosRotationInfo ReadInfo(ActionModel Action, SyncClientPacket C, bool GenLog)
        {
            SoundPosRotationInfo Info = new SoundPosRotationInfo()
            {
                Time = C.ReadT()
            };
            if (GenLog)
            {
                CLogger.Print($"Slot: {Action.Slot}; Time: {Info.Time}", LoggerType.Warning);
            }
            return Info;
        }
        public static SoundPosRotationInfo ReadInfo(ActionModel Action, SyncClientPacket C, float Time, bool GenLog)
        {
            SoundPosRotationInfo Info = new SoundPosRotationInfo()
            {
                Time = Time //C.ReadT() ??
            };
            if (GenLog)
            {
                CLogger.Print($"Slot: {Action.Slot}; Time: {Info.Time}", LoggerType.Warning);
            }
            return Info;
        }
        public static void WriteInfo(SyncServerPacket S, SoundPosRotationInfo Info)
        {
            S.WriteT(Info.Time);
        }
    }
}
