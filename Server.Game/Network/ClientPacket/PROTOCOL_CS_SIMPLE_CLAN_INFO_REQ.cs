using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_SIMPLE_CLAN_INFO_REQ : GameClientPacket
    {
        private int ClanId;
        public PROTOCOL_CS_SIMPLE_CLAN_INFO_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            ClanId = ReadD();
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
                Player.FindClanId = ClanId;
                ClanModel Clan = ClanManager.GetClan(ClanId);
                if (Clan.Id > 0)
                {
                    Client.SendPacket(new PROTOCOL_CS_CLAN_LIST_DETAIL_INFO_ACK(0, Clan));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_CS_SIMPLE_CLAN_INFO_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}