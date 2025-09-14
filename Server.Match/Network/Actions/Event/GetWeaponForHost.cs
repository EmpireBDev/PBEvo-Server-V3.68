using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Match.Data.Models;
using Server.Match.Data.Models.Event;

namespace Server.Match.Network.Actions.Event
{
    public class GetWeaponForHost
    {
        public static WeaponHost ReadInfo(ActionModel Action, SyncClientPacket C, bool GenLog)
        {
            WeaponHost info = new WeaponHost()
            {
                DeathType = C.ReadC(),
                HitPart = C.ReadC(),
                X = C.ReadUH(),
                Y = C.ReadUH(),
                Z = C.ReadUH(),
                WeaponId = C.ReadD()
            };
            if (GenLog)
            {
                CLogger.Print("Slot: " + Action.Slot + " Type: " + info.DeathType + " Hit: " + info.HitPart + " X: " + info.X + " Y: " + info.Y + " Z: " + info.Z + " WeaponId: " + info.WeaponId, LoggerType.Warning);
            }
            return info;
        }
        public static void WriteInfo(SyncServerPacket S, WeaponHost Info)
        {
            S.WriteC(Info.DeathType);
            S.WriteC(Info.HitPart);
            S.WriteH(Info.X);
            S.WriteH(Info.Y);
            S.WriteH(Info.Z);
            S.WriteD(Info.WeaponId);
        }
    }
}