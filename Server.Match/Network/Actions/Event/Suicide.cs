using Server.Match.Data.Models.Event;
using System.Collections.Generic;
using Plugin.Core;
using Plugin.Core.Network;
using Plugin.Core.Enums;
using Server.Match.Data.Models;
using Plugin.Core.Utility;

namespace Server.Match.Network.Actions.Event
{
    public class Suicide
    {
        public static List<SuicideInfo> ReadInfo(ActionModel Action, SyncClientPacket C, bool GenLog, bool OnlyBytes = false)
        {
            List<SuicideInfo> Hits = new List<SuicideInfo>();
            int Count = C.ReadC();
            for (int i = 0; i < Count; i++)
            {
                SuicideInfo Hit = new SuicideInfo()
                {
                    PlayerPos = C.ReadUHV(),
                    WeaponId = C.ReadD(),
                    Extensions = C.ReadC(),
                    ObjectId = C.ReadC(),
                    HitInfo = C.ReadUD(),
                };
                if (!OnlyBytes)
                {
                    Hit.WeaponClass = (ClassType)ComDiv.GetIdStatics(Hit.WeaponId, 2); //Funcional? Opcode = >> 32) & 31 | New = >> 32) & 63
                }
                if (GenLog)
                {
                    CLogger.Print($"Slot: {Action.Slot}; Suicide: Hit: {Hit.HitInfo} WeaponId: {Hit.WeaponId} X: {Hit.PlayerPos.X} Y: {Hit.PlayerPos.Y} Z: {Hit.PlayerPos.Z}", LoggerType.Warning);
                }
                Hits.Add(Hit);
            }
            return Hits;
        }
        public static void WriteInfo(SyncServerPacket S, List<SuicideInfo> Hits)
        {
            S.WriteC((byte)Hits.Count);
            foreach (SuicideInfo Hit in Hits)
            {
                S.WriteHV(Hit.PlayerPos);
                S.WriteD(Hit.WeaponId);
                S.WriteC(Hit.Extensions);
                S.WriteC(Hit.ObjectId);
                S.WriteD(Hit.HitInfo);
            }
        }
    }
}