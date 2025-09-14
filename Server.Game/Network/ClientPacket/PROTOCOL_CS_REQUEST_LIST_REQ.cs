using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Plugin.Core.SQL;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_REQUEST_LIST_REQ : GameClientPacket
    {
        private int page;
        public PROTOCOL_CS_REQUEST_LIST_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            page = ReadC();
        }
        public override void Run()
        {
            try
            {
                Account player = Client.Player;
                if (player == null)
                {
                    return;
                }
                if (player.ClanId == 0)
                {
                    Client.SendPacket(new PROTOCOL_CS_REQUEST_LIST_ACK(-1));
                    return;
                }
                List<ClanInvite> clanInvites = DaoManagerSQL.GetClanRequestList(player.ClanId);
                using (SyncServerPacket C = new SyncServerPacket())
                {
                    int count = 0;
                    int Startid;
                    if (page == 0)
                    {
                        Startid = 13;
                    }
                    else
                    {
                        Startid = 14;
                    }
                    for (int i = (page * Startid); i < clanInvites.Count; i++)
                    {
                        WriteData(clanInvites[i], C);
                        if (++count == 13)
                        {
                            break;
                        }
                    }
                    Client.SendPacket(new PROTOCOL_CS_REQUEST_LIST_ACK(0, count, page, C.ToArray()));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_CS_REQUEST_LIST_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
        private void WriteData(ClanInvite Invite, SyncServerPacket S)
        {
            S.WriteQ(Invite.PlayerId);
            Account Player = AccountManager.GetAccount(Invite.PlayerId, 31);
            if (Player != null)
            {
                S.WriteU(Player.Nickname, 66);
                S.WriteC((byte)Player.Rank);
                S.WriteC((byte)Player.NickColor);
                S.WriteD(Invite.InviteDate);
                S.WriteD(Player.Statistic.Basic.KillsCount);
                S.WriteD(Player.Statistic.Basic.DeathsCount);
                S.WriteD(Player.Statistic.Basic.Matches);
                S.WriteD(Player.Statistic.Basic.MatchWins);
                S.WriteD(Player.Statistic.Basic.MatchLoses);
                S.WriteN(Invite.Text, Invite.Text.Length + 2, "UTF-16LE");
            }
            S.WriteD(Invite.InviteDate);
        }
    }
}