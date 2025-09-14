using Plugin.Core.Models;
using Plugin.Core.Network;
using Plugin.Core.Utility;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_GET_PLAYERINFO_ACK : GameServerPacket
    {
        private readonly Account Player;
        private readonly ClanModel Clan;
        public PROTOCOL_ROOM_GET_PLAYERINFO_ACK(Account Player)
        {
            this.Player = Player;
            if (Player != null)
            {
                Clan = ClanManager.GetClan(Player.ClanId);
            }
        }
        public override void Write()
        {
            WriteH(3853);
            WriteD(0);
            WriteD(0);
            WriteD(0);
            WriteH(0);
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
            WriteC(3); //?
            WriteB(CouponsData(Player));
            WriteB(WeaponsData(Player));
            WriteB(CharaData(Player));
            WriteB(PartsData(Player));
            WriteD(590851); //03 04 09 00
            WriteU(Player.Nickname, 66);
            WriteD(Player.GetRank());
            WriteD(Player.Rank);
            WriteD(Player.Gold);
            WriteD(Player.Exp);
            WriteD(0);
            WriteD(0);
            WriteD(Player.Tags);
            WriteD(Player.HavePermission("observer_enabled") ? 111111 : 0);
            WriteC(0);
            WriteC(0);
            WriteC(0);
            WriteH(0);
            WriteD(uint.Parse(DateTimeUtil.Now("yyMMddHHmm")));
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
            WriteC((byte)(GameXender.Client.Config.EnableBlood ? Player.Age : 24));
        }
        private byte[] CharaData(Account Player)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                if (Player.SlotId % 2 == 0)
                {
                    S.WriteD(Player.Equipment.CharaRedId);
                    S.WriteD((uint)Player.Inventory.GetItem(Player.Equipment.CharaRedId).ObjectId);
                } 
                else
                {
                    S.WriteD(Player.Equipment.CharaBlueId);
                    S.WriteD((uint)Player.Inventory.GetItem(Player.Equipment.CharaBlueId).ObjectId);
                }
                return S.ToArray();
            }
        }
        private byte[] WeaponsData(Account Player)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                S.WriteD(Player.Equipment.WeaponPrimary);
                S.WriteD((uint)Player.Inventory.GetItem(Player.Equipment.WeaponPrimary).ObjectId);
                S.WriteD(Player.Equipment.WeaponSecondary);
                S.WriteD((uint)Player.Inventory.GetItem(Player.Equipment.WeaponSecondary).ObjectId);
                S.WriteD(Player.Equipment.WeaponMelee);
                S.WriteD((uint)Player.Inventory.GetItem(Player.Equipment.WeaponMelee).ObjectId);
                S.WriteD(Player.Equipment.WeaponExplosive);
                S.WriteD((uint)Player.Inventory.GetItem(Player.Equipment.WeaponExplosive).ObjectId);
                S.WriteD(Player.Equipment.WeaponSpecial);
                S.WriteD((uint)Player.Inventory.GetItem(Player.Equipment.WeaponSpecial).ObjectId);
                return S.ToArray();
            }
        }
        private byte[] PartsData(Account Player)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                S.WriteD(Player.Equipment.PartHead);
                S.WriteD((uint)Player.Inventory.GetItem(Player.Equipment.PartHead).ObjectId);
                S.WriteD(Player.Equipment.PartFace);
                S.WriteD((uint)Player.Inventory.GetItem(Player.Equipment.PartFace).ObjectId);
                S.WriteD(Player.Equipment.PartJacket);
                S.WriteD((uint)Player.Inventory.GetItem(Player.Equipment.PartJacket).ObjectId);
                S.WriteD(Player.Equipment.PartPocket);
                S.WriteD((uint)Player.Inventory.GetItem(Player.Equipment.PartPocket).ObjectId);
                S.WriteD(Player.Equipment.PartGlove);
                S.WriteD((uint)Player.Inventory.GetItem(Player.Equipment.PartGlove).ObjectId);
                S.WriteD(Player.Equipment.PartBelt);
                S.WriteD((uint)Player.Inventory.GetItem(Player.Equipment.PartBelt).ObjectId);
                S.WriteD(Player.Equipment.PartHolster);
                S.WriteD((uint)Player.Inventory.GetItem(Player.Equipment.PartHolster).ObjectId);
                S.WriteD(Player.Equipment.PartSkin);
                S.WriteD((uint)Player.Inventory.GetItem(Player.Equipment.PartSkin).ObjectId);
                S.WriteD(Player.Equipment.BeretItem);
                S.WriteD((uint)(Player.Inventory.GetItem(Player.Equipment.BeretItem) == null ? 0 : Player.Inventory.GetItem(Player.Equipment.BeretItem).ObjectId));
                return S.ToArray();
            }
        }
        private byte[] CouponsData(Account Player)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                S.WriteD(Player.Equipment.DinoItem);
                S.WriteD((uint)Player.Inventory.GetItem(Player.Equipment.DinoItem).ObjectId);
                S.WriteD(Player.Equipment.SprayId);
                S.WriteD((uint)(Player.Inventory.GetItem(Player.Equipment.SprayId) == null ? 0 : Player.Inventory.GetItem(Player.Equipment.SprayId).ObjectId));
                S.WriteD(Player.Equipment.NameCardId);
                S.WriteD((uint)(Player.Inventory.GetItem(Player.Equipment.NameCardId) == null ? 0 : Player.Inventory.GetItem(Player.Equipment.NameCardId).ObjectId));
                return S.ToArray();
            }
        }
    }
}