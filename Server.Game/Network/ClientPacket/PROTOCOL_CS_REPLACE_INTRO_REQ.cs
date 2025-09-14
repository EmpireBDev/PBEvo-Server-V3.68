using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;
using System.Text;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_REPLACE_INTRO_REQ : GameClientPacket
    {
        private string ClanInfo;
        private uint erro;
        public PROTOCOL_CS_REPLACE_INTRO_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            ClanInfo = ReadU(ReadC() * 2);
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
                if (Clan.Id > 0 && Clan.Info != ClanInfo && (Clan.OwnerId == Client.PlayerId || Player.ClanAccess >= 1 && Player.ClanAccess <= 2))
                {
                    if (ComDiv.UpdateDB("system_clan", "info", ClanInfo, "id", Clan.Id))
                    {
                        Clan.Info = ClanInfo;
                    }
                    else
                    {
                        erro = 2147487860;
                    }
                }
                else
                {
                    erro = 2147487835;
                }
                Client.SendPacket(new PROTOCOL_CS_REPLACE_INTRO_ACK(erro));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_CS_PAGE_CHATTING_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}