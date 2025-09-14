using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using Server.Game.Data.XML;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CLAN_WAR_JOIN_TEAM_REQ : GameClientPacket
    {
        private int matchId, serverInfo, type;
        private uint erro;
        public PROTOCOL_CLAN_WAR_JOIN_TEAM_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            matchId = ReadH();
            serverInfo = ReadH();
            type = ReadC(); //0 normal | 1 - amigos do clã
        }
        public override void Run()
        {
            try
            {
                Account p = Client.Player;
                if (type >= 2 || p == null || p.Match != null || p.Room != null)
                {
                    Client.SendPacket(new PROTOCOL_CLAN_WAR_JOIN_TEAM_ACK(0x80000000));
                    return;
                }
                int channelId = serverInfo - ((serverInfo / 10) * 10);
                ChannelModel ch = ChannelsXML.GetChannel(serverInfo, type == 0 ? channelId : p.ChannelId);
                if (ch != null)
                {
                    if (p.ClanId == 0)
                    {
                        Client.SendPacket(new PROTOCOL_CLAN_WAR_JOIN_TEAM_ACK(0x8000105B));
                    }
                    else
                    {
                        MatchModel mt = type == 1 ? ch.GetMatch(matchId, p.ClanId) : ch.GetMatch(matchId);
                        if (mt != null)
                        {
                            JoinPlayer(p, mt);
                        }
                        else
                        {
                            Client.SendPacket(new PROTOCOL_CLAN_WAR_JOIN_TEAM_ACK(0x80000000));
                        }
                    }
                }
                else
                {
                    Client.SendPacket(new PROTOCOL_CLAN_WAR_JOIN_TEAM_ACK(0x80000000));
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        private void JoinPlayer(Account p, MatchModel mt)
        {
            if (!mt.AddPlayer(p))
            {
                erro = 0x80000000;
            }
            Client.SendPacket(new PROTOCOL_CLAN_WAR_JOIN_TEAM_ACK(erro, mt));
            if (erro == 0)
            {
                using (PROTOCOL_CLAN_WAR_REGIST_MERCENARY_ACK packet = new PROTOCOL_CLAN_WAR_REGIST_MERCENARY_ACK(mt))
                {
                    mt.SendPacketToPlayers(packet);
                }
            }
        }
    }
}