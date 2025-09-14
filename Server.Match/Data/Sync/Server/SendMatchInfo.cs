using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Plugin.Core.XML;
using Server.Match.Data.Enums;
using Server.Match.Data.Models;
using System;
using System.Collections.Generic;
using System.Net;

namespace Server.Match.Data.Sync.Server
{
    public class SendMatchInfo
    {
        public static void SendPortalPass(RoomModel Room, PlayerModel Player, int Portal)
        {
            try
            {
                if (Room.RoomType != RoomCondition.Boss)
                {
                    return;
                }
                Player.Life = Player.MaxLife;
                IPEndPoint Sync = SynchronizeXML.GetServer(Room.Server.Port).Connection;
                using (SyncServerPacket S = new SyncServerPacket())
                {
                    S.WriteH(1);
                    S.WriteH((short)Room.RoomId);
                    S.WriteH((short)Room.ChannelId);
                    S.WriteH((short)Room.ServerId);
                    S.WriteC((byte)Player.Slot);
                    S.WriteC((byte)Portal);
                    MatchXender.Sync.SendPacket(S.ToArray(), Sync);
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        public static void SendBombSync(RoomModel Room, PlayerModel Player, int Type, int BombArea)
        {
            try
            {
                IPEndPoint Sync = SynchronizeXML.GetServer(Room.Server.Port).Connection;
                using (SyncServerPacket S = new SyncServerPacket())
                {
                    S.WriteH(2);
                    S.WriteH((short)Room.RoomId);
                    S.WriteH((short)Room.ChannelId);
                    S.WriteH((short)Room.ServerId);
                    S.WriteC((byte)Type);
                    S.WriteC((byte)Player.Slot);
                    if (Type == 0)
                    {
                        S.WriteC((byte)BombArea);
                        S.WriteTV(Player.Position);
                        S.WriteH(42); //Unk
                        Room.BombPosition = Player.Position;
                    }
                    MatchXender.Sync.SendPacket(S.ToArray(), Sync);
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        public static void SendDeathSync(RoomModel Room, PlayerModel Killer, int ObjectId, int WeaponId, List<DeathServerData> Deaths)
        {
            try
            {
                IPEndPoint Sync = SynchronizeXML.GetServer(Room.Server.Port).Connection;
                using (SyncServerPacket S = new SyncServerPacket())
                {
                    S.WriteH(3);
                    S.WriteH((short)Room.RoomId);
                    S.WriteH((short)Room.ChannelId);
                    S.WriteH((short)Room.ServerId);
                    S.WriteC((byte)Deaths.Count);
                    S.WriteC((byte)Killer.Slot);
                    S.WriteD(WeaponId);
                    S.WriteTV(Killer.Position);
                    S.WriteC((byte)ObjectId);
                    S.WriteC(0); //For Next Value | New | Enable - Disable Value?
                    foreach (DeathServerData Death in Deaths)
                    {
                        S.WriteC((byte)Death.Player.WeaponClass);
                        S.WriteC((byte)(((int)Death.DeathType * 16) + Death.Player.Slot));
                        S.WriteTV(Death.Player.Position);
                        S.WriteC((byte)Death.Assist);
                        S.WriteB(new byte[8]); //For Next Value | TODO HERE, Expand! | Kill Cam?
                    }
                    MatchXender.Sync.SendPacket(S.ToArray(), Sync);
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        public static void SendHitMarkerSync(RoomModel Room, PlayerModel Player, CharaDeath DeathType, HitType HitEnum, int Damage)
        {
            try
            {
                IPEndPoint Sync = SynchronizeXML.GetServer(Room.Server.Port).Connection;
                using (SyncServerPacket S = new SyncServerPacket())
                {
                    S.WriteH(4);
                    S.WriteH((short)Room.RoomId);
                    S.WriteH((short)Room.ChannelId);
                    S.WriteH((short)Room.ServerId);
                    S.WriteC((byte)Player.Slot);
                    S.WriteC((byte)DeathType);
                    S.WriteC((byte)HitEnum);
                    S.WriteD(Damage);
                    MatchXender.Sync.SendPacket(S.ToArray(), Sync);
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        public static void SendSabotageSync(RoomModel Room, PlayerModel Player, int Damage, int UltraSync)
        {
            try
            {
                IPEndPoint Sync = SynchronizeXML.GetServer(Room.Server.Port).Connection;
                using (SyncServerPacket S = new SyncServerPacket())
                {
                    S.WriteH(5);
                    S.WriteH((short)Room.RoomId);
                    S.WriteH((short)Room.ChannelId);
                    S.WriteH((short)Room.ServerId);
                    S.WriteC((byte)Player.Slot);
                    S.WriteH((ushort)Room.Bar1);
                    S.WriteH((ushort)Room.Bar2);
                    S.WriteC((byte)UltraSync);
                    S.WriteH((ushort)Damage);
                    MatchXender.Sync.SendPacket(S.ToArray(), Sync);
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        public static void SendPingSync(RoomModel Room, PlayerModel Player)
        {
            try
            {
                IPEndPoint Sync = SynchronizeXML.GetServer(Room.Server.Port).Connection;
                using (SyncServerPacket S = new SyncServerPacket())
                {
                    S.WriteH(6);
                    S.WriteH((short)Room.RoomId);
                    S.WriteH((short)Room.ChannelId);
                    S.WriteH((short)Room.ServerId);
                    S.WriteC((byte)Player.Slot);
                    S.WriteC((byte)Player.Ping);
                    S.WriteH((ushort)Player.Latency);
                    MatchXender.Sync.SendPacket(S.ToArray(), Sync);
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}
