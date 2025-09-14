using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Match.Data.Models.SubHead;

namespace Server.Match.Network.Actions.SubHead
{
    public class ObjectAnim
    {
        public static ObjectAnimInfo ReadInfo(SyncClientPacket C, bool GenLog)
        {
            ObjectAnimInfo Info = new ObjectAnimInfo()
            {
                Life = C.ReadUH(),
                Anim1 = C.ReadC(),
                Anim2 = C.ReadC(),
                SyncDate = C.ReadT()
            };
            if (GenLog)
            {
                CLogger.Print($"Sub Head: ObjectAnim; Life: {Info.Life}; Animation[1]: {Info.Anim1}; Animation[2]: {Info.Anim2}; Sync: {Info.SyncDate}", LoggerType.Warning);
            }
            return Info;
        }
        public static void WriteInfo(SyncServerPacket S, ObjectAnimInfo Info)
        {
            S.WriteH(Info.Life);
            S.WriteC(Info.Anim1);
            S.WriteC(Info.Anim2);
            S.WriteT(Info.SyncDate);
        }
    }
}