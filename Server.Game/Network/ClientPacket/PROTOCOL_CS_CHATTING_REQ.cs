using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_CHATTING_REQ : GameClientPacket
    {
        private ChattingType Type;
        private string Text;
        public PROTOCOL_CS_CHATTING_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            Type = (ChattingType)ReadH();
            Text = ReadU(ReadH() * 2);
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
                int TextLength = Text.Length, Exception = -1;
                bool ForOnlinePlayers = true, UseCache = true;
                if (TextLength <= 60 && Type == ChattingType.Clan)
                {
                    using (PROTOCOL_CS_CHATTING_ACK Packet = new PROTOCOL_CS_CHATTING_ACK(Text, Player))
                    {
                        ClanManager.SendPacket(Packet, Player.ClanId, Exception, UseCache, ForOnlinePlayers);
                    }
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}
