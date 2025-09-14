using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Plugin.Core.Utility;
using Server.Match.Data.Managers;
using Server.Match.Data.Models;
using Server.Match.Data.Utils;
using Server.Match.Data.XML;
using Server.Match.Network.Packets;
using System;
using System.Collections.Generic;

namespace Server.Match.Data.Sync.Client
{
    public class RespawnSync
    {
        public static List<AssistModel> Assists = new List<AssistModel>();
        public static void Load(SyncClientPacket C)
        {
            uint UniqueRoomId = C.ReadUD();
            uint RoomSeed = C.ReadUD();
            long RoomTick = C.ReadQ();
            int SyncType = C.ReadC();
            int Round = C.ReadC();
            int SlotId = C.ReadC();
            int SpawnNumber = C.ReadC();
            byte AccountId = C.ReadC();
            int CharaId = 0;
            int HpBonus = 0;
            int WpnPrimary = 0;
            int WpnSecondary = 0;
            int WpnMelee = 0;
            int WpnExplosive = 0;
            int WpnSpecial = 0;
            bool C4Speed = false;
            if (SyncType == 0 || SyncType == 2)
            {
                CharaId = C.ReadD();
                HpBonus = C.ReadC();
                C4Speed = C.ReadC() == 1;
                WpnPrimary = C.ReadD();
                WpnSecondary = C.ReadD();
                WpnMelee = C.ReadD();
                WpnExplosive = C.ReadD();
                WpnSpecial = C.ReadD();
                if (C.ToArray().Length > 49)
                {
                    CLogger.Print($"RespawnSync (Length > 48): {C.ToArray().Length}", LoggerType.Warning);
                }
            }
            else
            {
                if (C.ToArray().Length > 23)
                {
                    CLogger.Print($"RespawnSync (Length > 23): {C.ToArray().Length}", LoggerType.Warning);
                }
            }
            RoomModel Room = RoomsManager.GetRoom(UniqueRoomId, RoomSeed);
            if (Room == null)
            {
                return;
            }
            lock (Assists)
            {
                AssistModel Assist = Assists.Find(x => x.RoomId == Room.RoomId);
                if (Assist != null && Assists.Remove(Assist))
                {
                    foreach (AssistModel AssistFix in Assists.FindAll(x => x.RoomId == Room.RoomId))
                    {
                        Assists.Remove(AssistFix);
                    }
                }
            }
            Room.ResyncTick(RoomTick, RoomSeed);
            PlayerModel Player = Room.GetPlayer(SlotId, true);
            if (Player != null)
            {
                if (Player.PlayerIdByUser != AccountId)
                {
                    CLogger.Print($"Invalid User Ids: [By User: {Player.PlayerIdByUser} / Server: {AccountId}]", LoggerType.Warning);
                }
                if (Player.PlayerIdByUser == AccountId)
                {
                    Player.PlayerIdByServer = AccountId;
                    Player.RespawnByServer = SpawnNumber;
                    Player.Integrity = false;
                    if (Round > Room.ServerRound)
                    {
                        Room.ServerRound = Round;
                    }
                    if (SyncType == 0 || SyncType == 2) //0 = Entering the match (normally) | 1 = Entering spectator match (Destroy/Suppression) | 2 = Entering the match (normally/not the player's first respawn)
                    {
                        Player.Dead = false;
                        Player.PlantDuration = ConfigLoader.PlantDuration;
                        Player.DefuseDuration = ConfigLoader.DefuseDuration;
                        Player.Equip = new Equipment()
                        {
                            WpnPrimary = WpnPrimary,
                            WpnSecondary = WpnSecondary,
                            WpnMelee = WpnMelee,
                            WpnExplosive = WpnExplosive,
                            WpnSpecial = WpnSpecial
                        };
                        if (C4Speed)
                        {
                            Player.PlantDuration -= ComDiv.Percentage(ConfigLoader.PlantDuration, 50);
                            Player.DefuseDuration -= ComDiv.Percentage(ConfigLoader.DefuseDuration, 25);
                        }
                        if (!Room.BotMode)
                        {
                            if (Room.SourceToMap == -1)
                            {
                                Room.RoundResetRoomF1(Round);
                            }
                            else
                            {
                                Room.RoundResetRoomS1(Round);
                            }
                        }
                        if (CharaId == -1)
                        {
                            Player.Immortal = true;
                        }
                        else
                        {
                            Player.Immortal = false;
                            int CharaHp = CharaStructureXML.GetCharaHP(CharaId);
                            CharaHp += ComDiv.Percentage(CharaHp, HpBonus);
                            Player.MaxLife = CharaHp;
                            Player.ResetLife();
                        }
                    }
                    if (Room.BotMode || SyncType == 2 || !Room.ObjectsIsValid())
                    {
                        return;
                    }
                    List<ObjectHitInfo> SyncList = new List<ObjectHitInfo>();
                    foreach (ObjectInfo ObjI in Room.Objects)
                    {
                        ObjectModel ObjM = ObjI.Model;
                        if (ObjM != null && (SyncType != 2 && ObjM.Destroyable && ObjI.Life != ObjM.Life || ObjM.NeedSync))
                        {
                            ObjectHitInfo Obj = new ObjectHitInfo(3)
                            {
                                ObjSyncId = ObjM.NeedSync ? 1 : 0,
                                AnimId1 = ObjM.Animation,
                                AnimId2 = ObjI.Animation != null ? ObjI.Animation.Id : 255,
                                DestroyState = ObjI.DestroyState,
                                ObjId = ObjM.Id,
                                ObjLife = ObjI.Life,
                                SpecialUse = AllUtils.GetDuration(ObjI.UseDate)
                            };
                            SyncList.Add(Obj);
                        }
                    }
                    foreach (PlayerModel Member in Room.Players)
                    {
                        if (Member.Slot != SlotId && Member.AccountIdIsValid() && !Member.Immortal && Member.StartTime != new DateTime() && (Member.MaxLife != Member.Life || Member.Dead))
                        {
                            ObjectHitInfo Obj = new ObjectHitInfo(4)
                            {
                                ObjId = Member.Slot,
                                ObjLife = Member.Life
                            };
                            SyncList.Add(Obj);
                        }
                    }
                    if (SyncList.Count > 0)
                    {
                        byte[] Actions = PROTOCOL_EVENTS_ACTION.GET_CODE(SyncList);
                        byte[] Packet = AllUtils.BaseWriteCode(4, Actions, 255, AllUtils.GetDuration(Room.StartTime), Round);
                        MatchXender.Client.SendPacket(Packet, Player.Client);
                    }
                    SyncList.Clear();
                    SyncList = null;
                }
            }
        }
    }
}
