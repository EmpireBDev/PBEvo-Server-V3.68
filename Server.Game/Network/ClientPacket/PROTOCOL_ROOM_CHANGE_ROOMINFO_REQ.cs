using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_ROOM_CHANGE_ROOMINFO_REQ : GameClientPacket
    {
        private string Name, LeaderName;
        private MapIdEnum MapId;
        private MapRules MapRule;
        private StageOptions MapStage;
        private short BalanceType;
        private byte[] RandomMaps, LeaderAddr;
        private int NewInt, SlotCount, Ping, KillTime, CountPlayers;
        private byte Limit, WatchRuleFlag, AiCount, AiLevel, CountdownIG, KillCam;
        private RoomCondition RoomType;
        private RoomState State;
        private RoomWeaponsFlag WeaponFlag;
        private RoomStageFlag RoomFlag;
        public PROTOCOL_ROOM_CHANGE_ROOMINFO_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            ReadD();
            Name = ReadU(46);
            MapId = (MapIdEnum)ReadC();
            MapRule = (MapRules)ReadC();
            MapStage = (StageOptions)ReadC();
            RoomType = (RoomCondition)ReadC();
            State = (RoomState)ReadC();
            CountPlayers = ReadC();
            SlotCount = ReadC();
            Ping = ReadC();
            WeaponFlag = (RoomWeaponsFlag)ReadH();
            RoomFlag = (RoomStageFlag)ReadD();
            ReadH();
            NewInt = ReadD();
            ReadC();
            LeaderName = ReadU(66);
            KillTime = ReadD();
            Limit = ReadC();
            WatchRuleFlag = ReadC();
            BalanceType = ReadH();
            RandomMaps = ReadB(16);
            CountdownIG = ReadC();
            LeaderAddr = ReadB(4);
            KillCam = ReadC();
            ReadH();
            AiCount = ReadC();
            AiLevel = ReadC();
        }
        public override void Run()
        {
            try
            {
                Account Player = Client.Player;
                if (Player == null)
                {
                    return;
                }
                RoomModel Room = Player.Room;
                if (Room != null && Room.Leader == Player.SlotId)
                {
                    bool IsChangeName = !Room.Name.Equals(Name);
                    bool IsChangeMode = Room.Rule != MapRule || Room.Stage != MapStage || Room.RoomType != RoomType;
                    Room.Name = Name;
                    Room.MapId = MapId;
                    Room.Rule = MapRule;
                    Room.Stage = MapStage;
                    Room.RoomType = RoomType;
                    Room.Ping = Ping;
                    Room.Flag = RoomFlag;
                    Room.NewInt = NewInt;
                    Room.KillTime = KillTime;
                    Room.Limit = Limit;
                    Room.WatchRuleFlag = (byte)(Room.RoomType == RoomCondition.Ace ? 142 : WatchRuleFlag);
                    Room.BalanceType = (short)(Room.RoomType == RoomCondition.Ace ? 0 : BalanceType);
                    Room.BalanceType = BalanceType;
                    Room.RandomMaps = RandomMaps;
                    Room.CountdownIG = CountdownIG;
                    Room.LeaderAddr = LeaderAddr;
                    Room.KillCam = KillCam;
                    Room.AiCount = AiCount;
                    Room.AiLevel = AiLevel;
                    Room.SetSlotCount(SlotCount, true);
                    Room.CountPlayers = CountPlayers;
                    if (State < RoomState.Ready || (LeaderName.Equals("") || !LeaderName.Equals(Player.Nickname)) || IsChangeName || IsChangeMode || WeaponFlag != Room.WeaponsFlag || SlotCount != Room.CountMaxSlots)
                    {
                        Room.State = State < RoomState.Ready ? RoomState.Ready : State;
                        Room.LeaderName = (LeaderName.Equals("") || !LeaderName.Equals(Player.Nickname)) ? Player.Nickname : LeaderName;
                        Room.WeaponsFlag = WeaponFlag;
                        Room.CountMaxSlots = SlotCount;
                        Room.CountdownIG = 0;
                        if (Room.ResetReadyPlayers() > 0)
                        {
                            Room.UpdateSlotsInfo();
                        }
                    }
                    Room.UpdateRoomInfo();
                    using (PROTOCOL_ROOM_CHANGE_ROOM_OPTIONINFO_ACK packet = new PROTOCOL_ROOM_CHANGE_ROOM_OPTIONINFO_ACK(Room))
                    {
                        Room.SendPacketToPlayers(packet);
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BATTLE_CHANGE_ROOMINFO_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
