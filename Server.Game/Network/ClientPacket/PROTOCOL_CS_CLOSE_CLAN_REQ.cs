using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Data.Sync.Server;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_CLOSE_CLAN_REQ : GameClientPacket
    {
        private uint erro;
        public PROTOCOL_CS_CLOSE_CLAN_REQ(GameClient client, byte[] data)
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
                ClanModel Clan = ClanManager.GetClan(Player.ClanId);
                if (Clan.Id > 0 && Clan.OwnerId == Client.PlayerId && ComDiv.DeleteDB("system_clan", "id", Clan.Id) && ComDiv.UpdateDB("accounts", "player_id", Player.PlayerId, new string[] { "clan_id", "clan_access" }, 0, 0) && ComDiv.UpdateDB("player_stat_clans", "owner_id", Player.PlayerId, new string[] { "clan_matches", "clan_match_wins" }, 0, 0) && ClanManager.RemoveClan(Clan))
                {
                    Player.ClanId = 0;
                    Player.ClanAccess = 0;
                    SendClanInfo.Load(Clan, 1);
                }
                else
                {
                    erro = 2147487850;
                }
                Client.SendPacket(new PROTOCOL_CS_CLOSE_CLAN_ACK(erro));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_CS_CLOSE_CLAN_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}