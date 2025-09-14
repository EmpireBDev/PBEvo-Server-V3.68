using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Match.Data.Models;
using Server.Match.Data.Models.Event;

namespace Server.Match.Network.Actions.Event
{
    public class Animation
    {
        public static AnimationInfo ReadInfo(ActionModel Action, SyncClientPacket C, bool GenLog)
        {
            AnimationInfo Info = new AnimationInfo()
            {
                Animation = C.ReadUH()
            };
            if (GenLog)
            {
                CLogger.Print($"Slot: {Action.Slot}; POV: {Info.Animation}", LoggerType.Warning);
            }
            return Info;
        }
        public static void WriteInfo(SyncServerPacket S, AnimationInfo Info)
        {
            S.WriteH(Info.Animation);
        }
    }
}