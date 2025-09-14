using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.SharpDX;
using Plugin.Core.Utility;
using Plugin.Core.XML;
using Server.Match.Data.Utils;
using Server.Match.Data.XML;
using System;
using System.Collections.Generic;
using System.Net;

namespace Server.Match.Data.Models
{
    public class RoomModel
    {
        public PlayerModel[] Players = new PlayerModel[16];
        public ObjectInfo[] Objects = new ObjectInfo[200];
        public DateTime LastObjsSync;
        public DateTime LastPlayersSync;
        public uint UniqueRoomId;
        public uint RoomSeed;
        public int ObjsSyncRound;
        public int ServerRound;
        public int SourceToMap = -1;
        public int ServerId;
        public int RoomId;
        public int ChannelId;
        public int LastRound;
        public int DropCounter;
        public int Bar1 = 6000;
        public int Bar2 = 6000;
        public int Default1 = 6000;
        public int Default2 = 6000;
        public MapIdEnum MapId;
        public MapRules Rule;
        public RoomCondition RoomType;
        public SChannelModel Server;
        public MapModel Map;
        private readonly object Lock = new object();
        private readonly object Lock2 = new object();
        public bool BotMode;
        public bool HasC4;
        public long LastStartTick;
        public Half3 BombPosition;
        public DateTime StartTime;
        public RoomModel(int ServerId)
        {
            Server = SChannelXML.GetServer(ServerId);
            if (Server == null)
            {
                return;
            }
            this.ServerId = ServerId;
            for (int i = 0; i < 16; ++i)
            {
                Players[i] = new PlayerModel(i);
            }
            for (int i = 0; i < 200; ++i)
            {
                Objects[i] = new ObjectInfo(i);
            }
        }
        public void SyncInfo(List<ObjectHitInfo> Objs, int Type)
        {
            lock (Lock2)
            {
                if (BotMode || !ObjectsIsValid())
                {
                    return;
                }
                DateTime Now = DateTimeUtil.Now();
                double TimeObjects = (Now - LastObjsSync).TotalSeconds;
                double TimePlayers = (Now - LastPlayersSync).TotalSeconds;
                if (TimeObjects >= 2.5 && (Type & 1) == 1)
                {
                    LastObjsSync = Now;
                    foreach (ObjectInfo rObj in Objects)
                    {
                        ObjectModel mObj = rObj.Model;
                        if (mObj != null && (mObj.Destroyable && rObj.Life != mObj.Life || mObj.NeedSync))
                        {
                            float SyncingTime = AllUtils.GetDuration(rObj.UseDate);
                            AnimModel anim = rObj.Animation;
                            if (anim != null && anim.Duration > 0 && SyncingTime >= anim.Duration)
                            {
                                mObj.GetAnim(anim.NextAnim, SyncingTime, anim.Duration, rObj);
                            }
                            ObjectHitInfo Obj = new ObjectHitInfo(mObj.UpdateId)
                            {
                                ObjSyncId = mObj.NeedSync ? 1 : 0,
                                AnimId1 = mObj.Animation,
                                AnimId2 = rObj.Animation != null ? rObj.Animation.Id : 255,
                                DestroyState = rObj.DestroyState,
                                ObjId = mObj.Id,
                                ObjLife = rObj.Life,
                                SpecialUse = SyncingTime
                            };
                            Objs.Add(Obj);
                        }
                    }
                }
                if (TimePlayers >= 1.5 /*6.5*/ && (Type & 2) == 2)
                {
                    LastPlayersSync = Now;
                    for (int i = 0; i < Players.Length; i++)
                    {
                        PlayerModel Player = Players[i];
                        if (!Player.Immortal && (Player.MaxLife != Player.Life || Player.Dead))
                        {
                            ObjectHitInfo Obj = new ObjectHitInfo(4)
                            {
                                ObjId = Player.Slot,
                                ObjLife = Player.Life
                            };
                            Objs.Add(Obj);
                        }
                    }
                }
            }
        }
        public bool ObjectsIsValid()
        {
            return ServerRound == ObjsSyncRound;
        }
        public void ResyncTick(long StartTick, uint Seed)
        {
            if (StartTick > LastStartTick)
            {
                StartTime = new DateTime(StartTick);
                if (LastStartTick > 0)
                {
                    ResetRoomInfo(Seed);
                }
                LastStartTick = StartTick;
                if (ConfigLoader.IsTestMode)
                {
                    CLogger.Print($"New tick is defined. [{LastStartTick}]", LoggerType.Warning);
                }
            }
        }
        public void ResetRoomInfo(uint Seed)
        {
            for (int i = 0; i < 200; ++i)
            {
                Objects[i] = new ObjectInfo(i);
            }
            MapId = (MapIdEnum)AllUtils.GetSeedInfo(Seed, 2);
            RoomType = (RoomCondition)AllUtils.GetSeedInfo(Seed, 0);
            Rule = (MapRules)AllUtils.GetSeedInfo(Seed, 1);
            SourceToMap = -1;
            Map = null;
            LastRound = 0;
            DropCounter = 0;
            BotMode = false;
            HasC4 = false;
            ServerRound = 0;
            ObjsSyncRound = 0;
            LastObjsSync = new DateTime();
            LastPlayersSync = new DateTime();
            BombPosition = new Half3();
            if (ConfigLoader.IsTestMode)
            {
                CLogger.Print("A room has been reseted by server.", LoggerType.Warning);
            }
        }
        public bool RoundResetRoomF1(int Round)
        {
            lock (Lock)
            {
                if (LastRound != Round)
                {
                    if (ConfigLoader.IsTestMode)
                    {
                        CLogger.Print($"Reseting room. [Last: {LastRound}; New: {Round}]", LoggerType.Warning);
                    }
                    DateTime now = DateTimeUtil.Now();
                    LastRound = Round;
                    HasC4 = false;
                    BombPosition = new Half3();
                    DropCounter = 0;
                    ObjsSyncRound = 0;
                    SourceToMap = (int)MapId;
                    if (!BotMode)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            PlayerModel Player = Players[i];
                            Player.Life = Player.MaxLife;
                        }
                        LastPlayersSync = now;
                        Map = MapStructureXML.GetMapId((int)MapId);
                        List<ObjectModel> objsm = Map?.Objects;
                        if (objsm != null)
                        {
                            foreach (ObjectModel ob in objsm)
                            {
                                ObjectInfo obj = Objects[ob.Id];
                                obj.Life = ob.Life;
                                if (!ob.NoInstaSync)
                                {
                                    ob.GetRandomAnimation(this, obj);
                                }
                                else
                                {
                                    obj.Animation = new AnimModel { NextAnim = 1 };
                                    obj.UseDate = now;
                                }
                                obj.Model = ob;
                                obj.DestroyState = 0;
                                MapStructureXML.SetObjectives(ob, this);
                            }
                        }
                        LastObjsSync = now;
                        ObjsSyncRound = Round;
                    }
                    return true;
                }
            }
            return false;
        }
        public bool RoundResetRoomS1(int Round)
        {
            lock (Lock)
            {
                if (LastRound != Round)
                {
                    if (ConfigLoader.IsTestMode)
                    {
                        CLogger.Print($"Reseting room. [Last: {LastRound}; New: {Round}]", LoggerType.Warning);
                    }
                    LastRound = Round;
                    HasC4 = false;
                    DropCounter = 0;
                    BombPosition = new Half3();
                    if (!BotMode)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            PlayerModel Player = Players[i];
                            Player.Life = Player.MaxLife;
                        }
                        DateTime now = DateTimeUtil.Now();
                        LastPlayersSync = now;
                        foreach (ObjectInfo obj in Objects)
                        {
                            ObjectModel ob = obj.Model;
                            if (ob != null)
                            {
                                obj.Life = ob.Life;
                                if (!ob.NoInstaSync)
                                {
                                    ob.GetRandomAnimation(this, obj);
                                }
                                else
                                {
                                    obj.Animation = new AnimModel { NextAnim = 1 };
                                    obj.UseDate = now;
                                }
                                obj.DestroyState = 0;
                            }
                        }
                        LastObjsSync = now;
                        ObjsSyncRound = Round;
                        if ((RoomType == RoomCondition.Destroy || RoomType == RoomCondition.Defense))
                        {
                            Bar1 = Default1;
                            Bar2 = Default2;
                        }
                    }
                    return true;
                }
            }
            return false;
        }
        public PlayerModel AddPlayer(IPEndPoint Client, PacketModel Packet, string Udp)
        {
            if (ConfigLoader.UdpVersion != Udp)
            {
                if (ConfigLoader.IsTestMode)
                {
                    CLogger.Print($"Wrong UDP Version ({Udp}); Player can't be connected into it!", LoggerType.Warning);
                }
                return null;
            }
            try
            {
                PlayerModel Player = Players[Packet.Slot];
                if (!Player.CompareIp(Client))
                {
                    Player.Client = Client;
                    Player.StartTime = Packet.ReceiveDate;
                    Player.PlayerIdByUser = Packet.AccountId;
                    return Player;
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
            return null;
        }
        public bool GetPlayer(int Slot, out PlayerModel Player)
        {
            try
            {
                Player = Players[Slot];
            }
            catch
            {
                Player = null;
            }
            return Player != null;
        }
        public PlayerModel GetPlayer(int Slot, bool Active)
        {
            PlayerModel Player;
            try
            {
                Player = Players[Slot];
            }
            catch
            {
                Player = null;
            }
            return Player != null && (!Active || Player.Client != null) ? Player : null;
        }
        public PlayerModel GetPlayer(int Slot, IPEndPoint Client)
        {
            PlayerModel Player;
            try
            {
                Player = Players[Slot];
            }
            catch
            {
                Player = null;
            }
            return Player != null && Player.CompareIp(Client) ? Player : null;
        }
        public ObjectInfo GetObject(int Id)
        {
            ObjectInfo Object;
            try
            {
                Object = Objects[Id];
            }
            catch
            {
                Object = null;
            }
            return Object;
        }
        public bool RemovePlayer(IPEndPoint Client, PacketModel Packet, string Udp)
        {
            if (ConfigLoader.UdpVersion != Udp)
            {
                if (ConfigLoader.IsTestMode)
                {
                    CLogger.Print($"Wrong UDP Version ({Udp}); Player can't be disconnected on it!", LoggerType.Warning);
                }
                return false;
            }
            try
            {
                PlayerModel Player = Players[Packet.Slot];
                if (Player.CompareIp(Client))
                {
                    Player.ResetAllInfos();
                    return true;
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
            return false;
        }
        public int GetPlayersCount()
        {
            int Total = 0;
            for (int i = 0; i < 16; i++)
            {
                if (Players[i].Client != null)
                {
                    Total++;
                }
            }
            return Total;
        }
    }
}