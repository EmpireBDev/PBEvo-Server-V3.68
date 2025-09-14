using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Match.Data.Models;
using Server.Match.Data.Models.Event;

namespace Server.Match.Network.Actions.Event
{
    public class WeaponRecoil
    {
        public static WeaponRecoilInfo ReadInfo(ActionModel Action, SyncClientPacket C, bool GenLog)
        {
            WeaponRecoilInfo Info = new WeaponRecoilInfo()
            {
                RecoilHorzAngle = C.ReadT(),
                RecoilHorzMax = C.ReadT(),
                RecoilVertAngle = C.ReadT(),
                RecoilVertMax = C.ReadT(),
                Deviation = C.ReadT(),
                Extensions = C.ReadC(),
                WeaponId = C.ReadD(),
                Unk = C.ReadC(),
                RecoilHorzCount = C.ReadC()
            };
            if (GenLog)
            {
                CLogger.Print("Slot: " + Action.Slot + " WeaponId: " + Info.WeaponId, LoggerType.Warning);
            }
            return Info;
        }
        public static void WriteInfo(SyncServerPacket S, WeaponRecoilInfo Info)
        {
            S.WriteT(Info.RecoilHorzAngle);
            S.WriteT(Info.RecoilHorzMax);
            S.WriteT(Info.RecoilVertAngle);
            S.WriteT(Info.RecoilVertMax);
            S.WriteT(Info.Deviation);
            S.WriteC(Info.Extensions);
            S.WriteD(Info.WeaponId);
            S.WriteC(Info.Unk);
            S.WriteC(Info.RecoilHorzCount);
        }
    }
}
