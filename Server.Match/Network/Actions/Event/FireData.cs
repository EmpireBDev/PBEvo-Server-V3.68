using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Match.Data.Models;
using Server.Match.Data.Models.Event;

namespace Server.Match.Network.Actions.Event
{
    public class FireData
    {
        public static FireDataInfo ReadInfo(ActionModel Action, SyncClientPacket C, bool GenLog)
        {
            FireDataInfo Info = new FireDataInfo()
            {
                Effect = C.ReadC(),
                Part = C.ReadC(),
                Index = C.ReadH(),
                WeaponId = C.ReadD(),
                Extensions = C.ReadC(),
                Unk = C.ReadC(),
                X = C.ReadUH(),
                Y = C.ReadUH(),
                Z = C.ReadUH()
            };
            if (GenLog)
            {
                CLogger.Print($"FireData; [1] Effect: {(Info.Effect >> 4)}; Slot: {(Info.Effect & 15)}", LoggerType.Warning);
                CLogger.Print($"FireData; [2] Slot: {Action.Slot} Weapon Id: {Info.WeaponId}; FireData: {Info.Effect}; Part: {Info.Part}", LoggerType.Warning);
            }
            return Info;
        }
        public static void WriteInfo(SyncServerPacket S, FireDataInfo Info)
        {
            S.WriteC(Info.Effect);
            S.WriteC(Info.Part);
            S.WriteH(Info.Index);
            S.WriteD(Info.WeaponId);
            S.WriteC(Info.Extensions);
            S.WriteC(Info.Unk);
            S.WriteH(Info.X);
            S.WriteH(Info.Y);
            S.WriteH(Info.Z);

        }
    }
}