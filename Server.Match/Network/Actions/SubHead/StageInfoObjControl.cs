using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Plugin.Core.Utility;
using Server.Match.Data.Models.SubHead;

namespace Server.Match.Network.Actions.SubHead
{
    public class StageInfoObjControl
    {
        public static StageControlInfo ReadInfo(SyncClientPacket C, bool GenLog)
        {
            StageControlInfo Info = new StageControlInfo()
            {
                Unk = C.ReadB(9)
            };
            if (GenLog)
            {
                CLogger.Print($"Sub Head: StageInfoObjControl; {Bitwise.ToHexData("Controled Object Hex Data", Info.Unk)}", LoggerType.Opcode);
            }
            return Info;
        }
        public static void WriteInfo(SyncServerPacket S, StageControlInfo Info)
        {
            S.WriteB(Info.Unk);
        }
    }
}