using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Match.Data.Enums;
using Server.Match.Data.Models;
using Server.Match.Data.Models.Event;

namespace Server.Match.Network.Actions.Event
{
    public class ActionState
    {
        public static ActionStateInfo ReadInfo(ActionModel Action, SyncClientPacket C, bool GenLog)
        {
            ActionStateInfo Info = new ActionStateInfo()
            {
                Action = (ActionFlag)C.ReadUH(),
                Value = C.ReadC(),
                Flag = (WeaponSyncType)C.ReadC()
            };
            if (GenLog)
            {
                CLogger.Print($"Slot: {Action.Slot}; Action {Info.Action}; Value: {Info.Value}; Flag: {Info.Flag}", LoggerType.Warning);
            }
            return Info;
        }
        public static void WriteInfo(SyncServerPacket S, ActionStateInfo Info)
        {
            S.WriteH((ushort)Info.Action);
            S.WriteC(Info.Value);
            S.WriteC((byte)Info.Flag);
        }
    }
}