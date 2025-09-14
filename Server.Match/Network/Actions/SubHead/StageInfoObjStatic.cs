using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Match.Data.Models.SubHead;

namespace Server.Match.Network.Actions.SubHead
{
    public class StageInfoObjStatic
    {
        public static StageStaticInfo ReadInfo(SyncClientPacket C, bool GenLog)
        {
            StageStaticInfo Info = new StageStaticInfo()
            {
                IsDestroyed = C.ReadC()
            };
            if (GenLog)
            {
                CLogger.Print($"Sub Head: StageInfoObjStatic; Destroyed: {Info.IsDestroyed}", LoggerType.Warning);
            }
            return Info;
        }
        public static void WriteInfo(SyncServerPacket S, StageStaticInfo Info)
        {
            S.WriteC(Info.IsDestroyed);
        }
    }
}