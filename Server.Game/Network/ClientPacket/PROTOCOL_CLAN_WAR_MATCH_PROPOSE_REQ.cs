using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using Server.Game.Data.XML;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CLAN_WAR_MATCH_PROPOSE_REQ : GameClientPacket
    {
        private int id, serverInfo;
        private uint erro;
        public PROTOCOL_CLAN_WAR_MATCH_PROPOSE_REQ(GameClient client, byte[] data)
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
                if (Player == null)
                {
                    return;
                }
                if (Player.Match != null && Player.MatchSlot == Player.Match.Leader && Player.Match.State == MatchState.Ready)
                {
                    int channelId = serverInfo - ((serverInfo / 10) * 10);
                    MatchModel mt = ChannelsXML.GetChannel(serverInfo, channelId).GetMatch(id);
                    if (mt != null)
                    {
                        Account lider = mt.GetLeader();
                        if (lider != null && lider.Connection != null && lider.IsOnline)
                        {
                            lider.SendPacket(new PROTOCOL_CLAN_WAR_CHANGE_MAX_PER_ACK(Player.Match, Player));
                        }
                        else
                        {
                            erro = 0x80000000;
                        }
                    }
                    else
                    {
                        erro = 0x80000000;
                    }
                }
                else
                {
                    erro = 0x80000000;
                }
                Client.SendPacket(new PROTOCOL_CLAN_WAR_MATCH_PROPOSE_ACK(erro));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_CLAN_WAR_MATCH_PROPOSE_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}