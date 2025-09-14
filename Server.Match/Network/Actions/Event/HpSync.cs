using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Match.Data.Models;
using Server.Match.Data.Models.Event;

namespace Server.Match.Network.Actions.Event
{
    public class HpSync
    {
        public static HPSyncInfo ReadInfo(ActionModel Action, SyncClientPacket C, bool GenLog)
        {
            HPSyncInfo info = new HPSyncInfo()
            {
                CharaLife = C.ReadUH()
            };
            if (GenLog)
            {
                CLogger.Print($"Slot: {Action.Slot}; is using Chara with HP ({info.CharaLife})", LoggerType.Warning);
            }
            return info;
        }
        public static void WriteInfo(SyncServerPacket S, HPSyncInfo Info)
        {
            S.WriteH(Info.CharaLife);
        }
    }
}