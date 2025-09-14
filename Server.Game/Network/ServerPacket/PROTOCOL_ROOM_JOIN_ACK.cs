using Plugin.Core.Enums;
using Server.Game.Data.Managers;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Plugin.Core.Network;
using System.Net;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_JOIN_ACK : GameServerPacket
    {
        private readonly uint Error;
        private readonly RoomModel Room;
        private readonly int SlotId;
        public PROTOCOL_ROOM_JOIN_ACK(uint Error, Account Player)
        {
            this.Error = Error;
            if (Player != null)
            {
                SlotId = Player.SlotId;
                Room = Player.Room;
            }
        }
        public override void Write()
        {
            WriteH(3844);
            WriteH(0);
            WriteD(Error);
            if (Error == 0)
            {
                WriteB(JoinData(Room));
            }
        }
        private byte[] JoinData(RoomModel Room)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                lock (Room.Slots)
                {
                    S.WriteC((byte)Room.Slots.Length);
                    foreach (SlotModel Slot in Room.Slots)
                    {
                        S.WriteC((byte)(Slot.Id % 2));
                    }
                    S.WriteC((byte)Room.Slots.Length);
                    foreach (SlotModel Slot in Room.Slots)
                    {
                        Account Player = Room.GetPlayerBySlot(Slot);
                        if (Player != null)
                        {
                            ClanModel Clan = ClanManager.GetClan(Player.ClanId);
                            S.WriteC((byte)Slot.State);
                            S.WriteC((byte)Player.GetRank());
                            S.WriteD(Clan.Id);
                            S.WriteD(Player.ClanAccess);
                            S.WriteC((byte)Clan.Rank);
                            S.WriteD(Clan.Logo);
                            S.WriteC((byte)Player.CafePC);
                            S.WriteC((byte)Player.TourneyLevel);
                            S.WriteD((uint)Player.Effects);
                            S.WriteC((byte)Clan.Effect);
                            S.WriteC(0);
                            S.WriteH((ushort)NATIONS);
                            S.WriteD(Player.Equipment.NameCardId);
                            S.WriteH(0);
                            S.WriteU(Clan.Name, 34);
                            S.WriteC((byte)Slot.Id);
                            S.WriteU(Player.Nickname, 66);
                            S.WriteC((byte)Player.NickColor);
                            S.WriteC((byte)Player.Bonus.MuzzleColor);
                            S.WriteC(0);
                            S.WriteC(255);
                            S.WriteC(255);
                        }
                        else
                        {
                            S.WriteC((byte)Slot.State);
                            S.WriteB(new byte[10]);
                            S.WriteD(4294967295);
                            S.WriteB(new byte[50]);
                            S.WriteC((byte)Slot.Id);
                            S.WriteB(new byte[68]);
                            S.WriteC(0);
                            S.WriteC(255);
                            S.WriteC(255);
                        }
                    }
                    S.WriteC(Room.AiType);
                    S.WriteC(Room.State > RoomState.Ready ? Room.IngameAiLevel : Room.AiLevel);
                    S.WriteC(Room.AiCount);
                    S.WriteC((byte)Room.GetAllPlayers().Count);
                    S.WriteC((byte)Room.Leader);
                    S.WriteC((byte)Room.CountdownTime.GetTimeLeft());
                    S.WriteC((byte)Room.Password.Length); //4
                    S.WriteS(Room.Password, Room.Password.Length); //4
                    S.WriteB(new byte[17]);
                    S.WriteU(Room.LeaderName, 66);
                    S.WriteD(Room.KillTime);
                    S.WriteC(Room.Limit);
                    S.WriteC(Room.WatchRuleFlag);
                    S.WriteH(Room.BalanceType);
                    S.WriteB(Room.RandomMaps);
                    S.WriteC(Room.CountdownIG);
                    S.WriteB(Room.LeaderAddr);
                    S.WriteC(Room.KillCam);
                    S.WriteH(0);
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
                    S.WriteB(new byte[4]); //PlayerAddr
                    S.WriteC((byte)SlotId);
                }
                return S.ToArray();
            }
        }
    }
}