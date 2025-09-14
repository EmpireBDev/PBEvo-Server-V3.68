using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CLAN_WAR_CREATE_TEAM_REQ : GameClientPacket
    {
        private int formacao;
        private List<int> party = new List<int>();
        public PROTOCOL_CLAN_WAR_CREATE_TEAM_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            formacao = ReadC();
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
                ChannelModel Channel = Player.GetChannel();
                if (Channel != null && Channel.Type == ChannelType.Clan && Player.Room == null)
                {
                    if (Player.Match != null)
                    {
                        Client.SendPacket(new PROTOCOL_CLAN_WAR_CREATE_TEAM_ACK(0x80001087));
                        return;
                    }
                    if (Player.ClanId == 0)
                    {
                        Client.SendPacket(new PROTOCOL_CLAN_WAR_CREATE_TEAM_ACK(0x8000105B));
                        return;
                    }
                    int matchId = -1, friendId = -1;
                    lock (Channel.Matches)
                    {
                        for (int i = 0; i < 250; i++)
                        {
                            if (Channel.GetMatch(i) == null)
                            {
                                matchId = i;
                                break;
                            }
                        }
                        for (int i = 0; i < Channel.Matches.Count; i++)
                        {
                            MatchModel m = Channel.Matches[i];
                            if (m.Clan.Id == Player.ClanId)
                            {
                                party.Add(m.FriendId);
                            }
                        }
                    }
                    for (int i = 0; i < 25; i++)
                    {
                        if (!party.Contains(i))
                        {
                            friendId = i;
                            break;
                        }
                    }
                    if (matchId == -1)
                    {
                        Client.SendPacket(new PROTOCOL_CLAN_WAR_CREATE_TEAM_ACK(0x80001088));
                    }
                    else if (friendId == -1)
                    {
                        Client.SendPacket(new PROTOCOL_CLAN_WAR_CREATE_TEAM_ACK(0x80001089));
                    }
                    else
                    {
                        MatchModel match = new MatchModel(ClanManager.GetClan(Player.ClanId))
                        {
                            MatchId = matchId,
                            FriendId = friendId,
                            Training = formacao,
                            ChannelId = Player.ChannelId,
                            ServerId = Player.ServerId
                        };
                        match.AddPlayer(Player);
                        Channel.AddMatch(match);
                        Client.SendPacket(new PROTOCOL_CLAN_WAR_CREATE_TEAM_ACK(0, match));
                        Client.SendPacket(new PROTOCOL_CLAN_WAR_REGIST_MERCENARY_ACK(match));
                    }
                }
                else
                {
                    Client.SendPacket(new PROTOCOL_CLAN_WAR_CREATE_TEAM_ACK(0x80000000));
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}