using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using Server.Game.Data.XML;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CLAN_WAR_INVITE_ACCEPT_REQ : GameClientPacket
    {
        private int id, serverInfo, type;
        private uint erro;
        public PROTOCOL_CLAN_WAR_INVITE_ACCEPT_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            ReadD();
            id = ReadH();
            serverInfo = ReadH();
            type = ReadC();
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
                MatchModel PMatch = Player.Match;
                int channelId = serverInfo - ((serverInfo / 10) * 10);
                MatchModel Match = ChannelsXML.GetChannel(serverInfo, channelId).GetMatch(id);
                if (PMatch != null && Match != null && Player.MatchSlot == PMatch.Leader)
                {
                    if (type == 1)
                    {
                        if (PMatch.Training != Match.Training)
                        {
                            erro = 2147487890;
                        }
                        else if (Match.GetCountPlayers() != PMatch.Training || PMatch.GetCountPlayers() != PMatch.Training)
                        {
                            erro = 2147487889;
                        }
                        else if (Match.State == MatchState.Play || PMatch.State == MatchState.Play)
                        {
                            erro = 2147487888;
                        }
                        else
                        {
                            PMatch.State = MatchState.Play;
                            Account pM = Match.GetLeader();
                            if (pM != null && pM.Match != null)
                            {
                                pM.SendPacket(new PROTOCOL_CLAN_WAR_ENEMY_INFO_ACK(PMatch));
                                pM.SendPacket(new PROTOCOL_CLAN_WAR_CREATE_ROOM_ACK(PMatch));
                                Match.Slots[pM.MatchSlot].State = SlotMatchState.Ready;
                            }
                            Match.State = MatchState.Play;
                        }
                    }
                    else
                    {
                        Account pM = Match.GetLeader();
                        if (pM != null && pM.Match != null)
                        {
                            pM.SendPacket(new PROTOCOL_CLAN_WAR_INVITE_ACCEPT_ACK(0x80001093));
                        }
                    }
                }
                else
                {
                    erro = 0x80001094;
                }
                Client.SendPacket(new PROTOCOL_CLAN_WAR_ACCEPT_BATTLE_ACK(erro));
            }
            catch (Exception ex)
            {
                CLogger.Print($"CLAN_WAR_ACCEPT_BATTLE_REC: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}