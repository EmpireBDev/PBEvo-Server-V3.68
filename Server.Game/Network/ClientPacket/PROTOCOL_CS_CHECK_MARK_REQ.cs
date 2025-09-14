using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_CHECK_MARK_REQ : GameClientPacket
    {
        private uint logo, erro;
        public PROTOCOL_CS_CHECK_MARK_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            logo = ReadUD();
        }
        public override void Run()
        {
            try
            {
                Account Player = Client.Player;
                if (Player == null || ClanManager.GetClan(Player.ClanId).Logo == logo || ClanManager.IsClanLogoExist(logo))
                {
                    erro = 0x80000000;
                }
                Client.SendPacket(new PROTOCOL_CS_CHECK_MARK_ACK(erro));
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}