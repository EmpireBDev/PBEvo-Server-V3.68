using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Match.Data.Enums;
using Server.Match.Data.Models;
using Server.Match.Data.Models.Event;

namespace Server.Match.Network.Actions.Event
{
    public class MissionData
    {
        public static MissionDataInfo ReadInfo(ActionModel Action, SyncClientPacket C, float Time, bool GenLog, bool OnlyBytes = false)
        {
            MissionDataInfo Info = new MissionDataInfo()
            {
                PlantTime = C.ReadT(),
                Bomb = C.ReadC()
            };
            if (!OnlyBytes)
            {
                Info.BombEnum = (BombFlag)(Info.Bomb & 15);
                Info.BombId = (Info.Bomb >> 4);
            }
            if (GenLog)
            {
                CLogger.Print($"Slot: {Action.Slot}; Bomb: {Info.BombEnum}; Id: {Info.BombId}; PlantTime: {Info.PlantTime}; Time: {Time}", LoggerType.Warning);
            }
            return Info;
        }
        public static void WriteInfo(SyncServerPacket S, MissionDataInfo Info)
        {
            S.WriteT(Info.PlantTime);
            S.WriteC((byte)Info.Bomb);
        }
    }
}