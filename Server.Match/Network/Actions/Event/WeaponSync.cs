using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Plugin.Core.Utility;
using Server.Match.Data.Models;
using Server.Match.Data.Models.Event;

namespace Server.Match.Network.Actions.Event
{
    public class WeaponSync
    {
        public static WeaponSyncInfo ReadInfo(ActionModel Action, SyncClientPacket C, bool GenLog, bool OnlyBytes = false)
        {
            WeaponSyncInfo Info = new WeaponSyncInfo()
            {
                Extensions = C.ReadC(),
                WeaponId = C.ReadD()
            };
            if (!OnlyBytes)
            {
                Info.WeaponClass = (ClassType)ComDiv.GetIdStatics(Info.WeaponId, 2);
            }
            if (GenLog)
            {
                CLogger.Print($"Slot {Action.Slot}; Weapon Sync (ID, EXT): ({Info.WeaponId};{Info.Extensions})", LoggerType.Warning);
            }
            return Info;
        }
        public static void WriteInfo(SyncServerPacket S, WeaponSyncInfo Info)
        {
            S.WriteC(Info.Extensions);
            S.WriteD(Info.WeaponId);
        }
    }
}