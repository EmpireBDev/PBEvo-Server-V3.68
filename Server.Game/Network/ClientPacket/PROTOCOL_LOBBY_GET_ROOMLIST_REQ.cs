using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_LOBBY_GET_ROOMLIST_REQ : GameClientPacket
    {
        public PROTOCOL_LOBBY_GET_ROOMLIST_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
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
                if (Channel != null)
                {
                    Channel.RemoveEmptyRooms();
                    List<RoomModel> Rooms = Channel.Rooms;
                    List<Account> Waiting = Channel.GetWaitPlayers();
                    int RoomPages = (int)Math.Ceiling(Rooms.Count / 10d), PlayerPages = (int)Math.Ceiling(Waiting.Count / 8d);
                    if (Player.LastRoomPage >= RoomPages)
                    {
                        Player.LastRoomPage = 0;
                    }
                    if (Player.LastPlayerPage >= PlayerPages)
                    {
                        Player.LastPlayerPage = 0;
                    }
                    int RoomsCount = 0, PlayersCount = 0;
                    byte[] RoomsArray = GetRoomListData(Player.LastRoomPage, ref RoomsCount, Rooms);
                    byte[] WaitingArray = GetPlayerListData(Player.LastPlayerPage, ref PlayersCount, Waiting);
                    Client.SendPacket(new PROTOCOL_LOBBY_GET_ROOMLIST_ACK(Rooms.Count, Waiting.Count, Player.LastRoomPage++, Player.LastPlayerPage++, RoomsCount, PlayersCount, RoomsArray, WaitingArray));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_LOBBY_GET_ROOMLIST_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
        private byte[] GetRoomListData(int page, ref int count, List<RoomModel> List)
        {
            int Startid = page == 0 ? 10 : 11;
            using (SyncServerPacket S = new SyncServerPacket())
            {
                for (int i = (page * Startid); i < List.Count; i++)
                {
                    WriteRoomData(List[i], S);
                    if (++count == 10)
                    {
                        break;
                    }
                }
                return S.ToArray();
            }
        }
        private void WriteRoomData(RoomModel Room, SyncServerPacket S)
        {
            S.WriteD(Room.RoomId);
            S.WriteU(Room.Name, 46);
            S.WriteC((byte)Room.MapId);
            S.WriteC((byte)Room.Rule);
            S.WriteC((byte)Room.Stage);
            S.WriteC((byte)Room.RoomType);
            S.WriteC((byte)Room.State);
            S.WriteC((byte)Room.GetCountPlayers());
            S.WriteC((byte)Room.GetSlotCount());
            S.WriteC((byte)Room.Ping);
            S.WriteH((ushort)Room.WeaponsFlag);
            S.WriteD(Room.GetFlag());
            S.WriteH(0);
            S.WriteB(Room.LeaderAddr);
        }
        private byte[] GetPlayerListData(int Page, ref int Count, List<Account> List)
        {
            int Startid = Page == 0 ? 8 : 9;
            using (SyncServerPacket S = new SyncServerPacket())
            {
                for (int i = (Page * Startid); i < List.Count; i++)
                {
                    WritePlayerData(List[i], S);
                    if (++Count == 8)
                    {
                        break;
                    }
                }
                return S.ToArray();
            }
        }
        private void WritePlayerData(Account Player, SyncServerPacket S)
        {
            ClanModel Clan = ClanManager.GetClan(Player.ClanId);
            S.WriteD(Player.GetSessionId());
            S.WriteD(Clan.Logo);
            S.WriteC((byte)Clan.Effect);
            S.WriteU(Clan.Name, 34);
            S.WriteH((short)Player.GetRank());
            S.WriteU(Player.Nickname, 66);
            S.WriteC((byte)Player.NickColor);
            S.WriteC((byte)NATIONS);
            S.WriteD(Player.Equipment.NameCardId);
            S.WriteC(0);
        }
    }
}