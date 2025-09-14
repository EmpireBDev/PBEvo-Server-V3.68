using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_CLAN_LIST_DETAIL_INFO_REQ : GameClientPacket
    {
        private int ClanId, Unk;
        public PROTOCOL_CS_CLAN_LIST_DETAIL_INFO_REQ(GameClient Client, byte[] Buffer)
        {
            Makeme(Client, Buffer);
        }
        public override void Read()
        {
            ClanId = ReadD();
            Unk = ReadC();
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
                    Client.SendPacket(new PROTOCOL_CS_CLAN_LIST_DETAIL_INFO_ACK(Unk, Clan));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_CS_CLAN_LIST_DETAIL_INFO_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
