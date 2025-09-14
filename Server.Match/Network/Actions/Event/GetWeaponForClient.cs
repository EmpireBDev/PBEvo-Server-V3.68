using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Match.Data.Models;
using Server.Match.Data.Models.Event;

namespace Server.Match.Network.Actions.Event
{
    public class GetWeaponForClient
    {
        public static WeaponClient ReadInfo(ActionModel Action, SyncClientPacket C, bool GenLog)
        {
            WeaponClient Info = new WeaponClient()
            {
                Unk2 = C.ReadD(),
                Unk3 = C.ReadC(),
                AmmoPrin = C.ReadUH(),
                AmmoDual = C.ReadUH(),
                AmmoTotal = C.ReadUH(),
                WeaponFlag = C.ReadC(),
                Extensions = C.ReadC(),
                WeaponId = C.ReadD(),
                Unk1 = C.ReadUH()
            };
            if (GenLog)
            {
                CLogger.Print($"Slot: {Action.Slot}; Get WeaponId: {Info.WeaponId}; Flag: {Info.WeaponFlag}; Extensions: {Info.Extensions}", LoggerType.Warning);
                CLogger.Print($"Ammo Prin: {Info.AmmoPrin}; Ammo Dual: {Info.AmmoDual}; Ammo Total: {Info.AmmoTotal}", LoggerType.Warning);
            }
            return Info;
        }
        public static void WriteInfo(SyncServerPacket S, WeaponClient Info)
        {
            S.WriteD(Info.Unk2);
            S.WriteC(Info.Unk3);
            if (ConfigLoader.UseMaxAmmoInDrop)
            {
                S.WriteH(65535);
                S.WriteH(Info.AmmoDual);
                S.WriteH(10000);
            }
            else
            {
                S.WriteH(Info.AmmoPrin);
                S.WriteH(Info.AmmoDual);
                S.WriteH(Info.AmmoTotal);
            }
            S.WriteC(Info.WeaponFlag);
            S.WriteC(Info.Extensions);
            S.WriteD(Info.WeaponId);
            S.WriteH(Info.Unk1);
        }
    }
}