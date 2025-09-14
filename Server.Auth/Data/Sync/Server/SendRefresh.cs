using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Plugin.Core.XML;
using Server.Auth.Data.Managers;
using Server.Auth.Data.Models;
using System;
using System.Net;

namespace Server.Auth.Data.Sync.Server
{
    public static class SendRefresh
    {
        public static void RefreshAccount(Account Player, bool IsConnect)
        {
            try
            {
                UpdateServer.RefreshSChannel(0);
                AccountManager.GetFriendlyAccounts(Player.Friend);
                foreach (FriendModel Friend in Player.Friend.Friends)
                {
                    PlayerInfo Info = Friend.Info;
                    if (Info != null)
                    {
                        SChannelModel Server = SChannelXML.GetServer(Info.Status.ServerId);
                        if (Server == null)
                        {
                            continue;
                        }
                        SendRefreshPacket(0, Player.PlayerId, Friend.PlayerId, IsConnect, Server);
                    }
                }
                if (Player.ClanId > 0)
                {
                    foreach (Account Member in Player.ClanPlayers)
                    {
                        if (Member != null && Member.IsOnline)
                        {
                            SChannelModel Server = SChannelXML.GetServer(Member.Status.ServerId);
                            if (Server == null)
                            {
                                continue;
                            }
                            SendRefreshPacket(1, Player.PlayerId, Member.PlayerId, IsConnect, Server);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        public static void SendRefreshPacket(int Type, long PlayerId, long MemberId, bool IsConnect, SChannelModel Server)
        {
            IPEndPoint Sync = SynchronizeXML.GetServer(Server.Port).Connection;
            using (SyncServerPacket S = new SyncServerPacket())
            {
                S.WriteH(11);
                S.WriteC((byte)Type);
                S.WriteC((byte)(IsConnect ? 1 : 0));
                S.WriteQ(PlayerId);
                S.WriteQ(MemberId);
                AuthXender.Sync.SendPacket(S.ToArray(), Sync);
            }
        }
    }
}