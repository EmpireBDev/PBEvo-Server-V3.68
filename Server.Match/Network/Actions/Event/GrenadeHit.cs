using Server.Match.Data.Enums;
using Server.Match.Data.Models.Event;
using System.Collections.Generic;
using Plugin.Core;
using Server.Match.Data.Utils;
using Plugin.Core.Network;
using Plugin.Core.Utility;
using Plugin.Core.Enums;
using Server.Match.Data.Models;

namespace Server.Match.Network.Actions.Event
{
    public class GrenadeHit
    {
        public static List<GrenadeHitInfo> ReadInfo(ActionModel Action, SyncClientPacket C, bool GenLog, bool OnlyBytes = false)
        {
            List<GrenadeHitInfo> Hits = new List<GrenadeHitInfo>();
            int Count = C.ReadC();
            for (int i = 0; i < Count; i++)
            {
                GrenadeHitInfo Hit = new GrenadeHitInfo()
                {
                    WeaponId = C.ReadD(),
                    Extensions = C.ReadC(),
                    Unk = C.ReadC(),
                    GrenadesCount = C.ReadUH(),
                    ObjectId = C.ReadUH(),
                    HitInfo = C.ReadUD(),
                    PlayerPos = C.ReadUHV(),
                    FirePos = C.ReadUHV(),
                    HitPos = C.ReadUHV(),
                    BoomInfo = C.ReadUH(),
                    DeathType = (CharaDeath)C.ReadC()
                };
                if (!OnlyBytes)
                {
                    Hit.HitEnum = (HitType)AllUtils.GetHitHelmet(Hit.HitInfo);
                    if (Hit.BoomInfo > 0)
                    {
                        Hit.BoomPlayers = new List<int>();
                        for (int x = 0; x < 16; x++)
                        {
                            int Flag = (1 << x);
                            if ((Hit.BoomInfo & Flag) == Flag)
                            {
                                Hit.BoomPlayers.Add(x);
                            }
                        }
                    }
                    Hit.WeaponClass = (ClassType)ComDiv.GetIdStatics(Hit.WeaponId, 2);
                }
                if (GenLog)
                {
                    CLogger.Print($"Slot: {Action.Slot}; GrenadeHit; [Player Postion] X: {Hit.FirePos.X}; Y: {Hit.FirePos.Y}; Z: {Hit.FirePos.Z}", LoggerType.Warning);
                    CLogger.Print($"Slot: {Action.Slot}; GrenadeHit; [Object Postion] X: {Hit.HitPos.X}; Y: {Hit.HitPos.Y}; Z: {Hit.HitPos.Z}", LoggerType.Warning);
                }
                Hits.Add(Hit);
            }
            return Hits;
        }
        public static void WriteInfo(SyncServerPacket S, List<GrenadeHitInfo> Hits)
        {
            S.WriteC((byte)Hits.Count);
            foreach (GrenadeHitInfo Hit in Hits)
            {
                S.WriteD(Hit.WeaponId);
                S.WriteC(Hit.Extensions);
                S.WriteC(Hit.Unk);
                S.WriteH(Hit.GrenadesCount);
                S.WriteH(Hit.ObjectId);
                S.WriteD(Hit.HitInfo);
                S.WriteHV(Hit.PlayerPos);
                S.WriteHV(Hit.FirePos);
                S.WriteHV(Hit.HitPos);
                S.WriteH(Hit.BoomInfo);
                S.WriteC((byte)Hit.DeathType);
            }
        }
    }
}