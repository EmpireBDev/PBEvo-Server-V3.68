using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using Server.Game.Data.XML;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CLAN_WAR_MATCH_TEAM_INFO_REQ : GameClientPacket
    {
        private int id, serverInfo;
        public PROTOCOL_CLAN_WAR_MATCH_TEAM_INFO_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            id = ReadH();
            serverInfo = ReadH();
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
                int channelId = serverInfo - ((serverInfo / 10) * 10);
                ChannelModel ch = ChannelsXML.GetChannel(serverInfo, channelId);
                if (ch != null)
                {
                    MatchModel match = ch.GetMatch(id);
                    if (match != null)
                    {
                        Client.SendPacket(new PROTOCOL_CLAN_WAR_MATCH_TEAM_INFO_ACK(0, match.Clan));
                    }
                    else
                    {
                        Client.SendPacket(new PROTOCOL_CLAN_WAR_MATCH_TEAM_INFO_ACK(0x80000000));
                    }
                }
                else
                {
                    Client.SendPacket(new PROTOCOL_CLAN_WAR_MATCH_TEAM_INFO_ACK(0x80000000));
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}