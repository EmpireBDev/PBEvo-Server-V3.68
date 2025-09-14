using Plugin.Core.Enums;
using Plugin.Core.SharpDX;
using Plugin.Core.Utility;
using Server.Match.Data.Enums;
using Server.Match.Data.Models;
using Server.Match.Data.Sync.Server;
using Server.Match.Data.Utils;
using System.Collections.Generic;

namespace Server.Match.Network.Actions.Damage
{
    public class DamageManager
    {
        public static List<AssistModel> Assists = new List<AssistModel>();
        public static void SabotageDestroy(RoomModel Room, PlayerModel Player, ObjectModel ObjM, ObjectInfo ObjI, int Damage)
        {
            if (ObjM.UltraSync > 0 && (Room.RoomType == RoomCondition.Destroy || Room.RoomType == RoomCondition.Defense))
            {
                if (ObjM.UltraSync == 1 || ObjM.UltraSync == 3)
                {
                    Room.Bar1 = ObjI.Life;
                }
                else if (ObjM.UltraSync == 2 || ObjM.UltraSync == 4)
                {
                    Room.Bar2 = ObjI.Life;
                }
                SendMatchInfo.SendSabotageSync(Room, Player, Damage, ObjM.UltraSync == 4 ? 2 : 1);
            }
        }
        public static void SetDeath(List<DeathServerData> Deaths, PlayerModel Player, PlayerModel Killer, CharaDeath DeathType)
        {
            lock (Assists)
            {
                AssistModel Assist = Assists.Find(x => x.Victim == Player.Slot);
                Player.Life = 0;
                Player.Dead = true;
                Player.LastDie = DateTimeUtil.Now();
                DeathServerData Death = new DeathServerData()
                {
                    Player = Player,
                    DeathType = DeathType,
                    Assist = Assist == null ? Killer.Slot : Assist.IsAssist == true ? Assist.Killer : Killer.Slot
                };
                Deaths.Add(Death);
                Assists.Remove(Assist);
                foreach (AssistModel AssistFix in Assists.FindAll(x => x.Victim == Player.Slot))
                {
                    Assists.Remove(AssistFix);
                }
            }
        }
        public static void SetHitEffect(List<ObjectHitInfo> Objs, PlayerModel Player, PlayerModel Killer, CharaDeath DeathType, CharaHitPart HitPart)
        {
            ObjectHitInfo Obj = new ObjectHitInfo(6)
            {
                ObjId = Player.Slot,
                KillerId = Killer.Slot,
                DeathType = DeathType,
                ObjLife = Player.Life,
                HitPart = HitPart
            };
            Objs.Add(Obj);
        }
        public static void SetHitEffect(List<ObjectHitInfo> objs, PlayerModel Player, CharaDeath DeathType, CharaHitPart HitPart)
        {
            ObjectHitInfo Obj = new ObjectHitInfo(6)
            {
                ObjId = Player.Slot,
                KillerId = Player.Slot,
                DeathType = DeathType,
                ObjLife = Player.Life,
                HitPart = HitPart
            };
            objs.Add(Obj);
        }
        public static void BoomDeath(RoomModel room, PlayerModel Killer, int WeaponId, List<DeathServerData> Deaths, List<ObjectHitInfo> objs, List<int> BoomPlayers)
        {
            if (BoomPlayers == null || BoomPlayers.Count == 0)
            {
                return;
            }
            for (int i = 0; i < BoomPlayers.Count; i++)
            {
                int Slot = BoomPlayers[i];
                if (room.GetPlayer(Slot, out PlayerModel Player) && !Player.Dead)
                {
                    SetDeath(Deaths, Player, Killer, CharaDeath.OBJECT_EXPLOSION);
                    ObjectHitInfo Obj = new ObjectHitInfo(2)
                    {
                        HitPart = CharaHitPart.ALL,
                        DeathType = CharaDeath.OBJECT_EXPLOSION,
                        ObjId = Slot,
                        KillerId = Killer.Slot,
                        WeaponId = WeaponId,
                    };
                    objs.Add(Obj);
                }
                foreach (AssistModel AssistFix in Assists.FindAll(x => x.RoomId == room.RoomId))
                {
                    Assists.Remove(AssistFix);
                }
            }
        }
        public static void SimpleDeath(RoomModel Room, List<DeathServerData> deaths, List<ObjectHitInfo> objs, PlayerModel Killer, PlayerModel Victim, int damage, int WeaponId, CharaHitPart HitPart, CharaDeath DeathType)
        {
            Victim.Life -= damage;
            AllUtils.BaseAssistLogic(Room, Killer, Victim, damage);
            lock (Assists)
            {
                foreach (AssistModel Assist in Assists.FindAll(x => x.Victim == Victim.Slot))
                {
                    if (!Assist.IsKiller)
                    {
                        Assist.IsAssist = true;
                    }
                }
            }
            if (Victim.Life <= 0)
            {
                SetDeath(deaths, Victim, Killer, DeathType);
            }
            else
            {
                SetHitEffect(objs, Victim, Killer, DeathType, HitPart);
            }
            ObjectHitInfo Obj = new ObjectHitInfo(2)
            {
                ObjId = Victim.Slot,
                ObjLife = Victim.Life,
                HitPart = HitPart,
                KillerId = Killer.Slot,
                Position = ((Vector3)Victim.Position - Killer.Position),
                DeathType = DeathType,
                WeaponId = WeaponId
            };
            objs.Add(Obj);
        }
    }
}