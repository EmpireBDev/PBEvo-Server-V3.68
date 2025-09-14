using Server.Match.Data.Enums;
using Server.Match.Data.Models.Event;
using System.Collections.Generic;
using Server.Match.Data.Utils;
using Plugin.Core.Network;
using Plugin.Core.Utility;
using Plugin.Core;
using Plugin.Core.Enums;
using Server.Match.Data.Models;
using System.Net.Sockets;

namespace Server.Match.Network.Actions.Event
{
    public class HitData
    {
        public static List<HitDataInfo> ReadInfo(ActionModel Action, SyncClientPacket C, bool genLog, bool OnlyBytes = false)
        {
            List<HitDataInfo> Hits = new List<HitDataInfo>();
            int Count = C.ReadC();
            for (int i = 0; i < Count; i++)
            {
                HitDataInfo Hit = new HitDataInfo()
                {
                    StartBullet = C.ReadTV(),
                    EndBullet = C.ReadTV(),
                    BulletPos = C.ReadTV(),
                    BoomInfo = C.ReadUH(),
                    ObjectId = C.ReadUH(),
                    HitIndex = C.ReadUD(),
                    WeaponId = C.ReadD(),
                    Extensions = C.ReadC(),
                    Unk = C.ReadC()
                };
                if (!OnlyBytes)
                {
                    Hit.HitEnum = (HitType)AllUtils.GetHitHelmet(Hit.HitIndex);
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
                if (genLog)
                {
                    CLogger.Print($"Slot: {Action.Slot}; Weapon Id: {Hit.WeaponId}; HitData: [Start]: X: {Hit.StartBullet.X}; Y: {Hit.StartBullet.Y}; Z: {Hit.StartBullet.Z}", LoggerType.Warning);
                    CLogger.Print($"Slot: {Action.Slot}; Weapon Id: {Hit.WeaponId}; HitData: [Ended]: X: {Hit.EndBullet.X}; Y: {Hit.EndBullet.Y}; Z: {Hit.EndBullet.Z}", LoggerType.Warning);
                }
                Hits.Add(Hit);
            }
            return Hits;
        }
        public static void WriteInfo(SyncServerPacket S, List<HitDataInfo> Hits)
        {
            S.WriteC((byte)Hits.Count);
            foreach (HitDataInfo Hit in Hits)
            {
                S.WriteTV(Hit.StartBullet);
                S.WriteTV(Hit.EndBullet);
                S.WriteTV(Hit.BulletPos);
                S.WriteH(Hit.BoomInfo);
                S.WriteH(Hit.ObjectId);
                S.WriteD(Hit.HitIndex);
                S.WriteD(Hit.WeaponId);
                S.WriteC(Hit.Extensions);
                S.WriteC(Hit.Unk);

            }
        }
    }
}