using Plugin.Core.Models;
using Plugin.Core.Network;
using Plugin.Core.Utility;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_GET_USER_DETAIL_INFO_ACK : GameServerPacket
    {
        private readonly uint Error;
        private readonly Account Player;
        private readonly PlayerEquipment Equip;
        private readonly StatSeason Season;
        private readonly StatWeapon Weapon;
        private readonly StatAcemode Acemode;
        private readonly StatBattleroyale Battleroyale;
        public PROTOCOL_BASE_GET_USER_DETAIL_INFO_ACK(uint Error, Account Player)
        {
            this.Error = Error;
            this.Player = Player;
            if (Player != null)
            {
                Equip = Player.Equipment;
                Season = Player.Statistic.Season;
                Weapon = Player.Statistic.Weapon;
                Acemode = Player.Statistic.Acemode;
                Battleroyale = Player.Statistic.Battleroyale;
            }
        }
        public override void Write()
        {
            WriteH(646);
            WriteH(0);
            WriteD(ComDiv.GetPlayerStatus(Player.Status, Player.IsOnline));
            if (Error == 0)
            {
                WriteQ(Player.PlayerId);
                WriteU(Player.Nickname, 66);
                WriteB(PlayerClanData(Player));
                WriteC((byte)Player.GetRank());
                WriteD(Player.Exp);
                WriteD(Season.Matches);
                WriteD(Season.MatchWins);
                WriteD(Season.MatchDraws);
                WriteD(Season.MatchLoses);
                WriteD(Season.EscapesCount);
                WriteD(Season.KillsCount);
                WriteD(Season.DeathsCount);
                WriteD(Season.HeadshotsCount);
                WriteD(Season.AssistsCount);
                WriteD(Season.MvpCount);
                WriteB(new byte[184]);
                WriteD(0);
                WriteD(0);
                WriteD(0); //player effect flags?
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
                WriteC(2); //unk
                WriteC(0); //unk
                WriteB(ItemId(Equip.WeaponPrimary));
                WriteB(ItemId(Equip.WeaponSecondary));
                WriteB(ItemId(Equip.WeaponMelee));
                WriteB(ItemId(Equip.WeaponExplosive));
                WriteB(ItemId(Equip.WeaponSpecial));
                WriteB(ItemId(Equip.CharaRedId));
                WriteB(ItemId(Equip.PartHead));
                WriteB(ItemId(Equip.PartFace));
                WriteB(ItemId(Equip.PartJacket));
                WriteB(ItemId(Equip.PartPocket));
                WriteB(ItemId(Equip.PartGlove));
                WriteB(ItemId(Equip.PartBelt));
                WriteB(ItemId(Equip.PartHolster));
                WriteB(ItemId(Equip.PartSkin));
                WriteB(ItemId(Equip.BeretItem));
                WriteC(0); //unk
                WriteD(Equip.CharaRedId);
                WriteD(Equip.CharaBlueId);
                WriteB(new byte[631]); //mercenary and clan war statistics
                WriteD(Weapon.AssaultKills);
                WriteD(Weapon.AssaultDeaths);
                WriteD(Weapon.SmgKills);
                WriteD(Weapon.SmgDeaths);
                WriteD(Weapon.SniperKills);
                WriteD(Weapon.SniperDeaths);
                WriteD(Weapon.MachinegunKills);
                WriteD(Weapon.MachinegunDeaths);
                WriteD(Weapon.ShotgunKills);
                WriteD(Weapon.ShotgunDeaths);
                WriteD(Equip.CharaRedId);
                WriteD(Equip.CharaBlueId);
                WriteC(0);
                WriteC(0);
                WriteB(new byte[16]);
                WriteD(Acemode.Matches);
                WriteD(Acemode.MatchWins);
                WriteD(Acemode.MatchLoses);
                WriteD(Acemode.Kills);
                WriteD(Acemode.Deaths);
                WriteD(Acemode.Headshots);
                WriteD(Acemode.Assists);
                WriteD(Acemode.Escapes);
                WriteD(Acemode.Winstreaks);
                WriteD(Battleroyale.Matches);
                WriteD(Player.Statistic.GetBRWinRatio());
                WriteD(Battleroyale.MatchLoses);
                WriteD(Battleroyale.KillsCount);
                WriteD(Battleroyale.DeathsCount);
                WriteD(Battleroyale.HeadshotsCount);
                WriteD(Battleroyale.AssistsCount);
                WriteD(Battleroyale.EscapesCount);
                WriteD(Player.Statistic.GetBRKDRatio());
                WriteD(Battleroyale.MatchWins);
                WriteD(Battleroyale.AverageDamage);
                WriteD(Battleroyale.PlayTime);
            }
        }
        private byte[] PlayerClanData(Account Player)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                ClanModel Clan = ClanManager.GetClan(Player.ClanId);
                S.WriteU(Clan.Name, 34);
                S.WriteD(Clan.Logo);
                S.WriteC((byte)Clan.Effect);
                return S.ToArray();
            }
        }
        private byte[] ItemId(int ItemId)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                S.WriteD(ItemId);
                S.WriteD(0); //Stock Id || Object Id
                return S.ToArray();
            }
        }
    }
}