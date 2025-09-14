using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CLAN_WAR_CREATE_ROOM_REQ : GameClientPacket
    {
        private int roomId = -1, SlotCount, Ping, MatchId;
        private string Name;
        private StageOptions MapStage;
        private MapIdEnum MapId;
        private MapRules MapRule;
        private RoomCondition RoomType;
        private RoomWeaponsFlag WeaponFlag;
        private RoomStageFlag RoomFlag;
        public PROTOCOL_CLAN_WAR_CREATE_ROOM_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            MatchId = ReadH();
            ReadD();
            ReadD();
            ReadH();
            Name = ReadU(46);
            MapId = (MapIdEnum)ReadC();
            MapRule = (MapRules)ReadC();
            MapStage = (StageOptions)ReadC();
            RoomType = (RoomCondition)ReadC();
            ReadC();
            ReadC();
            SlotCount = ReadC();
            Ping = ReadC();
            WeaponFlag = (RoomWeaponsFlag)ReadH();
            RoomFlag = (RoomStageFlag)ReadD();
        }
        public override void Run()
        {
            try
            {
                Account Player = Client.Player;
                if (Player == null || Player.ClanId == 0)
                {
                    return;
                }
                ChannelModel Channel = Player.GetChannel();
                MatchModel MyMatch = Player.Match;
                if (Channel == null || MyMatch == null)
                {
                    return;
                }
                MatchModel EnemyMatch = Channel.GetMatch(MatchId);
                if (EnemyMatch == null)
                {
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
                            Room.SetSlotCount(SlotCount, false);
                            Room.Ping = Ping;
                            Room.WeaponsFlag = WeaponFlag;
                            Room.Flag = RoomFlag;
                            Room.Password = "";
                            Room.KillTime = 3;
                            if (Room.AddPlayer(Player) >= 0)
                            {
                                Channel.AddRoom(Room);
                                Client.SendPacket(new PROTOCOL_ROOM_CREATE_ACK(0, Room));
                                roomId = i;
                                return;
                            }
                        }
                    }
                }
                using (PROTOCOL_CLAN_WAR_ENEMY_INFO_ACK Packet = new PROTOCOL_CLAN_WAR_ENEMY_INFO_ACK(EnemyMatch))
                {
                    using (PROTOCOL_CLAN_WAR_JOIN_ROOM_ACK Packet2 = new PROTOCOL_CLAN_WAR_JOIN_ROOM_ACK(EnemyMatch, roomId, 0))
                    {
                        byte[] Data = Packet.GetCompleteBytes("PROTOCOL_CLAN_WAR_CREATE_ROOM_REQ-1");
                        byte[] Data2 = Packet2.GetCompleteBytes("PROTOCOL_CLAN_WAR_CREATE_ROOM_REQ-2");
                        foreach (Account Member in MyMatch.GetAllPlayers(MyMatch.Leader))
                        {
                            Member.SendCompletePacket(Data, Packet.GetType().Name);
                            Member.SendCompletePacket(Data2, Packet2.GetType().Name);
                            if (Member.Match != null)
                            {
                                MyMatch.Slots[Member.MatchSlot].State = SlotMatchState.Ready;
                            }
                        }
                    }
                }
                using (PROTOCOL_CLAN_WAR_ENEMY_INFO_ACK Packet = new PROTOCOL_CLAN_WAR_ENEMY_INFO_ACK(MyMatch))
                {
                    using (PROTOCOL_CLAN_WAR_JOIN_ROOM_ACK Packet2 = new PROTOCOL_CLAN_WAR_JOIN_ROOM_ACK(MyMatch, roomId, 1))
                    {
                        byte[] Data = Packet.GetCompleteBytes("PROTOCOL_CLAN_WAR_CREATE_ROOM_REQ-3");
                        byte[] Data2 = Packet2.GetCompleteBytes("PROTOCOL_CLAN_WAR_CREATE_ROOM_REQ-4");
                        foreach (Account Member in EnemyMatch.GetAllPlayers())
                        {
                            Member.SendCompletePacket(Data, Packet.GetType().Name);
                            Member.SendCompletePacket(Data2, Packet2.GetType().Name);
                            if (Member.Match != null)
                            {
                                MyMatch.Slots[Member.MatchSlot].State = SlotMatchState.Ready;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}