using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.SharpDX;
using Server.Match.Data.Enums;
using System;
using System.Net;

namespace Server.Match.Data.Models
{
    public class PlayerModel
    {
        public int Slot = -1;
        public int Team;
        public int Life = 100;
        public int MaxLife = 100;
        public int PlayerIdByUser = 0;
        public int PlayerIdByServer = 0;
        public int WeaponId;
        public int RespawnByUser = 0;
        public int RespawnByServer = 0;
        public int Ping = 5;
        public int Latency;
        public float PlantDuration;
        public float DefuseDuration;
        public float C4Time;
        public byte Extensions;
        public Half3 Position;
        public IPEndPoint Client;
        public DateTime StartTime;
        public DateTime LastPing;
        public DateTime LastDie;
        public DateTime C4First;
        public Equipment Equip;
        public ClassType WeaponClass;
        public CharaResId CharaRes;
        public bool Dead = true;
        public bool NeverRespawn = true;
        public bool Integrity = true;
        public bool Immortal;
        public PlayerModel(int Slot)
        {
            this.Slot = Slot;
            Team = (Slot % 2);
        }
        public void LogPlayerPos(Half3 EndBullet)
        {
            CLogger.Print($"Player Position X: {Position.X} Y: {Position.Y} Z: {Position.Z}", LoggerType.Warning);
            CLogger.Print($"End Bullet Position X: {EndBullet.X} Y: {EndBullet.Y} Z: {EndBullet.Z}", LoggerType.Warning);
        }
        public bool CompareIp(IPEndPoint Address)
        {
            return Client != null && Address != null && Client.Address.Equals(Address.Address) && Client.Port == Address.Port;
        }
        public bool RespawnIsValid()
        {
            return RespawnByServer == RespawnByUser;
        }
        public bool AccountIdIsValid()
        {
            return PlayerIdByServer == PlayerIdByUser;
        }
        public bool AccountIdIsValid(int Number)
        {
            return PlayerIdByServer == Number;
        }
        public void CheckLifeValue()
        {
            if (Life > MaxLife)
            {
                Life = MaxLife;
            }
        }
        public void ResetAllInfos()
        {
            Client = null;
            StartTime = new DateTime();
            PlayerIdByUser = 0;
            PlayerIdByServer = 0;
            Integrity = true;
            ResetBattleInfos();
        }
        public void ResetBattleInfos()
        {
            RespawnByServer = 0;
            RespawnByUser = 0;
            Immortal = false;
            Dead = true;
            NeverRespawn = true;
            WeaponId = 0;
            LastPing = new DateTime();
            LastDie = new DateTime();
            C4First = new DateTime();
            C4Time = 0;
            Position = new Half3();
            Life = 100;
            MaxLife = 100;
            Ping = 5;
            Latency = 0;
            PlantDuration = ConfigLoader.PlantDuration;
            DefuseDuration = ConfigLoader.DefuseDuration;
        }
        public void ResetLife()
        {
            Life = MaxLife;
        }
    }
}
