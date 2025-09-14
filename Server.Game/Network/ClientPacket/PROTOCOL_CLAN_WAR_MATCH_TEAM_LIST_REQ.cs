using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CLAN_WAR_MATCH_TEAM_LIST_REQ : GameClientPacket
    {
        public PROTOCOL_CLAN_WAR_MATCH_TEAM_LIST_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
        }
        public override void Run()
        {
            try
            {
                Account Player = Client.Player;
                if (Player == null || Player.Match == null)
                {
                    return;
                }
                ChannelModel Channel = Player.GetChannel();
                if (Channel != null && Channel.Type == ChannelType.Clan)
                {
                    Client.SendPacket(new PROTOCOL_CLAN_WAR_MATCH_TEAM_LIST_ACK(Channel.Matches, Player.Match.MatchId));
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}