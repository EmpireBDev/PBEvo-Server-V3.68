using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_ROOM_CREATE_REQ : GameClientPacket
    {
        private uint Error;
        private string Name, Password, LeaderName;
        private MapIdEnum MapId;
        private MapRules MapRule;
        private StageOptions MapStage;
        private short BalanceType;
        private byte[] RandomMaps, LeaderAddr;
        private int SlotCount, Ping, KillTime, CountPlayers, NewInt;
        private byte Limit, WatchRuleFlag, AiCount, AiLevel, AiType, KillCam, CountdownIG;
        private RoomCondition RoomType;
        private RoomState State;
        private RoomWeaponsFlag WeaponFlag;
        private RoomStageFlag RoomFlag;
        public PROTOCOL_ROOM_CREATE_REQ(GameClient client, byte[] data)
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
            Password = ReadS(4);
            AiCount = ReadC();
            AiLevel = ReadC();
            AiType = ReadC();
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
                ChannelModel Channel = Player.GetChannel();
                if (Channel == null || Player.Nickname.Length == 0 || Player.Room != null || Player.Match != null)
                {
                    Error = 0x80000000;
                    return;
                }
                lock (Channel.Rooms)
                {
                    for (int i = 0; i < Channel.MaxRooms; i++)
                    {
                        if (Channel.GetRoom(i) == null)
                        {
                            RoomModel Room = new RoomModel(i, Channel)
                            {
                                Name = Name,
                                MapId = MapId,
                                Rule = MapRule,
                                Stage = MapStage,
                                RoomType = RoomType
                            };
                            Room.GenerateSeed();
                            if (Room.RoomType == 0)
                            {
                                break;
                            }
                            Room.State = State < RoomState.Ready ? RoomState.Ready : State;
                            Room.LeaderName = (LeaderName.Equals("") || !LeaderName.Equals(Player.Nickname)) ? Player.Nickname : LeaderName;
                            Room.Ping = Ping;
                            Room.WeaponsFlag = WeaponFlag;
                            Room.Flag = RoomFlag;
                            Room.NewInt = NewInt;
                            bool IsBotMode = Room.IsBotMode();
                            if (IsBotMode && Room.ChannelType == ChannelType.Clan)
                            {
                                Error = 0x8000107D;
                                return;
                            }
                            Room.KillTime = KillTime;
                            Room.Limit = (byte)(Channel.Type == ChannelType.Clan ? 1 : Limit);
                            Room.WatchRuleFlag = (byte)(Room.RoomType == RoomCondition.Ace ? 142 : WatchRuleFlag);
                            Room.BalanceType = (short)((Channel.Type == ChannelType.Clan || Room.RoomType == RoomCondition.Ace) ? 0 : BalanceType);
                            Room.RandomMaps = RandomMaps;
                            Room.CountdownIG = CountdownIG;
                            Room.LeaderAddr = LeaderAddr;
                            Room.KillCam = KillCam;
                            Room.Password = Password;
                            if (IsBotMode)
                            {
                                Room.AiCount = AiCount;
                                Room.AiLevel = AiLevel;
                                Room.AiType = AiType;
                            }
                            Room.SetSlotCount(SlotCount, false);
                            Room.CountPlayers = CountPlayers;
                            Room.CountMaxSlots = SlotCount;
                            if (Room.AddPlayer(Player) >= 0)
                            {
                                Player.ResetPages();
                                Channel.AddRoom(Room);
                                Client.SendPacket(new PROTOCOL_ROOM_CREATE_ACK(Error, Room));
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_LOBBY_CREATE_ROOM_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}