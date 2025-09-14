using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Match.Data.Enums;
using Server.Match.Data.Models;
using Server.Match.Data.Models.Event;
using System.Collections.Generic;

namespace Server.Match.Network.Actions.Event
{
    public class UseObject
    {
        public static List<UseObjectInfo> ReadInfo(ActionModel Action, SyncClientPacket C, bool GenLog)
        {
            List<UseObjectInfo> Infos = new List<UseObjectInfo>();
            int Count = C.ReadC();
            for (int i = 0; i < Count; i++)
            {
                UseObjectInfo Info = new UseObjectInfo()
                {
                    ObjectId = C.ReadUH(),
                    Use = C.ReadC(),
                    SpaceFlags = (CharaMoves)C.ReadC()
                };
                if (GenLog)
                {
                    CLogger.Print($"Slot: {Action.Slot}; Use Object: {Info.Use}; Flag: {Info.SpaceFlags}; ObjectId: {Info.ObjectId}", LoggerType.Warning);
                }
                Infos.Add(Info);
            }
            return Infos;
        }
        public static void WriteInfo(SyncServerPacket S, List<UseObjectInfo> Infos)
        {
            S.WriteC((byte)Infos.Count);
            foreach (UseObjectInfo Info in Infos)
            {
                S.WriteH(Info.ObjectId);
                S.WriteC(Info.Use);
                S.WriteC((byte)Info.SpaceFlags);
            }
        }
    }
}