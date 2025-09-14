using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Plugin.Core.Utility;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_MEMBER_LIST_REQ : GameClientPacket
    {
        private int page;
        public PROTOCOL_CS_MEMBER_LIST_REQ(GameClient client, byte[] data)
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
                Account Player = Client.Player;
                if (Player == null)
                {
                    return;
                }
                ClanModel Clan = ClanManager.GetClan(Player.FindClanId);
                if (Clan.Id == 0)
                {
                    Client.SendPacket(new PROTOCOL_CS_MEMBER_LIST_ACK(-1));
                    return;
                }
                List<Account> ClanPlayers = ClanManager.GetClanPlayers(Player.FindClanId, -1, false);
                using (SyncServerPacket S = new SyncServerPacket())
                {
                    int count = 0;
                    for (int i = (page * 50); i < ClanPlayers.Count; i++)
                    {
                        WriteData(ClanPlayers[i], S);
                        if (++count == 50)
                        {
                            break;
                        }
                    }
                    Client.SendPacket(new PROTOCOL_CS_MEMBER_LIST_ACK(page, count, S.ToArray()));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_CS_MEMBER_LIST_REQ {ex.Message}", LoggerType.Error, ex);
            }
        }
        private void WriteData(Account Member, SyncServerPacket S)
        {
            S.WriteQ(Member.PlayerId);
            S.WriteU(Member.Nickname, 66);
            S.WriteC((byte)Member.Rank);
            S.WriteC((byte)Member.ClanAccess);
            S.WriteQ(ComDiv.GetClanStatus(Member.Status, Member.IsOnline));
            S.WriteD(Member.ClanDate);
            S.WriteC((byte)Member.NickColor);
            S.WriteD(Member.Statistic.Clan.Matches); //member.Statistic.Clan need to be checked. Sometimes it detected Null so MemberList PAGE NOT SHOWS PLAYER.
            S.WriteD(Member.Statistic.Clan.MatchWins); //member.Statistic.Clan need to be checked. Sometimes it detected Null so MemberList PAGE NOT SHOWS PLAYER.
            S.WriteD(Member.NameCard);
            S.WriteC((byte)0); //UNK: wesley said this is bonus.nickEffect
            S.WriteD(10); //Medals of the week
            S.WriteD(20); //Medals of the month
            S.WriteD(30); //Medal total
        }
    }
}
