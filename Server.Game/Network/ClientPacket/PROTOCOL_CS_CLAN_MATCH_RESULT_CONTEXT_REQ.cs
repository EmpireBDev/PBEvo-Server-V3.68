using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_CLAN_MATCH_RESULT_CONTEXT_REQ : GameClientPacket
    {
        private int matchs;
        public PROTOCOL_CS_CLAN_MATCH_RESULT_CONTEXT_REQ(GameClient client, byte[] data)
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
                if (Player == null)
                {
                    return;
                }
                if (Player.ClanId > 0)
                {
                    ChannelModel Channel = Player.GetChannel();
                    if (Channel != null && Channel.Type == ChannelType.Clan)
                    {
                        lock (Channel.Matches)
                        {
                            for (int i = 0; i < Channel.Matches.Count; i++)
                            {
                                MatchModel m = Channel.Matches[i];
                                if (m.Clan.Id == Player.ClanId)
                                {
                                    matchs++;
                                }
                            }
                        }
                    }
                }
                Client.SendPacket(new PROTOCOL_CS_CLAN_MATCH_RESULT_CONTEXT_ACK(matchs));
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}