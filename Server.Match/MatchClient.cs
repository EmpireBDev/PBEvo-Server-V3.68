using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.SharpDX;
using Plugin.Core.Network;
using Plugin.Core.Utility;
using Server.Match.Data.Enums;
using Server.Match.Data.Managers;
using Server.Match.Data.Models;
using Server.Match.Data.Models.Event;
using Server.Match.Data.Models.Event.Event;
using Server.Match.Data.Models.SubHead;
using Server.Match.Data.Sync.Server;
using Server.Match.Data.Utils;
using Server.Match.Network.Actions.Damage;
using Server.Match.Network.Actions.SubHead;
using Server.Match.Network.Actions.Event;
using Server.Match.Network.Packets;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Server.Match
{
    public class MatchClient
    {
        private readonly Socket MainSocket;
        private readonly IPEndPoint RemoteAddress;
        public MatchClient(Socket MainSocket, IPEndPoint RemoteAddress)
        {
            this.MainSocket = MainSocket;
            this.MainSocket.DontFragment = false;
            this.MainSocket.Ttl = 128;
            this.RemoteAddress = RemoteAddress;
        }
        public void BeginReceive(byte[] Buffer, DateTime Date)
        {
            PacketModel Packet = new PacketModel()
            {
                Data = Buffer,
                ReceiveDate = Date
            };
            SyncClientPacket C = new SyncClientPacket(Packet.Data);
            Packet.Opcode = C.ReadC();
            Packet.Slot = C.ReadC();
            Packet.Time = C.ReadT();
            Packet.Round = C.ReadC();
            Packet.Length = C.ReadUH();
            Packet.Respawn = C.ReadC();
            Packet.RoundNumber = C.ReadC();
            Packet.AccountId = C.ReadC();
            Packet.Unk = C.ReadC();
            Packet.Unk2 = C.ReadD();
            if (Packet.Length > Packet.Data.Length)
            {
                CLogger.Print($"Packet with invalid size canceled. [ Length: {Packet.Length} DataLength: {Packet.Data.Length} ]", LoggerType.Warning);
                return;
            }
            AllUtils.GetDecryptedData(Packet); //03 00 80 07 F8 20 04 80 00 (9 Bytes)
            if (ConfigLoader.IsTestMode && Packet.Unk > 0)
            {
                CLogger.Print(Bitwise.ToHexData($"[N] Test Mode, Packet Unk: {Packet.Unk}", Packet.Data), LoggerType.Opcode);
                CLogger.Print(Bitwise.ToHexData($"[D] Test Mode, Packet Unk: {Packet.Unk}", Packet.WithoutEndData), LoggerType.Opcode);
            }
            if (ConfigLoader.EnableLog)
            {
                //byte[] DecryptedData = ComDiv.Unshift(Buffer, Packet.Length % 6 + 1);
                if (Packet.Opcode == 131 || Packet.Opcode == 132 || Packet.Opcode == 3 || Packet.Opcode == 4)
                {
                    //CLogger.Print(ComDiv.ToHexData($"UDP LOG Opcode: [{Packet.Opcode}]", DecryptedData), LoggerType.Opcode);
                }
            }
            ReadPacket(Packet);
        }
        public void ReadPacket(PacketModel Packet)
        {
            byte[] WithEndData = Packet.WithEndData;
            byte[] WithoutEndData = Packet.WithoutEndData;
            SyncClientPacket C = new SyncClientPacket(WithEndData);
            int BasicBufferLength = WithoutEndData.Length;
            try
            {
                uint UniqueRoomId = 0, RoomSeed = 0;
                int DedicationSlot = 0;
                RoomModel Room = null;
                switch (Packet.Opcode)
                {
                    case 65: //REGISTER
                    {
                        string Version = $"{C.ReadH()}.{C.ReadH()}";
                        UniqueRoomId = C.ReadUD();
                        RoomSeed = C.ReadUD();
                        DedicationSlot = C.ReadC();
                        Room = RoomsManager.CreateOrGetRoom(UniqueRoomId, RoomSeed);
                        if (Room != null)
                        {
                            PlayerModel Player = Room.AddPlayer(RemoteAddress, Packet, Version);
                            if (Player != null)
                            {
                                if (!Player.Integrity)
                                {
                                    Player.ResetBattleInfos();
                                }
                                byte[] Code = PROTOCOL_CONNECT.GET_CODE();
                                SendPacket(Code, Player.Client);
                                if (ConfigLoader.IsTestMode)
                                {
                                    CLogger.Print($"Player Connected. [{Player.Client.Address}:{Player.Client.Port}]", LoggerType.Warning);
                                }
                            }
                        }
                        break;
                    }
                    case 132: //AI BY HOST
                    {
                        C.Advance(BasicBufferLength);
                        UniqueRoomId = C.ReadUD();
                        DedicationSlot = C.ReadC();
                        RoomSeed = C.ReadUD();
                        Room = RoomsManager.GetRoom(UniqueRoomId, RoomSeed);
                        if (Room != null)
                        {
                            PlayerModel Player = Room.GetPlayer(Packet.Slot, RemoteAddress);
                            if (Player != null && Player.AccountIdIsValid(Packet.AccountId))
                            {
                                Room.BotMode = true;
                                byte[] Actions = PROTOCOL_BOTS_ACTION.GET_CODE(WithoutEndData, Room, true);
                                byte[] Code = AllUtils.BaseWriteCode(132, Actions, Packet.Slot, AllUtils.GetDuration(Player.StartTime), Packet.Round);
                                for (int i = 0; i < 16; i++)
                                {
                                    PlayerModel PlayerR = Room.Players[i];
                                    if (PlayerR.Client != null && PlayerR.AccountIdIsValid() && i != Packet.Slot)
                                    {
                                        SendPacket(Code, PlayerR.Client);
                                    }
                                }
                            }
                        }
                        break;
                    }
                    case 131: //AI BY PLAYER
                    {
                        C.Advance(BasicBufferLength);
                        UniqueRoomId = C.ReadUD();
                        DedicationSlot = C.ReadC();
                        RoomSeed = C.ReadUD();
                        Room = RoomsManager.GetRoom(UniqueRoomId, RoomSeed);
                        if (Room != null)
                        {
                            PlayerModel Player = Room.GetPlayer(Packet.Slot, RemoteAddress);
                            if (Player != null && Player.AccountIdIsValid(Packet.AccountId))
                            {
                                Room.BotMode = true;
                                PlayerModel Member = Room.GetPlayer(DedicationSlot, false);
                                byte[] Actions = PROTOCOL_BOTS_ACTION.GET_CODE(WithoutEndData, Room, true);
                                byte[] Code = AllUtils.BaseWriteCode(132, Actions, DedicationSlot, AllUtils.GetDuration(Member.StartTime), Packet.Round);
                                for (int i = 0; i < 16; i++)
                                {
                                    PlayerModel PlayerR = Room.Players[i];
                                    if (PlayerR.Client != null && PlayerR.AccountIdIsValid() && i != Packet.Slot)
                                    {
                                        SendPacket(Code, PlayerR.Client);
                                    }
                                }
                            }
                        }
                        break;
                    }
                    case 97: //PING | LATENCY
                    {
                        UniqueRoomId = C.ReadUD();
                        DedicationSlot = C.ReadC();
                        RoomSeed = C.ReadUD();
                        Room = RoomsManager.GetRoom(UniqueRoomId, RoomSeed);
                        byte[] Data = Packet.Data;
                        if (Room != null)
                        {
                            PlayerModel Player = Room.GetPlayer(Packet.Slot, RemoteAddress);
                            if (Player != null)
                            {
                                if (ConfigLoader.SendPingSync)
                                {
                                    Player.Latency = AllUtils.PingTime($"{RemoteAddress.Address}", Data, MainSocket.Ttl, 120, false, out int PlayerPing);
                                    Player.Ping = PlayerPing;
                                    SendMatchInfo.SendPingSync(Room, Player);
                                }
                                Player.LastPing = Packet.ReceiveDate;
                                SendPacket(Data, RemoteAddress);
                            }
                        }
                        break;
                    }
                    case 4: //AI | BOT
                    {
                        C.Advance(BasicBufferLength);
                        UniqueRoomId = C.ReadUD();
                        DedicationSlot = C.ReadC();
                        RoomSeed = C.ReadUD();
                        Room = RoomsManager.GetRoom(UniqueRoomId, RoomSeed);
                        if (Room != null)
                        {
                            PlayerModel Player = Room.GetPlayer(Packet.Slot, RemoteAddress);
                            if (Player != null && Player.AccountIdIsValid(Packet.AccountId))
                            {
                                Player.RespawnByUser = Packet.Respawn;
                                Room.BotMode = true;
                                if (Room.StartTime == new DateTime())
                                {
                                    return;
                                }
                                byte[] Actions = PROTOCOL_BOTS_ACTION.GET_CODE(WithoutEndData, Room, false);
                                byte[] Code = AllUtils.BaseWriteCode(4, Actions, Packet.Slot, AllUtils.GetDuration(Player.StartTime), Packet.Round);
                                for (int i = 0; i < 16; i++)
                                {
                                    PlayerModel PlayerR = Room.Players[i];
                                    if (PlayerR.Client != null && PlayerR.AccountIdIsValid() && i != Packet.Slot)
                                    {
                                        SendPacket(Code, PlayerR.Client);
                                    }
                                }
                            }
                        }
                        break;
                    }
                    case 3: //PVP | PLAYER
                    {
                        C.Advance(BasicBufferLength);
                        UniqueRoomId = C.ReadUD();
                        DedicationSlot = C.ReadC();
                        RoomSeed = C.ReadUD();
                        Room = RoomsManager.GetRoom(UniqueRoomId, RoomSeed);
                        if (Room != null)
                        {
                            PlayerModel Player = Room.GetPlayer(Packet.Slot, RemoteAddress);
                            if (Player != null && Player.AccountIdIsValid(Packet.AccountId))
                            {
                                Player.RespawnByUser = Packet.Respawn;
                                if (Room.StartTime == new DateTime())
                                {
                                    return;
                                }
                                if (Room.BotMode)
                                {
                                    byte[] Actions = PROTOCOL_BOTS_ACTION.GET_CODE(WithoutEndData, Room, false);
                                    byte[] Code = AllUtils.BaseWriteCode(4, Actions, Packet.Slot, AllUtils.GetDuration(Room.StartTime), Packet.Round);
                                    for (int i = 0; i < 16; i++)
                                    {
                                        PlayerModel PlayerR = Room.Players[i];
                                        if (PlayerR.Client != null && PlayerR.AccountIdIsValid() && i != Packet.Slot)
                                        {
                                            SendPacket(Code, PlayerR.Client);
                                        }
                                    }
                                }
                                else
                                {
                                    if (DedicationSlot == 255)
                                    {
                                        CLogger.Print($"Battle Opcode: {Packet.Opcode}; DedicationSlot invalid in PVP! DedicationSlot: {DedicationSlot}", LoggerType.Warning);
                                    }
                                    byte[] Actions = WritePlayerActionData(WithoutEndData, Room, AllUtils.GetDuration(Player.StartTime), Packet);
                                    byte[] Code = AllUtils.BaseWriteCode(4, Actions, 255, AllUtils.GetDuration(Room.StartTime), Packet.Round);
                                    for (int i = 0; i < 16; i++)
                                    {
                                        PlayerModel PlayerR = Room.Players[i];
                                        if (PlayerR.Client != null && PlayerR.AccountIdIsValid())
                                        {
                                            SendPacket(Code, PlayerR.Client);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    }
                    case 67: //UNREGISTER
                    {
                        string Version = $"{C.ReadH()}.{C.ReadH()}";
                        UniqueRoomId = C.ReadUD();
                        RoomSeed = C.ReadUD();
                        DedicationSlot = C.ReadC();
                        Room = RoomsManager.GetRoom(UniqueRoomId, RoomSeed);
                        if (Room != null)
                        {
                            if (Room.RemovePlayer(RemoteAddress, Packet, Version))
                            {
                                if (ConfigLoader.IsTestMode)
                                {
                                    CLogger.Print($"Player Disconnected. [{RemoteAddress.Address}:{RemoteAddress.Port}]", LoggerType.Warning);
                                }
                            }
                            if (Room.GetPlayersCount() == 0)
                            {
                                RoomsManager.RemoveAssists(Room.UniqueRoomId, Room.RoomSeed);
                                RoomsManager.RemoveRoom(Room.UniqueRoomId, Room.RoomSeed);
                            }
                        }
                        break;
                    }
                    default:
                    {
                        CLogger.Print(Bitwise.ToHexData($"Opcode Not Found: [{Packet.Opcode}]", WithEndData), LoggerType.Opcode);
                        break;
                    }
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        public List<ObjectHitInfo> GetPlayerActionData(ActionModel Action, RoomModel Room, float Time, out byte[] EventsData)
        {
            EventsData = new byte[0];
            if (Room == null)
            {
                return null;
            }
            if (Action.Data.Length == 0)
            {
                return new List<ObjectHitInfo>();
            }
            byte[] Data = Action.Data;
            List<ObjectHitInfo> Objs = new List<ObjectHitInfo>();
            SyncClientPacket C = new SyncClientPacket(Data);
            using (SyncServerPacket S = new SyncServerPacket())
            {
                uint Event = 0;
                PlayerModel Player = Room.GetPlayer(Action.Slot, false);                
                if (Action.Flag.HasFlag(UdpGameEvent.ActionState))
                {
                    Event += (uint)UdpGameEvent.ActionState;
                    ActionStateInfo Info = ActionState.ReadInfo(Action, C, false);
                    if (!Room.BotMode)
                    {
                        if (Player != null && Player.Equip != null)
                        {
                            int WeaponId = 0;
                            byte Extensions = 0;
                            if (Info.Action.HasFlag(ActionFlag.WeaponSync))
                            {
                                if (Info.Flag.HasFlag(WeaponSyncType.Primary))
                                {
                                    WeaponId = Player.Equip.WpnPrimary;
                                }
                                if (Info.Flag.HasFlag(WeaponSyncType.Secondary))
                                {
                                    WeaponId = Player.Equip.WpnSecondary;
                                }
                                if (Info.Flag.HasFlag(WeaponSyncType.Melee))
                                {
                                    WeaponId = Player.Equip.WpnMelee;
                                }
                                if (Info.Flag.HasFlag(WeaponSyncType.Explosive))
                                {
                                    WeaponId = Player.Equip.WpnExplosive;
                                }
                                if (Info.Flag.HasFlag(WeaponSyncType.Special))
                                {
                                    WeaponId = Player.Equip.WpnSpecial;
                                }
                                if (Info.Flag.HasFlag(WeaponSyncType.Mission))
                                {
                                    if (Room.RoomType == RoomCondition.Bomb)
                                    {
                                        WeaponId = 5009001;
                                    }
                                }
                                if (Info.Flag.HasFlag(WeaponSyncType.Dual))
                                {
                                    Extensions = 16;
                                    WeaponId = Player.Equip.WpnPrimary;
                                }
                            }
                            ObjectHitInfo Obj = new ObjectHitInfo(5)
                            {
                                ObjId = Player.Slot,
                                WeaponId = WeaponId,
                                Extensions = Extensions
                            };
                            Objs.Add(Obj);
                        }
                    }
                    ActionState.WriteInfo(S, Info);
                    Info = null;
                }
                if (Action.Flag.HasFlag(UdpGameEvent.Animation))
                {
                    Event += (uint)UdpGameEvent.Animation;
                    AnimationInfo Info = Animation.ReadInfo(Action, C, false);
                    Animation.WriteInfo(S, Info);
                    Info = null;
                }
                if (Action.Flag.HasFlag(UdpGameEvent.PosRotation))
                {
                    Event += (uint)UdpGameEvent.PosRotation;
                    PosRotationInfo Info = PosRotation.ReadInfo(Action, C, false);
                    if (Player != null)
                    {
                        Player.Position = new Half3(Info.RotationX, Info.RotationY, Info.RotationZ);
                    }
                    Action.Flag += (uint)UdpGameEvent.SoundPosRotation;
                    PosRotation.WriteInfo(S, Info);
                    Info = null;
                }
                if (Action.Flag.HasFlag(UdpGameEvent.SoundPosRotation))
                {
                    Event += (uint)UdpGameEvent.SoundPosRotation;
                    SoundPosRotationInfo Info = SoundPosRotation.ReadInfo(Action, C, Time, false);
                    SoundPosRotation.WriteInfo(S, Info);
                    Info = null;
                }
                if (Action.Flag.HasFlag(UdpGameEvent.UseObject))
                {
                    Event += (uint)UdpGameEvent.UseObject;
                    List<UseObjectInfo> Infos = UseObject.ReadInfo(Action, C, false);
                    for (int i = 0; i < Infos.Count; i++)
                    {
                        UseObjectInfo Info = Infos[i];
                        if (!Room.BotMode && Info.ObjectId != 65535)
                        {
                            ObjectInfo Obj = Room.GetObject(Info.ObjectId);
                            if (Obj != null)
                            {
                                bool SecurityBlock = false;
                                if (Info.SpaceFlags.HasFlag(CharaMoves.HeliInMove))
                                {
                                    DateTime Date = Obj.UseDate;
                                    if (Date.ToString("yyMMddHHmm") == "0101010000")
                                    {
                                        SecurityBlock = true;
                                    }
                                }
                                if (Info.SpaceFlags.HasFlag(CharaMoves.HeliUnknown))
                                {
                                }
                                if (Info.SpaceFlags.HasFlag(CharaMoves.HeliLeave))
                                {
                                }
                                if (Info.SpaceFlags.HasFlag(CharaMoves.HeliStopped))
                                {
                                    AnimModel Anim = Obj.Animation;
                                    if (Anim != null && Anim.Id == 0)
                                    {
                                        Obj.Model.GetAnim(Anim.NextAnim, 0, 0, Obj);
                                    }
                                }
                                if (!SecurityBlock)
                                {
                                    ObjectHitInfo ObjH = new ObjectHitInfo(3)
                                    {
                                        ObjSyncId = 1,
                                        ObjId = Obj.Id,
                                        ObjLife = Obj.Life,
                                        AnimId1 = 255,
                                        AnimId2 = Obj.Animation != null ? Obj.Animation.Id : 255,
                                        SpecialUse = AllUtils.GetDuration(Obj.UseDate)
                                    };
                                    Objs.Add(ObjH);
                                }
                            } 
                        }
                        else
                        {
                            AllUtils.RemoveHit(Infos, i--);
                        }
                    }
                    UseObject.WriteInfo(S, Infos);
                    Infos = null;
                }
                if (Action.Flag.HasFlag(UdpGameEvent.ActionForObjectSync))
                {
                    Event += (uint)UdpGameEvent.ActionForObjectSync;
                    ActionObjectInfo Info = ActionForObjectSync.ReadInfo(Action, C, false);
                    ActionForObjectSync.WriteInfo(S, Info);
                    Info = null;
                }
                if (Action.Flag.HasFlag(UdpGameEvent.RadioChat))
                {
                    Event += (uint)UdpGameEvent.RadioChat;
                    RadioChatInfo Info = RadioChat.ReadInfo(Action, C, false);
                    RadioChat.WriteInfo(S, Info);
                    Info = null;
                }
                if (Action.Flag.HasFlag(UdpGameEvent.WeaponSync))
                {
                    Event += (uint)UdpGameEvent.WeaponSync;
                    WeaponSyncInfo Info = WeaponSync.ReadInfo(Action, C, false);
                    if (Player != null)
                    {
                        Player.Extensions = Info.Extensions;
                        Player.WeaponId = Info.WeaponId;
                        Player.WeaponClass = Info.WeaponClass;
                        Room.SyncInfo(Objs, 2);
                    }
                    WeaponSync.WriteInfo(S, Info);
                    Info = null;
                }
                if (Action.Flag.HasFlag(UdpGameEvent.WeaponRecoil))
                {
                    Event += (uint)UdpGameEvent.WeaponRecoil;
                    WeaponRecoilInfo Info = WeaponRecoil.ReadInfo(Action, C, false);
                    WeaponRecoil.WriteInfo(S, Info);
                    Info = null;
                }
                if (Action.Flag.HasFlag(UdpGameEvent.HpSync))
                {
                    Event += (uint)UdpGameEvent.HpSync;
                    HPSyncInfo Info = HpSync.ReadInfo(Action, C, false);
                    HpSync.WriteInfo(S, Info);
                    Info = null;
                }
                if (Action.Flag.HasFlag(UdpGameEvent.Suicide))
                {
                    Event += (uint)UdpGameEvent.Suicide;
                    List<SuicideInfo> Hits = Suicide.ReadInfo(Action, C, false);
                    int BasicInfo = 0;
                    if (Player != null)
                    {
                        List<DeathServerData> Deaths = new List<DeathServerData>();
                        int ObjIdx = -1;
                        for (int i = 0; i < Hits.Count; i++)
                        {
                            SuicideInfo Hit = Hits[i];
                            if (Player != null && !Player.Dead && Player.Life > 0)
                            {
                                int Damage = AllUtils.GetHitDamageBot(Hit.HitInfo), KillerId = Hit.ObjectId, Type = AllUtils.GetObjectType(Hit.HitInfo);
                                CharaHitPart HitPart = (CharaHitPart)(Hit.HitInfo >> 4 & 63);
                                CharaDeath DeathType = AllUtils.GetCharaDeath(Hit.HitInfo);
                                if (Type == 1)
                                {
                                    ObjIdx = KillerId;
                                }
                                BasicInfo = Hit.WeaponId;
                                Player.Life -= Damage;
                                AllUtils.BaseAssistLogic(Room, Player, Player, Damage);
                                if (Player.Life <= 0)
                                {
                                    DamageManager.SetDeath(Deaths, Player, Player, DeathType);
                                }
                                else
                                {
                                    DamageManager.SetHitEffect(Objs, Player, DeathType, HitPart);
                                }
                                ObjectHitInfo Obj = new ObjectHitInfo(2)
                                {
                                    ObjId = Player.Slot,
                                    ObjLife = Player.Life,
                                    DeathType = DeathType,
                                    HitPart = HitPart,
                                    WeaponId = BasicInfo,
                                    Position = Hit.PlayerPos,
                                };
                                Objs.Add(Obj);
                            }
                            else
                            {
                                AllUtils.RemoveHit(Hits, i--);
                            }
                        }
                        if (Deaths.Count > 0)
                        {
                            SendMatchInfo.SendDeathSync(Room, Player, ObjIdx, BasicInfo, Deaths);
                        }
                        Deaths = null;
                    }
                    else
                    {
                        Hits = new List<SuicideInfo>();
                    }
                    Suicide.WriteInfo(S, Hits);
                    Hits = null;
                }
                if (Action.Flag.HasFlag(UdpGameEvent.MissionData))
                {
                    Event += (uint)UdpGameEvent.MissionData;
                    MissionDataInfo Info = MissionData.ReadInfo(Action, C, Time, false);
                    if (Room.Map != null && Player != null && !Player.Dead && Info.PlantTime > 0 && !Info.BombEnum.HasFlag(BombFlag.Stop))
                    {
                        BombPosition Bomb = Room.Map.GetBomb(Info.BombId);
                        if (Bomb != null)
                        {
                            bool IsDefuse = Info.BombEnum.HasFlag(BombFlag.Defuse);
                            Vector3 BombVec3d = IsDefuse ? Room.BombPosition : Info.BombEnum.HasFlag(BombFlag.Start) ? Bomb.Position : new Half3(0, 0, 0);
                            double PlayerDistance = Vector3.DistanceBomb(Player.Position, BombVec3d);
                            if ((Bomb.EveryWhere || PlayerDistance <= 2.0) && (Player.Team == 1 && IsDefuse || Player.Team == 0 && !IsDefuse))
                            {
                                if (Player.C4Time != Info.PlantTime)
                                {
                                    Player.C4First = DateTimeUtil.Now();
                                    Player.C4Time = Info.PlantTime;
                                }
                                double Seconds = ComDiv.GetDuration(Player.C4First);
                                float Objective = IsDefuse ? Player.DefuseDuration : Player.PlantDuration;
                                if ((Time >= Info.PlantTime + Objective || Seconds >= Objective) && (!Room.HasC4 && Info.BombEnum.HasFlag(BombFlag.Start) || Room.HasC4 && IsDefuse))
                                {
                                    Room.HasC4 = !Room.HasC4;
                                    Info.Bomb |= 2;
                                    SendMatchInfo.SendBombSync(Room, Player, Info.BombEnum.HasFlag(BombFlag.Defuse) ? 1 : 0, Info.BombId);
                                }
                            }
                        }
                    }
                    MissionData.WriteInfo(S, Info);
                    Info = null;
                }
                if (Action.Flag.HasFlag(UdpGameEvent.DropWeapon))
                {
                    Event += (uint)UdpGameEvent.DropWeapon;
                    DropWeaponInfo DropWpn = DropWeapon.ReadInfo(Action, C, false);
                    if (!Room.BotMode)
                    {
                        Room.DropCounter++;
                        if (Room.DropCounter > ConfigLoader.MaxDropWpnCount)
                        {
                            Room.DropCounter = 0;
                        }
                        if (Player != null && Player.Equip != null)
                        {
                            int Static = ComDiv.GetIdStatics(DropWpn.WeaponId, 1);
                            if (Static == 1)
                            {
                                Player.Equip.WpnPrimary = 0;
                            }
                            if (Static == 2)
                            {
                                Player.Equip.WpnSecondary = 0;
                            }
                        }
                    }
                    DropWeapon.WriteInfo(S, DropWpn, Room.DropCounter);
                    DropWpn = null;
                }
                if (Action.Flag.HasFlag(UdpGameEvent.GetWeaponForClient))
                {
                    Event += (uint)UdpGameEvent.GetWeaponForClient;
                    WeaponClient GetWpn = GetWeaponForClient.ReadInfo(Action, C, false);
                    if (!Room.BotMode)
                    {
                        if (Player != null && Player.Equip != null)
                        {
                            int Static = ComDiv.GetIdStatics(GetWpn.WeaponId, 1);
                            if (Static == 1)
                            {
                                Player.Equip.WpnPrimary = GetWpn.WeaponId;
                            }
                            if (Static == 2)
                            {
                                Player.Equip.WpnSecondary = GetWpn.WeaponId;
                            }
                        }
                    }
                    GetWeaponForClient.WriteInfo(S, GetWpn);
                    GetWpn = null;
                }
                if (Action.Flag.HasFlag(UdpGameEvent.FireData))
                {
                    Event += (uint)UdpGameEvent.FireData;
                    FireDataInfo Info = FireData.ReadInfo(Action, C, false);
                    FireData.WriteInfo(S, Info);
                    Info = null;
                }
                if (Action.Flag.HasFlag(UdpGameEvent.CharaFireNHitData))
                {
                    Event += (uint)UdpGameEvent.CharaFireNHitData;
                    CharaFireNHitDataInfo Info = CharaFireNHitData.ReadInfo(Action, C, false);
                    CharaFireNHitData.WriteInfo(S, Info);
                    Info = null;
                }
                if (Action.Flag.HasFlag(UdpGameEvent.HitData))
                {
                    Event += (uint)UdpGameEvent.HitData;
                    List<HitDataInfo> Hits = HitData.ReadInfo(Action, C, false);
                    List<DeathServerData> Deaths = new List<DeathServerData>();
                    int BasicInfo = 0;
                    if (Player != null)
                    {
                        for (int i = 0; i < Hits.Count; i++)
                        {
                            HitDataInfo Hit = Hits[i];
                            if (Hit.HitEnum == HitType.HelmetProtection || Hit.HitEnum == HitType.HeadshotProtection)
                            {
                                continue;
                            }
                            int RawDamage = AllUtils.GetHitDamageNormal(Hit.HitIndex);
                            if (AllUtils.ValidateHitData(RawDamage, Hit, out int Damage))
                            {
                                int ObjId = Hit.ObjectId;
                                CharaHitPart HitPart = AllUtils.GetHitPart(Hit.HitIndex);
                                CharaDeath DeathType = CharaDeath.DEFAULT;
                                BasicInfo = Hit.WeaponId;
                                ObjectType ObjHitType = AllUtils.GetHitType(Hit.HitIndex);
                                if (ObjHitType == ObjectType.Object)
                                {
                                    ObjectInfo Obj = Room.GetObject(ObjId);
                                    ObjectModel ObjM = Obj?.Model;
                                    if (ObjM != null && ObjM.Destroyable)
                                    {
                                        if (Obj.Life > 0)
                                        {
                                            Obj.Life -= Damage;
                                            if (Obj.Life <= 0)
                                            {
                                                Obj.Life = 0;
                                                DamageManager.BoomDeath(Room, Player, BasicInfo, Deaths, Objs, Hit.BoomPlayers);
                                            }
                                            Obj.DestroyState = ObjM.CheckDestroyState(Obj.Life);
                                            DamageManager.SabotageDestroy(Room, Player, ObjM, Obj, Damage);
                                            float SyncingTime = AllUtils.GetDuration(Obj.UseDate);
                                            if (Obj.Animation != null && Obj.Animation.Duration > 0 && SyncingTime >= Obj.Animation.Duration)
                                            {
                                                Obj.Model.GetAnim(Obj.Animation.NextAnim, SyncingTime, Obj.Animation.Duration, Obj);
                                            }
                                            ObjectHitInfo ObjH = new ObjectHitInfo(ObjM.UpdateId)
                                            {
                                                ObjId = Obj.Id,
                                                ObjLife = Obj.Life,
                                                KillerId = Action.Slot,
                                                ObjSyncId = ObjM.NeedSync ? 1 : 0,
                                                SpecialUse = SyncingTime,
                                                AnimId1 = ObjM.Animation,
                                                AnimId2 = Obj.Animation != null ? Obj.Animation.Id : 255,
                                                DestroyState = Obj.DestroyState
                                            };
                                            Objs.Add(ObjH);
                                        }
                                    }
                                    else if (ConfigLoader.SendFailMsg && ObjM == null)
                                    {
                                        CLogger.Print($"Fire Obj: {ObjId} Map: {Room.MapId} Invalid Object.", LoggerType.Warning);
                                        Player.LogPlayerPos(Hit.EndBullet);
                                    }
                                }
                                else if (ObjHitType == ObjectType.User)
                                {
                                    if (Room.GetPlayer(ObjId, out PlayerModel User) && Player.RespawnIsValid() && !Player.Dead && !User.Dead && !User.Immortal)
                                    {
                                        if (HitPart == CharaHitPart.HEAD)
                                        {
                                            DeathType = CharaDeath.HEADSHOT;
                                        }
                                        if (Room.RoomType == RoomCondition.DeathMatch && Room.Rule == MapRules.HeadHunter && DeathType != CharaDeath.HEADSHOT)
                                        {
                                            Damage = 1;
                                        }
                                        else if (Room.RoomType == RoomCondition.Boss && DeathType == CharaDeath.HEADSHOT)
                                        {
                                            if (Room.LastRound == 1 && User.Team == 0 || Room.LastRound == 2 && User.Team == 1)
                                            {
                                                Damage /= 10;
                                            }
                                        }
                                        else if (Room.RoomType == RoomCondition.DeathMatch && Room.Rule == MapRules.Chaos)
                                        {
                                            Damage = 200;
                                        }
                                        if (ConfigLoader.UseHitMarker)
                                        {
                                            SendMatchInfo.SendHitMarkerSync(Room, Player, DeathType, Hit.HitEnum, Damage);
                                        }
                                        DamageManager.SimpleDeath(Room, Deaths, Objs, Player, User, Damage, BasicInfo, HitPart, DeathType);
                                    }
                                    else
                                    {
                                        AllUtils.RemoveHit(Hits, i--);
                                    }
                                }
                                else if (ObjHitType == ObjectType.UserObject)
                                {
                                    int OwnerSlot = ObjId >> 4;
                                    int GrenadeMapId = ObjId & 15;
                                }
                                else
                                {
                                    CLogger.Print($"HitType: ({ObjHitType}/{(int)ObjHitType}) Slot: {Action.Slot}", LoggerType.Warning);
                                    CLogger.Print($"BoomPlayers: {Hit.BoomInfo} {Hit.BoomPlayers.Count}", LoggerType.Warning);
                                }
                            }
                            else
                            {
                                AllUtils.RemoveHit(Hits, i--);
                            }
                        }
                        if (Deaths.Count > 0)
                        {
                            SendMatchInfo.SendDeathSync(Room, Player, 255, BasicInfo, Deaths);
                        }
                    }
                    else
                    {
                        Hits = new List<HitDataInfo>();
                    }
                    HitData.WriteInfo(S, Hits);
                    Deaths = null;
                    Hits = null;
                }
                if (Action.Flag.HasFlag(UdpGameEvent.GrenadeHit))
                {
                    Event += (uint)UdpGameEvent.GrenadeHit;
                    List<GrenadeHitInfo> Hits = GrenadeHit.ReadInfo(Action, C, false);
                    List<DeathServerData> Deaths = new List<DeathServerData>();
                    int BasicInfo = 0;
                    if (Player != null)
                    {
                        int PlayerSlot = -1;
                        for (int i = 0; i < Hits.Count; i++)
                        {
                            GrenadeHitInfo Hit = Hits[i];
                            int RawDamage = AllUtils.GetHitDamageNormal(Hit.HitInfo);
                            if (AllUtils.ValidateGrenadeHit(RawDamage, Hit, out int Damage))
                            {
                                int ObjId = Hit.ObjectId;
                                CharaHitPart HitPart = AllUtils.GetHitPart(Hit.HitInfo);
                                BasicInfo = Hit.WeaponId;
                                ObjectType HitType = AllUtils.GetHitType(Hit.HitInfo);
                                if (HitType == ObjectType.Object)
                                {
                                    ObjectInfo Obj = Room.GetObject(ObjId);
                                    ObjectModel ObjM = Obj?.Model;
                                    if (ObjM != null && ObjM.Destroyable && Obj.Life > 0)
                                    {
                                        Obj.Life -= Damage;
                                        if (Obj.Life <= 0)
                                        {
                                            Obj.Life = 0;
                                            DamageManager.BoomDeath(Room, Player, BasicInfo, Deaths, Objs, Hit.BoomPlayers);
                                        }
                                        Obj.DestroyState = ObjM.CheckDestroyState(Obj.Life);
                                        DamageManager.SabotageDestroy(Room, Player, ObjM, Obj, Damage);
                                        if (Damage > 0)
                                        {
                                            ObjectHitInfo ObjH = new ObjectHitInfo(ObjM.UpdateId)
                                            {
                                                ObjId = Obj.Id,
                                                ObjLife = Obj.Life,
                                                KillerId = Action.Slot,
                                                ObjSyncId = ObjM.NeedSync ? 1 : 0,
                                                AnimId1 = ObjM.Animation,
                                                AnimId2 = Obj.Animation != null ? Obj.Animation.Id : 255,
                                                DestroyState = Obj.DestroyState,
                                                SpecialUse = AllUtils.GetDuration(Obj.UseDate)
                                            };
                                            Objs.Add(ObjH);
                                        }
                                    }
                                    else if (ConfigLoader.SendFailMsg && ObjM == null)
                                    {
                                        CLogger.Print($"Boom Obj: {ObjId} Map: {Room.MapId} Invalid Object.", LoggerType.Warning);
                                        Player.LogPlayerPos(Hit.HitPos);
                                    }
                                }
                                else if (HitType == ObjectType.User)
                                {
                                    PlayerSlot++;
                                    if (Damage > 0 && Room.GetPlayer(ObjId, out PlayerModel User) && Player.RespawnIsValid() && !User.Dead && !User.Immortal)
                                    {
                                        if (Hit.DeathType == CharaDeath.MEDICAL_KIT)
                                        {
                                            User.Life += Damage;
                                            User.CheckLifeValue();
                                        }
                                        else if (Hit.DeathType == CharaDeath.BOOM && ClassType.Dino != Hit.WeaponClass && PlayerSlot % 2 == 0)
                                        {
                                            int Value = Damage;
                                            Damage = (int)Math.Ceiling(Damage / 2.7); // + 14;
                                            User.Life -= Damage;
                                            AllUtils.BaseAssistLogic(Room, Player, User, Damage);
                                            if (User.Life <= 0)
                                            {
                                                DamageManager.SetDeath(Deaths, User, Player, Hit.DeathType);
                                            }
                                            else
                                            {
                                                DamageManager.SetHitEffect(Objs, User, Player, Hit.DeathType, HitPart);
                                            }
                                        }
                                        else
                                        {
                                            User.Life -= Damage;
                                            AllUtils.BaseAssistLogic(Room, Player, User, Damage);
                                            if (User.Life <= 0)
                                            {
                                                DamageManager.SetDeath(Deaths, User, Player, Hit.DeathType);
                                            }
                                            else
                                            {
                                                DamageManager.SetHitEffect(Objs, User, Player, Hit.DeathType, HitPart);
                                            }
                                        }
                                        if (Damage > 0)
                                        {
                                            if (ConfigLoader.UseHitMarker)
                                            {
                                                SendMatchInfo.SendHitMarkerSync(Room, Player, Hit.DeathType, Hit.HitEnum, Damage);
                                            }
                                            ObjectHitInfo Obj = new ObjectHitInfo(2)
                                            {
                                                ObjId = User.Slot,
                                                ObjLife = User.Life,
                                                DeathType = Hit.DeathType,
                                                WeaponId = BasicInfo,
                                                HitPart = HitPart,
                                            };
                                            Objs.Add(Obj);
                                        }
                                    }
                                    else
                                    {
                                        AllUtils.RemoveHit(Hits, i--);
                                    }
                                }
                                else if (HitType == ObjectType.UserObject)
                                {
                                    int OwnerSlot = ObjId >> 4;
                                    int GrenadeMapId = ObjId & 15;
                                }
                                else
                                {
                                    CLogger.Print($"Grenade Boom, HitType: ({HitType}/{(int)HitType})", LoggerType.Warning);
                                }
                            }
                            else
                            {
                                AllUtils.RemoveHit(Hits, i--);
                            }
                        }
                        if (Deaths.Count > 0)
                        {
                            SendMatchInfo.SendDeathSync(Room, Player, 255, BasicInfo, Deaths);
                        }
                    }
                    else
                    {
                        Hits = new List<GrenadeHitInfo>();
                    }
                    GrenadeHit.WriteInfo(S, Hits);
                    Deaths = null;
                    Hits = null;
                }
                if (Action.Flag.HasFlag(UdpGameEvent.GetWeaponForHost))
                {
                    Event += (uint)UdpGameEvent.GetWeaponForHost;
                    WeaponHost Info = GetWeaponForHost.ReadInfo(Action, C, false);
                    GetWeaponForHost.WriteInfo(S, Info);
                    Info = null;
                }
                if (Action.Flag.HasFlag(UdpGameEvent.FireDataOnObject))
                {
                    Event += (uint)UdpGameEvent.FireDataOnObject;
                    FireDataObjectInfo Info = FireDataOnObject.ReadInfo(Action, C, false);
                    FireDataOnObject.WriteInfo(S, Info);
                    Info = null;
                }
                if (Action.Flag.HasFlag(UdpGameEvent.FireNHitDataOnObject))
                {
                    Event += (uint)UdpGameEvent.FireNHitDataOnObject;
                    FireNHitDataObjectInfo Info = FireNHitDataOnObject.ReadInfo(Action, C, false);
                    if (Player != null && !Player.Dead)
                    {
                        SendMatchInfo.SendPortalPass(Room, Player, Info.Portal);
                    }
                    FireNHitDataOnObject.WriteInfo(S, Info);
                    Info = null;
                }
                EventsData = S.ToArray();
                if (Event != (uint)Action.Flag)
                {
                    CLogger.Print(Bitwise.ToHexData($"UDP EVENT FLAGS: '{(uint)Action.Flag}' | '{((uint)Action.Flag - Event)}'", Data), LoggerType.Opcode);
                }
                return Objs;
            }
        }
        public byte[] WritePlayerActionData(byte[] Data, RoomModel Room, float Time, PacketModel Packet)
        {
            SyncClientPacket C = new SyncClientPacket(Data);
            List<ObjectHitInfo> Objs = new List<ObjectHitInfo>();
            using (SyncServerPacket S = new SyncServerPacket())
            {
                for (int i = 0; i < 16; i++)
                {
                    ActionModel Action = new ActionModel();
                    try
                    {
                        Action.Length = C.ReadUH(out bool Exception);
                        if (Exception)
                        {
                            break;
                        }
                        Action.Slot = C.ReadUH();
                        Action.SubHead = (UdpSubHead)C.ReadC();
                        if (Action.SubHead == (UdpSubHead)byte.MaxValue)
                        {
                            break;
                        }
                        S.WriteH(Action.Length);
                        S.WriteH(Action.Slot);
                        S.WriteC((byte)Action.SubHead);
                        if (Action.SubHead == UdpSubHead.Grenade)
                        {
                            GrenadeInfo Info = GrenadeSync.ReadInfo(C, false);
                            GrenadeSync.WriteInfo(S, Info);
                            Info = null;
                        }
                        else if (Action.SubHead == UdpSubHead.DroppedWeapon)
                        {
                            DropedWeaponInfo Info = DropedWeapon.ReadInfo(C, false);
                            DropedWeapon.WriteInfo(S, Info);
                            Info = null;
                        }
                        else if (Action.SubHead == UdpSubHead.ObjectStatic)
                        {
                            ObjectStaticInfo Info = ObjectStatic.ReadInfo(C, false);
                            ObjectStatic.WriteInfo(S, Info);
                            Info = null;
                        }
                        else if (Action.SubHead == UdpSubHead.ObjectAnim)
                        {
                            ObjectAnimInfo Info = ObjectAnim.ReadInfo(C, false);
                            ObjectAnim.WriteInfo(S, Info);
                            Info = null;
                        }
                        else if (Action.SubHead == UdpSubHead.StageInfoObjectStatic)
                        {
                            StageStaticInfo Info = StageInfoObjStatic.ReadInfo(C, false);
                            StageInfoObjStatic.WriteInfo(S, Info);
                            Info = null;
                        }
                        else if (Action.SubHead == UdpSubHead.StageInfoObjectAnim)
                        {
                            StageAnimInfo Info = StageInfoObjAnim.ReadInfo(C, false);
                            StageInfoObjAnim.WriteInfo(S, Info);
                            Info = null;
                        }
                        else if (Action.SubHead == UdpSubHead.StageInfoObjectControl)
                        {
                            StageControlInfo Info = StageInfoObjControl.ReadInfo(C, false);
                            StageInfoObjControl.WriteInfo(S, Info);
                            Info = null;
                        }
                        else if (Action.SubHead == UdpSubHead.User || Action.SubHead == UdpSubHead.StageInfoChara)
                        {
                            Action.Flag = (UdpGameEvent)C.ReadUD();
                            Action.Data = C.ReadB(Action.Length - 9);
                            AllUtils.CheckDataFlags(Action, Packet);
                            Objs.AddRange(GetPlayerActionData(Action, Room, Time, out byte[] Result));
                            S.GoBack(5);
                            S.WriteH((ushort)(Result.Length + 9));
                            S.WriteH(Action.Slot);
                            S.WriteC((byte)Action.SubHead);
                            S.WriteD((uint)Action.Flag);
                            S.WriteB(Result);
                            if (Action.Data.Length == 0 && Action.Length - 9 != 0)
                            {
                                break;
                            }
                        }
                        else
                        {
                            CLogger.Print(Bitwise.ToHexData($"UDP SUB HEAD: '{Action.SubHead}' or '{(int)Action.SubHead}'", Data), LoggerType.Opcode);
                        }
                    }
                    catch (Exception ex)
                    {
                        CLogger.Print($"UDP WriteActionBytes - Buffer (Length: {Data.Length}): | {ex.Message}", LoggerType.Error, ex);
                        Objs = new List<ObjectHitInfo>();
                        break;
                    }
                }
                if (Objs.Count > 0)
                {
                    S.WriteB(PROTOCOL_EVENTS_ACTION.GET_CODE(Objs));
                }
                Objs = null;
                return S.ToArray();
            }
        }
        private void SendPacket(byte[] Data, IPEndPoint Address)
        {
            MainSocket.SendTo(Data, 0, Data.Length, SocketFlags.None, Address);
        }
    }
}
