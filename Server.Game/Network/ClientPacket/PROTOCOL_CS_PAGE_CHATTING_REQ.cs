using Server.Game.Data.Managers;
using Server.Game.Network.ServerPacket;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using System;
using Plugin.Core;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_PAGE_CHATTING_REQ : GameClientPacket
    {
        private ChattingType type;
        private string text;
        public PROTOCOL_CS_PAGE_CHATTING_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            type = (ChattingType)ReadH();
            text = ReadU(ReadH() * 2);
        }
        public override void Run()
        {
            try
            {
                Account Player = Client.Player;
                if (Player == null || type != ChattingType.ClanMemberPage)
                {
                    return;
                }
                using (PROTOCOL_CS_PAGE_CHATTING_ACK packet = new PROTOCOL_CS_PAGE_CHATTING_ACK(Player, text))
                {
                    ClanManager.SendPacket(packet, Player.ClanId, -1, true, true);
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_CS_PAGE_CHATTING_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}