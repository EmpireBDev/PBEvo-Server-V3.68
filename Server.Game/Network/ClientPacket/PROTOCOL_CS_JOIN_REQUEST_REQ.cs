using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Plugin.Core.Utility;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_JOIN_REQUEST_REQ : GameClientPacket
    {
        private int clanId;
        private string text;
        private uint erro;
        public PROTOCOL_CS_JOIN_REQUEST_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            clanId = ReadD();
            text = ReadU(ReadC() * 2);
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
                ClanInvite Invite = new ClanInvite()
                {
                    Id = clanId,
                    PlayerId = Client.PlayerId,
                    Text = text,
                    InviteDate = uint.Parse(DateTimeUtil.Now("yyyyMMdd"))
                };
                if (Player.ClanId > 0 || Player.Nickname.Length == 0)
                {
                    erro = 2147487836;
                }
                else if (ClanManager.GetClan(clanId).Id == 0)
                {
                    erro = 0x80000000;
                }
                else if (DaoManagerSQL.GetRequestClanInviteCount(clanId) >= 100)
                {
                    erro = 2147487831;
                }
                else if (!DaoManagerSQL.CreateClanInviteInDB(Invite))
                {
                    erro = 2147487848;
                }
                Invite = null;
                Client.SendPacket(new PROTOCOL_CS_JOIN_REQUEST_ACK(erro, clanId));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_CS_JOIN_REQUEST_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}