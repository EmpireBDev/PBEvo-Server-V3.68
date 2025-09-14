using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_DETAIL_INFO_REQ : GameClientPacket
    {
        private int clanId, unk;
        public PROTOCOL_CS_DETAIL_INFO_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            clanId = ReadD();
            unk = ReadC(); //1 = Always | 0 = Not Owner
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
                Player.FindClanId = clanId;
                ClanModel Clan = ClanManager.GetClan(clanId);
                if (Clan.Id > 0)
                {
                    Client.SendPacket(new PROTOCOL_CS_DETAIL_INFO_ACK(unk, Clan));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_CS_DETAIL_INFO_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}