using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Match.Data.Models.SubHead;

namespace Server.Match.Network.Actions.SubHead
{
    public class StageInfoObjAnim
    {
        public static StageAnimInfo ReadInfo(SyncClientPacket C, bool GenLog)
        {
            StageAnimInfo Info = new StageAnimInfo()
            {
                Unk = C.ReadC(),
                Life = C.ReadUH(),
                SyncDate = C.ReadT(),
                Anim1 = C.ReadC(),
                Anim2 = C.ReadC()
            };
            if (GenLog)
            {
                CLogger.Print($"Sub Head: StageObjAnim; Unk: {Info.Unk}; Life: {Info.Life}; Sync: {Info.SyncDate}; Animation[1]: {Info.Anim1}; Animation[2]: {Info.Anim2}", LoggerType.Warning);
            }
            return Info;
        }
        public static void WriteInfo(SyncServerPacket S, StageAnimInfo Info)
        {
            S.WriteC(Info.Unk);
            S.WriteH(Info.Life);
            S.WriteT(Info.SyncDate);
            S.WriteC(Info.Anim1);
            S.WriteC(Info.Anim2);
        }
    }
}