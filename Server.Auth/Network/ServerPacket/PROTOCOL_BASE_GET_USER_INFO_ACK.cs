using Plugin.Core.Managers.Events;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Plugin.Core.Utility;
using Server.Auth.Data.Managers;
using Server.Auth.Data.Models;
using Server.Auth.Data.Utils;
using System.Collections.Generic;

namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_GET_USER_INFO_ACK : AuthServerPacket
    {
        private readonly Account Player;
        private readonly PlayerStatistic Stat;
        private readonly ClanModel Clan;
        private readonly EventVisitModel EvVisit;
        private readonly List<QuickstartModel> Quickstarts;
        private readonly List<CharacterModel> Characters;
        private readonly uint Error, DateNow;
        //private bool Christmas;
        public PROTOCOL_BASE_GET_USER_INFO_ACK(Account Player)
        {
            this.Player = Player;
            if (Player != null)
            {
                Stat = Player.Statistic;
                DateNow = uint.Parse(Player.LastLoginDate.ToString("yyMMddHHmm"));
                Clan = ClanManager.GetClanDB(Player.ClanId, 1);
                Quickstarts = Player.Quickstart.Quickjoins;
                Characters = Player.Character.Characters;
                EvVisit = EventVisitSync.GetRunningEvent();
            }
            else
            {
                Error = 0x80000000;
            }
            AllUtils.CheckGameEvents(Player, EvVisit, DateNow);
        }
        public override void Write()
        {
            WriteH(525);
            WriteH(0);
            WriteD(Error);
            if (Error == 0)
            {
                WriteB(new byte[105]);
                WriteD(Stat.Battleroyale.Matches);
                WriteD(Stat.GetBRWinRatio());
                WriteD(Stat.Battleroyale.MatchLoses);
                WriteD(Stat.Battleroyale.KillsCount);
                WriteD(Stat.Battleroyale.DeathsCount);
                WriteD(Stat.Battleroyale.HeadshotsCount);
                WriteD(Stat.Battleroyale.AssistsCount);
                WriteD(Stat.Battleroyale.EscapesCount);
                WriteD(Stat.GetBRKDRatio());
                WriteD(Stat.Battleroyale.MatchWins);
                WriteD(Stat.Battleroyale.AverageDamage);
                WriteD(Stat.Battleroyale.PlayTime);
                WriteD(Stat.Acemode.Matches);
                WriteD(Stat.Acemode.MatchWins);
                WriteD(Stat.Acemode.MatchLoses);
                WriteD(Stat.Acemode.Kills);
                WriteD(Stat.Acemode.Deaths);
                WriteD(Stat.Acemode.Headshots);
                WriteD(Stat.Acemode.Assists);
                WriteD(Stat.Acemode.Escapes);
                WriteD(Stat.Acemode.Winstreaks);
                WriteD(0);
                WriteD(0);
                WriteD(0);
                WriteD(Player.Equipment.AccessoryId);
                WriteD((uint)(Player.Inventory.GetItem(Player.Equipment.AccessoryId) == null ? 0 : Player.Inventory.GetItem(Player.Equipment.AccessoryId).ObjectId));
                WriteB(UnknownArrayData(0)); //Max 2
                WriteB(UnknownListData(0)); //Max 2
                WriteD(Stat.Weapon.AssaultKills);
                WriteD(Stat.Weapon.AssaultDeaths);
                WriteD(Stat.Weapon.SmgKills);
                WriteD(Stat.Weapon.SmgDeaths);
                WriteD(Stat.Weapon.SniperKills);
                WriteD(Stat.Weapon.SniperDeaths);
                WriteD(Stat.Weapon.MachinegunKills);
                WriteD(Stat.Weapon.MachinegunDeaths);
                WriteD(Stat.Weapon.ShotgunKills);
                WriteD(Stat.Weapon.ShotgunDeaths);
                WriteC((byte)Characters.Count);
                WriteH((ushort)NATIONS);
                WriteB(QuickstartData(Quickstarts));
                WriteB(new byte[33]);
                WriteC(4);
                WriteB(new byte[20]);
                WriteD(Player.Title.Slots);
                WriteC(3);
                WriteC((byte)Player.Title.Equiped1);
                WriteC((byte)Player.Title.Equiped2);
                WriteC((byte)Player.Title.Equiped3);
                WriteQ(Player.Title.Flags);
                WriteC(160);
                WriteB(Player.Mission.List1); //40
                WriteB(Player.Mission.List2); //40
                WriteB(Player.Mission.List3); //40
                WriteB(Player.Mission.List4); //40
                WriteC((byte)Player.Mission.ActualMission);
                WriteC((byte)Player.Mission.Card1);
                WriteC((byte)Player.Mission.Card2);
                WriteC((byte)Player.Mission.Card3);
                WriteC((byte)Player.Mission.Card4);
                WriteB(ComDiv.GetMissionCardFlags(Player.Mission.Mission1, Player.Mission.List1)); //20
                WriteB(ComDiv.GetMissionCardFlags(Player.Mission.Mission2, Player.Mission.List2)); //20
                WriteB(ComDiv.GetMissionCardFlags(Player.Mission.Mission3, Player.Mission.List3)); //20
                WriteB(ComDiv.GetMissionCardFlags(Player.Mission.Mission4, Player.Mission.List4)); //20
                WriteC((byte)Player.Mission.Mission1);
                WriteC((byte)Player.Mission.Mission2);
                WriteC((byte)Player.Mission.Mission3);
                WriteC((byte)Player.Mission.Mission4);
                WriteD(Player.MasterMedal);
                WriteD(Player.Medal);
                WriteD(Player.Ensign);
                WriteD(Player.Ribbon);
                WriteD(0);
                WriteC(0);
                WriteD(0);
                WriteC(2);
                WriteB(new byte[406]); //Doorman Data
                WriteB(AttendanceData(EvVisit));
                WriteC(2);
                WriteD(0);
                WriteC(0);
                WriteD(0);
                WriteD(EvVisit != null ? EvVisit.Id : 0);
                WriteC((byte)(Player.Event != null ? Player.Event.LastVisitSequence1 : 0));
                WriteC(0);
                WriteC((byte)(EvVisit != null && Player.Event != null ? Player.Event.NextVisitDate >= int.Parse(DateTimeUtil.Now().AddDays(1).ToString("yyMMdd")) ? 2 : 1 : 0));
                WriteC(0);
                WriteC(1);
                WriteB(ComDiv.AddressBytes("127.0.0.1")); //Player Public IP??
                WriteD(DateNow); //DateTime without seconds 'yyMMddHHmm'
                WriteC((byte)(Characters.Count == 0 ? 0 : Player.Character.GetCharacter(Player.Equipment.CharaRedId).Slot));
                WriteC((byte)(Characters.Count == 0 ? 1 : Player.Character.GetCharacter(Player.Equipment.CharaBlueId).Slot));
                WriteD(Player.Equipment.DinoItem);
                WriteD((uint)Player.Inventory.GetItem(Player.Equipment.DinoItem).ObjectId);
                WriteD(Player.Equipment.SprayId);
                WriteD((uint)(Player.Inventory.GetItem(Player.Equipment.SprayId) == null ? 0 : Player.Inventory.GetItem(Player.Equipment.SprayId).ObjectId));
                WriteD(Player.Equipment.NameCardId);
                WriteD((uint)(Player.Inventory.GetItem(Player.Equipment.NameCardId) == null ? 0 : Player.Inventory.GetItem(Player.Equipment.NameCardId).ObjectId));
                WriteD(0);
                WriteD(0);
                WriteB(AllUtils.LoadCouponEffects(Player));
                WriteC(0);
                WriteD(0);
                WriteD(0);
                WriteC(0);
                WriteC((byte)Player.NickColor);
                WriteD(Player.Bonus.FakeRank);
                WriteD(Player.Bonus.FakeRank);
                WriteU(Player.Bonus.FakeNick, 66);
                WriteH((short)Player.Bonus.CrosshairColor);
                WriteH((short)Player.Bonus.MuzzleColor);
                WriteC(0);
                WriteD(Stat.Season.Matches);
                WriteD(Stat.Season.MatchWins);
                WriteD(Stat.Season.MatchLoses);
                WriteD(Stat.Season.MatchDraws);
                WriteD(Stat.Season.KillsCount);
                WriteD(Stat.Season.HeadshotsCount);
                WriteD(Stat.Season.DeathsCount);
                WriteD(Stat.Season.TotalMatchesCount);
                WriteD(Stat.Season.TotalKillsCount);
                WriteD(Stat.Season.EscapesCount);
                WriteD(Stat.Season.AssistsCount);
                WriteD(Stat.Season.MvpCount);
                WriteD(Stat.Basic.Matches);
                WriteD(Stat.Basic.MatchWins);
                WriteD(Stat.Basic.MatchLoses);
                WriteD(Stat.Basic.MatchDraws);
                WriteD(Stat.Basic.KillsCount);
                WriteD(Stat.Basic.HeadshotsCount);
                WriteD(Stat.Basic.DeathsCount);
                WriteD(Stat.Basic.TotalMatchesCount);
                WriteD(Stat.Basic.TotalKillsCount);
                WriteD(Stat.Basic.EscapesCount);
                WriteD(Stat.Basic.AssistsCount);
                WriteD(Stat.Basic.MvpCount);
                WriteU(Player.Nickname, 66);
                WriteD(Player.Rank);
                WriteD(Player.GetRank());
                WriteD(Player.Gold);
                WriteD(Player.Exp);
                WriteD(0);
                WriteD(0);
                WriteD(0);
                WriteC(0);
                WriteH(Player.HavePermission("observer_enabled") ? (ushort)11111 : (ushort)0);
                WriteD(Player.Tags);
                WriteH(0);
                WriteD(DateNow);
                WriteH((ushort)Player.InventoryPlus);
                WriteD(Player.Cash);
                WriteD(Clan.Id);
                WriteD(Player.ClanAccess);
                WriteQ(Player.StatusId());
                WriteC((byte)Player.CafePC);
                WriteC((byte)Player.TourneyLevel);
                WriteU(Clan.Name, 34);
                WriteC((byte)Clan.Rank);
                WriteC((byte)Clan.GetClanUnit());
                WriteD(Clan.Logo);
                WriteC((byte)Clan.NameColor);
                WriteC((byte)Clan.Effect);
                WriteC((byte)(AuthXender.Client.Config.EnableBlood ? Player.Age : 42));
            }
        }
        private byte[] AttendanceData(EventVisitModel EvVisit)
        {
            List<int> EventIds = new List<int>();
            foreach (EventVisitModel EVM in EventVisitSync.Events)
            {
                EventIds.Add(EVM.Id);
            }
            using (SyncServerPacket S = new SyncServerPacket())
            {
                PlayerEvent Event = Player.Event;
                if (EvVisit != null && (Event.LastVisitSequence1 < EvVisit.Checks && Event.NextVisitDate <= int.Parse(DateTimeUtil.Now("yyMMdd")) || Event.LastVisitSequence2 < EvVisit.Checks && Event.LastVisitSequence2 != Event.LastVisitSequence1 || EvVisit.EventIsEnabled()))
                {
                    EventVisitModel NextEvVisit = EventVisitSync.GetEvent(ComDiv.NextOf(EventIds, EvVisit.Id));
                    S.WriteU(EvVisit.Title, 70);
                    S.WriteC(0); //unk
                    S.WriteC((byte)EvVisit.Checks);
                    S.WriteD(EvVisit.Id);
                    S.WriteD(EvVisit.StartDate);
                    S.WriteD(EvVisit.EndDate);
                    S.WriteD(NextEvVisit != null ? NextEvVisit.StartDate : 0);
                    S.WriteD(NextEvVisit != null ? NextEvVisit.EndDate : 0);
                    S.WriteB(new byte[4]); //Unk
                    for (int i = 0; i < 31; i++)
                    {
                        VisitBoxModel Box = EvVisit.Boxes[i];
                        S.WriteC(0); //?
                        S.WriteC((byte)Box.RewardCount);
                        S.WriteD(Box.Reward1.GoodId);
                        S.WriteD(Box.Reward2.GoodId);
                    }
                }
                else
                {
                    S.WriteB(new byte[406]);
                }
                return S.ToArray();
            }
        }
        private byte[] QuickstartData(List<QuickstartModel> Quickstarts)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                S.WriteC((byte)Quickstarts.Count);
                foreach (QuickstartModel Quick in Quickstarts)
                {
                    S.WriteC((byte)Quick.MapId);
                    S.WriteC((byte)Quick.Rule);
                    S.WriteC((byte)Quick.StageOptions);
                    S.WriteC((byte)Quick.Type);
                }
                return S.ToArray();
            }
        }
        private byte[] UnknownListData(int ListCount)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                S.WriteC((byte)ListCount);
                for (int i = 0; i < ListCount; i++)
                {
                    S.WriteC(0); //?
                    S.WriteC(3); //?
                    S.WriteB(new byte[43]); //?
                }
                return S.ToArray();
            }
        }
        private byte[] UnknownArrayData(int ListCount)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                S.WriteC((byte)ListCount);
                for (int i = 0; i < ListCount; i++)
                {
                    S.WriteB(new byte[45]); //?
                }
                return S.ToArray();
            }
        }
    }
}