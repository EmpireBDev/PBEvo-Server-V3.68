using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CLAN_WAR_TEAM_CHATTING_REQ : GameClientPacket
    {
        private ChattingType type;
        private string text;
        public PROTOCOL_CLAN_WAR_TEAM_CHATTING_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            type = (ChattingType)ReadH();
            text = ReadS(ReadH());
        }
        public override void Run()
        {
            try
            {
                Account Player = Client.Player;
                if (Player == null || Player.Match == null || type != ChattingType.Match)
                {
                    return;
                }
                MatchModel match = Player.Match;
                using (PROTOCOL_CLAN_WAR_TEAM_CHATTING_ACK packet = new PROTOCOL_CLAN_WAR_TEAM_CHATTING_ACK(Player.Nickname, text))
                {
                    match.SendPacketToPlayers(packet);
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_CLAN_WAR_TEAM_CHATTING_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
