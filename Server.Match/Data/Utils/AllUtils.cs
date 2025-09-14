using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Plugin.Core.SharpDX;
using Plugin.Core.Utility;
using Server.Match.Data.Enums;
using Server.Match.Data.Models;
using Server.Match.Data.Models.Event;
using Server.Match.Data.XML;
using Server.Match.Network.Actions.Damage;
using System;
using System.Collections;
using System.Net.NetworkInformation;

namespace Server.Match.Data.Utils
{
    public class AllUtils
    {
        public static float GetDuration(DateTime Date)
        {
            return (float)(DateTimeUtil.Now() - Date).TotalSeconds;
        }
        public static ItemClass ItemClassified(ClassType ClassWeapon)
        {
            ItemClass Value = ItemClass.Unknown;
            if (ClassWeapon == ClassType.Assault)
            {
                Value = ItemClass.Primary;
            }
            else if (ClassWeapon == ClassType.SMG || ClassWeapon == ClassType.DualSMG)
            {
                Value = ItemClass.Primary;
            }
            else if (ClassWeapon == ClassType.Sniper)
            {
                Value = ItemClass.Primary;
            }
            else if (ClassWeapon == ClassType.Shotgun || ClassWeapon == ClassType.DualShotgun)
            {
                Value = ItemClass.Primary;
            }
            else if (ClassWeapon == ClassType.Machinegun)
            {
                Value = ItemClass.Primary;
            }
            else if (ClassWeapon == ClassType.HandGun || ClassWeapon == ClassType.DualHandGun || ClassWeapon == ClassType.CIC)
            {
                Value = ItemClass.Secondary;
            }
            else if (ClassWeapon == ClassType.Knife || ClassWeapon == ClassType.DualKnife || ClassWeapon == ClassType.Knuckle)
            {
                Value = ItemClass.Melee;
            }
            else if (ClassWeapon == ClassType.ThrowingGrenade)
            {
                Value = ItemClass.Explosive;
            }
            else if (ClassWeapon == ClassType.ThrowingSpecial)
            {
                Value = ItemClass.Special;
            }
            else if (ClassWeapon == ClassType.Dino)
            {
                Value = ItemClass.Unknown;
            }
            return Value;
        }
        public static ObjectType GetHitType(uint HitInfo)
        {
            return (ObjectType)(HitInfo & 3); //Max=4
        }
        public static int GetHitWho(uint HitInfo)
        {
            return (int)((HitInfo >> 2) & 511); //Max=512
        }
        public static CharaHitPart GetHitPart(uint HitInfo)
        {
            return (CharaHitPart)((HitInfo >> 11) & 63); //>> 11 & 63 Max=64
        }
        public static int GetHitDamageBot(uint HitInfo) //Modo BOT
        {
            return (int)(HitInfo >> 20); //(Max=4096)
        }
        public static int GetHitDamageNormal(uint HitInfo) //32768 e 65536
        {
            return (int)(HitInfo >> 21); //(Max=4096)
        }
        public static int GetHitHelmet(uint info)
        {
            return (int)((info >> 17) & 7); //Max=8
        }
        public static CharaDeath GetCharaDeath(uint HitInfo)
        {
            return (CharaDeath)(HitInfo & 15);
        }
        public static int GetKillerId(uint HitInfo)
        {
            return (int)((HitInfo >> 11) & 511);
        }
        public static int GetObjectType(uint HitInfo)
        {
            return (int)((HitInfo >> 10) & 1);
        }
        public static int GetRoomInfo(uint UniqueRoomId, int Type)
        {
            if (Type == 0)
            {
                return (int)(UniqueRoomId & 0xFFF);
            }
            else if (Type == 1)
            {
                return (int)((UniqueRoomId >> 12) & 0xFF);
            }
            else if (Type == 2)
            {
                return (int)((UniqueRoomId >> 20) & 0xFFF);
            }
            return 0;
        }
        public static int GetSeedInfo(uint Seed, int Type)
        {
            if (Type == 0)
            {
                return (int)(Seed & 0xFFF);
            }
            else if (Type == 1)
            {
                return (int)((Seed >> 12) & 0xFF);
            }
            else if (Type == 2)
            {
                return (int)((Seed >> 20) & 0xFFF);
            }
            return 0;
        }
        public static byte[] BaseWriteCode(int Opcode, byte[] Actions, int SlotId, float Time, int Round)
        {
            int Shift = (17 + Actions.Length) % 6 + 1;
            byte[] Buffer = Bitwise.Encrypt(Actions, Shift);
            using (SyncServerPacket S = new SyncServerPacket())
            {
                S.WriteC((byte)Opcode);
                S.WriteC((byte)SlotId);
                S.WriteT(Time);
                S.WriteC((byte)Round);
                S.WriteH((ushort)(17 + Buffer.Length));
                S.WriteD(0);
                S.WriteD(0);
                S.WriteB(Buffer);
                return S.ToArray();
            }
        }
        public static void BaseAssistLogic(RoomModel Room, PlayerModel Killer, PlayerModel Victim, int Damage)
        {
            AssistModel Assist = new AssistModel()
            {
                RoomId = Room.RoomId,
                Killer = Killer.Slot,
                Victim = Victim.Slot,
                Damage = Damage,
                IsKiller = Victim.Life <= 0,
                VictimDead = Victim.Life <= 0
            };
            if (Assist.Killer != Assist.Victim)
            {
                DamageManager.Assists.Add(Assist);
            }
        }
        public static int SlotValue(bool IsBotMode, bool UseMyDate, int Opcode, int Slot, int DedicationSlot)
        {
            if (UseMyDate)
            {
                return Slot;
            }
            else if (Opcode == 3)
            {
                return (IsBotMode ? Slot : 255);
            }
            else
            {
                CLogger.Print($"Dedication Slot Id: {DedicationSlot}; Slot Id: {Slot}; Opcode: {Opcode}", LoggerType.Warning);
                return 255;
            }
        }
        public static bool ValidateHitData(int RawDamage, HitDataInfo Hit, out int Damage)
        {
            if (!ConfigLoader.AntiScript)
            {
                Damage = RawDamage;
                return true;
            }
            ItemsStatistic Item = ItemStatisticXML.GetItemStats(Hit.WeaponId);
            if (Item == null)
            {
                CLogger.Print($"The Item Statistic was not found. Please add: {Hit.WeaponId} to config!", LoggerType.Warning);
                Damage = 0;
                return false;
            }
            ItemClass Class = ItemClassified(Hit.WeaponClass);
            float Range = Vector3.DistanceRange(Hit.StartBullet, Hit.EndBullet);
            if (Class != ItemClass.Melee && Range > Item.Range)
            {
                Damage = 0;
                return false;
            }
            else if (Class == ItemClass.Melee && Range > Item.Range)
            {
                Damage = 0;
                return false;
            }
            if (GetHitPart(Hit.HitIndex) != CharaHitPart.HEAD)
            {
                int PercentualDamage = Item.Damage + (Item.Damage * 30 / 100);
                if (Class != ItemClass.Melee && RawDamage > PercentualDamage)
                {
                    Damage = 0;
                    return false;
                }
                if (Class == ItemClass.Melee)
                {
                    if (RawDamage > Item.Damage)
                    {
                        Damage = 0;
                        return false;
                    }
                }
            }
            Damage = RawDamage;
            return true;
        }
        public static bool ValidateGrenadeHit(int RawDamage, GrenadeHitInfo Hit, out int Damage)
        {
            if (!ConfigLoader.AntiScript)
            {
                Damage = RawDamage;
                return true;
            }
            ItemsStatistic Item = ItemStatisticXML.GetItemStats(Hit.WeaponId);
            if (Item == null)
            {
                CLogger.Print($"The Item Statistic was not found. Please add: {Hit.WeaponId} to config!", LoggerType.Warning);
                Damage = 0;
                return false;
            }
            ItemClass Class = ItemClassified(Hit.WeaponClass);
            float Range = Vector3.DistanceRange(Hit.FirePos, Hit.HitPos);
            if (Class == ItemClass.Explosive)
            {
                if (Range > Item.Range)
                {
                    Damage = 0;
                    return false;
                }
                if (RawDamage > Item.Damage)
                {
                    Damage = 0;
                    return false;
                }
            }
            Damage = RawDamage;
            return true;
        }
        public static void GetDecryptedData(PacketModel Packet)
        {
            try
            {
                byte[] WithEnd, WithoutEnd;
                if (Packet.Data.Length >= Packet.Length)
                {
                    byte[] Result = new byte[Packet.Length - 17];
                    Array.Copy(Packet.Data, 17, Result, 0, Result.Length);
                    WithEnd = Bitwise.Decrypt(Result, Packet.Length % 6 + 1);
                    WithoutEnd = new byte[WithEnd.Length - 9];
                    Array.Copy(WithEnd, WithoutEnd, WithoutEnd.Length);
                    Packet.WithEndData = WithEnd;
                    Packet.WithoutEndData = WithoutEnd;
                }
                else
                {
                    CLogger.Print($"Invalid packet size. (Packet.Data.Length >= Packet.Length): [ {Packet.Data.Length} | {Packet.Length} ]", LoggerType.Warning);
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        public static void RemoveHit(IList List, int Idx) => List.RemoveAt(Idx);
        public static void CheckDataFlags(ActionModel Action, PacketModel Packet)
        {
            UdpGameEvent Flags = Action.Flag;
            if (!Flags.HasFlag(UdpGameEvent.WeaponSync) || Packet.Opcode == 4)
            {
                return;
            }
            if ((Flags & (UdpGameEvent.GetWeaponForClient | UdpGameEvent.DropWeapon)) > 0)
            {
                Action.Flag -= UdpGameEvent.WeaponSync;
            }
        }
        public static int PingTime(string Address, byte[] Buffer, int TTL, int TimeOut, bool IsFragmented, out int Ping)
        {
            int PingResult = 0;
            try
            {
                PingOptions Options = new PingOptions()
                {
                    Ttl = TTL,
                    DontFragment = IsFragmented
                };
                using (Ping PingSender = new Ping())
                {
                    PingReply Reply = PingSender.Send(Address, TimeOut, Buffer, Options);
                    if (Reply.Status == IPStatus.Success)
                    {
                        PingResult = Convert.ToInt32(Reply.RoundtripTime);
                    }
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
            Ping = PingBarSignal(PingResult);
            return PingResult;
        }
        private static byte PingBarSignal(int PlayerLatency)
        {
            if (PlayerLatency <= 100)
            {
                return 5;
            }
            else if (PlayerLatency >= 100 && PlayerLatency <= 200)
            {
                return 4;
            }
            else if (PlayerLatency >= 200 && PlayerLatency <= 300)
            {
                return 3;
            }
            else if (PlayerLatency >= 300 && PlayerLatency <= 400)
            {
                return 2;
            }
            else if (PlayerLatency >= 400 && PlayerLatency <= 500)
            {
                return 1;
            }
            return 0;
        }
    }
}