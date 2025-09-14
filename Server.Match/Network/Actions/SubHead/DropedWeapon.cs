using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Plugin.Core.SharpDX;
using Server.Match.Data.Models.SubHead;

namespace Server.Match.Network.Actions.SubHead
{
    public class DropedWeapon
    {
        public static DropedWeaponInfo ReadInfo(SyncClientPacket C, bool GenLog)
        {
            DropedWeaponInfo Info = new DropedWeaponInfo()
            {
                WeaponFlag = C.ReadC(),
                X = C.ReadUH(),
                Y = C.ReadUH(),
                Z = C.ReadUH(),
                Unk1 = C.ReadUH(),
                Unk2 = C.ReadUH(),
                Unk3 = C.ReadUH(),
                Unk4 = C.ReadUH()
            };
            if (GenLog)
            {
                Vector3 VEC = new Half3(Info.X, Info.Y, Info.Z);
                CLogger.Print($"Sub Head: DroppedWeapon; Weapon Flag: {Info.WeaponFlag}; X: {VEC.X}; Y: {VEC.Y}; Z: {VEC.Z}", LoggerType.Warning);
            }
            return Info;
        }
        public static void WriteInfo(SyncServerPacket S, DropedWeaponInfo Info)
        {
            S.WriteC(Info.WeaponFlag);
            S.WriteH(Info.X);
            S.WriteH(Info.Y);
            S.WriteH(Info.Z);
            S.WriteH(Info.Unk1);
            S.WriteH(Info.Unk2);
            S.WriteH(Info.Unk3);
            S.WriteH(Info.Unk4);
        }
    }
}