using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Match.Data.Models;
using Server.Match.Data.Models.Event;

namespace Server.Match.Network.Actions.Event
{
    public class CharaFireNHitData
    {
        public static CharaFireNHitDataInfo ReadInfo(ActionModel Action, SyncClientPacket C, bool GenLog)
        {
            CharaFireNHitDataInfo Hit = new CharaFireNHitDataInfo()
            {
                HitInfo = C.ReadUD(),
                Extensions = C.ReadC(),
                WeaponId = C.ReadD(),
                Unk = C.ReadUH(),
                X = C.ReadUH(),
                Y = C.ReadUH(),
                Z = C.ReadUH()
            };
            if (GenLog)
            {
                CLogger.Print($"Slot: {Action.Slot}; Weapon Id: {Hit.WeaponId}; X: {Hit.X} Y: {Hit.Y} Z: {Hit.Z}", LoggerType.Warning);
            }
            return Hit;
        }
        public static void WriteInfo(SyncServerPacket S, CharaFireNHitDataInfo Hit)
        {
            S.WriteD(Hit.HitInfo);
            S.WriteC(Hit.Extensions);
            S.WriteD(Hit.WeaponId);
            S.WriteH(Hit.Unk);
            S.WriteH(Hit.X);
            S.WriteH(Hit.Y);
            S.WriteH(Hit.Z);
        }
    }
}