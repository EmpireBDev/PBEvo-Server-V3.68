using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Match.Data.Models;
using Server.Match.Data.Models.Event;

namespace Server.Match.Network.Actions.Event
{
    public class RadioChat
    {
        public static RadioChatInfo ReadInfo(ActionModel Action, SyncClientPacket C, bool GenLog)
        {
            RadioChatInfo Info = new RadioChatInfo()
            {
                RadioId = C.ReadC(),
                AreaId = C.ReadC()
            };
            if (GenLog)
            {
                CLogger.Print($"Slot: {Action.Slot} Radio: {Info.RadioId} Area: {Info.AreaId}", LoggerType.Warning);
            }
            return Info;
        }
        public static void WriteInfo(SyncServerPacket S, RadioChatInfo Info)
        {
            S.WriteC(Info.RadioId);
            S.WriteC(Info.AreaId);
        }
    }
}