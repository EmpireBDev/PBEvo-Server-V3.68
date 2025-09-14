using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Plugin.Core.Utility;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Data.Sync.Server;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_CREATE_CLAN_REQ : GameClientPacket
    {
        private uint erro;
        private string clanName, clanInfo;
        public PROTOCOL_CS_CREATE_CLAN_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            ReadD();
            clanName = ReadU(34);
            clanInfo = ReadU(510);
            ReadD();
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
                ClanModel clan = new ClanModel()
                {
                    Name = clanName,
                    Info = clanInfo,
                    Logo = 0,
                    OwnerId = Player.PlayerId,
                    CreationDate = uint.Parse(DateTimeUtil.Now("yyyyMMdd"))
                };
                if (Player.ClanId > 0 || DaoManagerSQL.GetRequestClanId(Player.PlayerId) > 0)
                {
                    erro = 0x8000105C;
                }
                else if (0 > Player.Gold - ConfigLoader.MinCreateGold || ConfigLoader.MinCreateRank > Player.Rank)
                {
                    erro = 0x8000104A;
                }
                else if (ClanManager.IsClanNameExist(clan.Name))
                {
                    erro = 0x8000105A;
                    return;
                }
                else if (ClanManager.Clans.Count > ConfigLoader.MaxActiveClans)
                {
                    erro = 0x80001055;
                }
                else if (DaoManagerSQL.CreateClan(out clan.Id, clan.Name, clan.OwnerId, clan.Info, clan.CreationDate) && DaoManagerSQL.UpdateAccountGold(Player.PlayerId, Player.Gold - ConfigLoader.MinCreateGold))
                {
                    clan.BestPlayers.SetDefault();
                    Player.ClanDate = clan.CreationDate;
                    if (ComDiv.UpdateDB("accounts", "player_id", Player.PlayerId, new string[] { "clan_access", "clan_date", "clan_id" }, 1, (long)clan.CreationDate, clan.Id))
                    {
                        if (clan.Id > 0)
                        {
                            Player.ClanId = clan.Id;
                            Player.ClanAccess = 1;
                            ClanManager.AddClan(clan);
                            SendClanInfo.Load(clan, 0);
                            Player.Gold -= ConfigLoader.MinCreateGold;
                        }
                        else
                        {
                            erro = 0x8000104B;
                        }
                    }
                    else
                    {
                        erro = 0x80001048;
                    }
                }
                else
                {
                    erro = 0x80001048;
                }
                Client.SendPacket(new PROTOCOL_CS_CREATE_CLAN_ACK(erro, clan, Player));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_CS_CREATE_CLAN_REQ: {ex.Message}", LoggerType.Error, ex);
            }
            /*
             * 80001055 - STBL_IDX_CLAN_MESSAGE_FAIL_CREATING_OVERFLOW
             * 8000105C - STBL_IDX_CLAN_MESSAGE_FAIL_CREATING_ALREADY
             * 8000105A - STBL_IDX_CLAN_MESSAGE_FAIL_CREATING_OVERLAPPING
             * 80001048 - STBL_IDX_CLAN_MESSAGE_FAIL_CREATING
             * Padrão: STBL_IDX_CLAN_MESSAGE_FAIL_CREATING_ADMIN
             */
        }
    }
}
