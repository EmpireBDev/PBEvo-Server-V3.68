using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Match.Data.Models.SubHead;

namespace Server.Match.Network.Actions.SubHead
{
    public class ObjectStatic
    {
        public static ObjectStaticInfo ReadInfo(SyncClientPacket C, bool GenLog)
        {
            ObjectStaticInfo Info = new ObjectStaticInfo()
            {
                Type = C.ReadUH(),
                Life = C.ReadUH(),
                DestroyedBySlot = C.ReadC()
            };
            if (GenLog)
            {
                CLogger.Print($"Sub Head: ObjectStatic; Life: {Info.Life}; Destroyed: {Info.DestroyedBySlot}", LoggerType.Warning);
            }
            return Info;
        }
        public static void WriteInfo(SyncServerPacket S, ObjectStaticInfo Info)
        {
            S.WriteH(Info.Type);
            S.WriteH(Info.Life);
            S.WriteC(Info.DestroyedBySlot);
        }
    }
}
