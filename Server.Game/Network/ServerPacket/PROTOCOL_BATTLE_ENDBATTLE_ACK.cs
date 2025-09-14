using Plugin.Core.Enums;
using Server.Game.Data.Managers;
using Server.Game.Data.Utils;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_ENDBATTLE_ACK : GameServerPacket
    {
        private readonly RoomModel Room;
        private readonly Account Player;
        private readonly ClanModel Clan;
        private readonly int Winner = 2;
        private readonly ushort PlayersFlag, MissionsFlag;
        private readonly bool IsBotMode;
        private readonly byte[] SlotInfoData;
        public PROTOCOL_BATTLE_ENDBATTLE_ACK(Account Player)
        {
            this.Player = Player;
            if (Player != null)
            {
                Room = Player.Room;
                if (Room.RoomType == RoomCondition.Tutorial)
                {
                    Winner = 0;
                }
                else
                {
                    Winner = (int)AllUtils.GetWinnerTeam(Room);
                }
                Clan = ClanManager.GetClan(Player.ClanId);
                IsBotMode = Room.IsBotMode();
                AllUtils.GetBattleResult(Room, out MissionsFlag, out PlayersFlag, out SlotInfoData);
            }
        }
        public PROTOCOL_BATTLE_ENDBATTLE_ACK(Account Player, int Winner, ushort PlayersFlag, ushort MissionsFlag, bool IsBotMode, byte[] SlotInfoData)
        {
            this.Player = Player;
            this.Winner = Winner;
            this.PlayersFlag = PlayersFlag;
            this.MissionsFlag = MissionsFlag;
            this.IsBotMode = IsBotMode;
            this.SlotInfoData = SlotInfoData;
            if (Player != null)
            {
                Room = Player.Room;
                Clan = ClanManager.GetClan(Player.ClanId);
            }
        }
        public PROTOCOL_BATTLE_ENDBATTLE_ACK(Account Player, TeamEnum Winner, ushort PlayersFlag, ushort MissionsFlag, bool IsBotMode, byte[] SlotInfoData)
        {
            this.Player = Player;
            this.Winner = (int)Winner;
            this.PlayersFlag = PlayersFlag;
            this.MissionsFlag = MissionsFlag;
            this.IsBotMode = IsBotMode;
            this.SlotInfoData = SlotInfoData;
            if (Player != null)
            {
                Room = Player.Room;
                Clan = ClanManager.GetClan(Player.ClanId);
            }
        }
        public override void Write()
        {
            WriteH(4116);
            WriteH(PlayersFlag);
            WriteC((byte)Winner);
            WriteB(SlotInfoData);
            WriteH(MissionsFlag);
            WriteB(SlotObjData(Room, IsBotMode));
            WriteC(0);
            WriteC(0);
            WriteC(0);
            WriteB(new byte[5]);
            WriteC(0);
            WriteD(0);
            WriteD(0);
            WriteD(0);
            WriteD(0);
            WriteC(9);
            WriteC(9);
            WriteC(9);
            WriteC(9);
            WriteC(9);
            WriteH(9);
            WriteB(new byte[35]);
            WriteB(new byte[5] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
            WriteD(0); //ItemId Slot 1
            WriteD(0); //ItemId Slot 2
            WriteD(0); //ItemId Slot 3
            WriteD(0); //ItemId Slot 4
            WriteD(0); //ItemId Slot 5
            WriteB(SlotRankData(Room));
            WriteC((byte)(Player.Nickname.Length * 2));
            WriteU(Player.Nickname, Player.Nickname.Length * 2);
            WriteD(Player.GetRank());
            WriteD(Player.GetRank());
            WriteD(Player.Gold);
            WriteD(Player.Exp);
            WriteD(0);
            WriteD(0);
            WriteD(0);
            WriteC(0);
            WriteH((ushort)(Player.HavePermission("observer_enabled") ? 11111 : 0));
            WriteD(Player.Tags);
            WriteD(Player.Cash);
            WriteD(Clan.Id);
            WriteD(Player.ClanAccess);
            WriteQ(Player.StatusId());
            WriteC((byte)Player.CafePC);
            WriteC((byte)Player.TourneyLevel);
            WriteC((byte)(Clan.Name.Length * 2));
            WriteU(Clan.Name, Clan.Name.Length * 2);
            WriteC((byte)Clan.Rank);
            WriteC((byte)Clan.GetClanUnit());
            WriteD(Clan.Logo);
            WriteC((byte)Clan.NameColor);
            WriteC((byte)Clan.Effect);
            WriteD(Player.Statistic.Season.Matches);
            WriteD(Player.Statistic.Season.MatchWins);
            WriteD(Player.Statistic.Season.MatchLoses);
            WriteD(Player.Statistic.Season.MatchDraws);
            WriteD(Player.Statistic.Season.KillsCount);
            WriteD(Player.Statistic.Season.HeadshotsCount);
            WriteD(Player.Statistic.Season.DeathsCount);
            WriteD(Player.Statistic.Season.TotalMatchesCount);
            WriteD(Player.Statistic.Season.TotalKillsCount);
            WriteD(Player.Statistic.Season.EscapesCount);
            WriteD(Player.Statistic.Season.AssistsCount);
            WriteD(Player.Statistic.Season.MvpCount);
            WriteD(Player.Statistic.Basic.Matches);
            WriteD(Player.Statistic.Basic.MatchWins);
            WriteD(Player.Statistic.Basic.MatchLoses);
            WriteD(Player.Statistic.Basic.MatchDraws);
            WriteD(Player.Statistic.Basic.KillsCount);
            WriteD(Player.Statistic.Basic.HeadshotsCount);
            WriteD(Player.Statistic.Basic.DeathsCount);
            WriteD(Player.Statistic.Basic.TotalMatchesCount);
            WriteD(Player.Statistic.Basic.TotalKillsCount);
            WriteD(Player.Statistic.Basic.EscapesCount);
            WriteD(Player.Statistic.Basic.AssistsCount);
            WriteD(Player.Statistic.Basic.MvpCount);
            WriteH((ushort)Player.Statistic.Daily.Matches);
            WriteH((ushort)Player.Statistic.Daily.MatchWins);
            WriteH((ushort)Player.Statistic.Daily.MatchLoses);
            WriteH((ushort)Player.Statistic.Daily.MatchDraws);
            WriteH((ushort)Player.Statistic.Daily.KillsCount);
            WriteH((ushort)Player.Statistic.Daily.HeadshotsCount);
            WriteH((ushort)Player.Statistic.Daily.DeathsCount);
            WriteD(Player.Statistic.Daily.ExpGained);
            WriteD(Player.Statistic.Daily.PointGained);
            WriteB(new byte[16]);
            WriteC(0); // Play Time
            WriteD(0); // Time
            WriteD(0); // Time
            WriteC(0);
            WriteB(SeasonPassData(IsBotMode));
        }
        private byte[] SlotObjData(RoomModel Room, bool IsBotMode)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                if (IsBotMode)
                {
                    foreach (SlotModel Slot in Room.Slots)
                    {
                        S.WriteH((ushort)Slot.Score);
                    }
                }
                else if (Room.ThisModeHaveRounds() || Room.IsDinoMode())
                {
                    S.WriteH((ushort)(Room.IsDinoMode("DE") ? Room.FRDino : Room.IsDinoMode("CC") ? Room.FRKills : Room.FRRounds)); //WriteD ?
                    S.WriteH((ushort)(Room.IsDinoMode("DE") ? Room.CTDino : Room.IsDinoMode("CC") ? Room.CTKills : Room.CTRounds)); //WriteD ?
                    foreach (SlotModel Slot in Room.Slots)
                    {
                        S.WriteC((byte)Slot.Objects);
                    }
                    S.WriteH(0); //Change Team Score (FR) ??
                    S.WriteH(0); //Change Team Score (CT) ??
                }
                return S.ToArray();
            }
        }
        private byte[] SlotRankData(RoomModel Room)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                foreach (SlotModel Slot in Room.Slots)
                {
                    bool IsPlayer = Room.GetPlayerBySlot(Slot, out Account Player);
                    WriteC((byte)(IsPlayer ? Player.Rank : 51));
                    WriteH(0);
                    WriteD(0); // Max UINT Value?
                }
                return S.ToArray();
            }
        }
        private byte[] SeasonPassData(bool IsBotMode)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                if (!IsBotMode)
                {
                    S.WriteD(0);
                    S.WriteD(0);
                    S.WriteD(0);
                    S.WriteD(0);
                    S.WriteD(0);
                    S.WriteD(0);
                    S.WriteD(0);
                    S.WriteD(0);
                    S.WriteD(0);
                    S.WriteD(10); //tags earned
                    S.WriteD(0);
                    S.WriteD(0);
                    S.WriteD(0);
                    S.WriteD(0);
                    S.WriteH(100); //season pass points earned
                    S.WriteH(5); //season pass points earned bonus
                    S.WriteC(0);
                    S.WriteD(0);
                    S.WriteD(0);
                    S.WriteD(0);
                    S.WriteD(100); //medals earned
                }
                return S.ToArray();
            }
        }
    }
}