using Server.Game.Data.Managers;
using Server.Game.Network.ServerPacket;
using Plugin.Core;
using System.Text;
using Plugin.Core.Utility;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using System;
using Plugin.Core.Enums;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_REPLACE_NOTICE_REQ : GameClientPacket
    {
        private string clan_news;
        private uint erro;
        public PROTOCOL_CS_REPLACE_NOTICE_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            clan_news = ReadU(ReadC() * 2);
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
                if (Clan.Id > 0 && Clan.News != clan_news && (Clan.OwnerId == Client.PlayerId || Player.ClanAccess >= 1 && Player.ClanAccess <= 2))
                {
                    if (ComDiv.UpdateDB("system_clan", "news", clan_news, "id", Clan.Id))
                    {
                        Clan.News = clan_news;
                    }
                    else
                    {
                        erro = 2147487859;
                    }
                }
                else
                {
                    erro = 2147487835;
                }
                Client.SendPacket(new PROTOCOL_CS_REPLACE_NOTICE_ACK(erro));
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}