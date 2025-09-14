using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Plugin.Core.XML;
using Server.Game.Data.Models;
using System;
using System.Net;

namespace Server.Game.Data.Sync.Server
{
    public class SendClanInfo
    {
        public static void Load(Account Player, Account Member, int Type)
        {
            try
            {
                if (Player == null)
                {
                    return;
                }
                SChannelModel Server = GameXender.Sync.GetServer(Player.Status);
                if (Server == null)
                {
                    return;
                }
                IPEndPoint Sync = SynchronizeXML.GetServer(Server.Port).Connection;
                using (SyncServerPacket S = new SyncServerPacket())
                {
                    S.WriteH(16);
                    S.WriteQ(Player.PlayerId);
                    S.WriteC((byte)Type);
                    if (Type == 1) //adicionar
                    {
                        S.WriteQ(Member.PlayerId);
                        S.WriteC((byte)(Member.Nickname.Length + 1));
                        S.WriteS(Member.Nickname, Member.Nickname.Length + 1);
                        S.WriteB(Member.Status.Buffer);
                        S.WriteC((byte)Member.Rank);
                    }
                    else if (Type == 2) //remover
                    {
                        S.WriteQ(Member.PlayerId);
                    }
                    else if (Type == 3) //atualizar id do clã e aux
                    {
                        S.WriteD(Player.ClanId);
                        S.WriteC((byte)Player.ClanAccess);
                    }
                    GameXender.Sync.SendPacket(S.ToArray(), Sync);
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        public static void Update(ClanModel Clan, int Type)
        {
            try
            {
                foreach (SChannelModel Server in SChannelXML.Servers)
                {
                    if (Server.Id == 0 || Server.Id == GameXender.Client.ServerId)
                    {
                        continue;
                    }
                    IPEndPoint Sync = SynchronizeXML.GetServer(Server.Port).Connection;
                    using (SyncServerPacket S = new SyncServerPacket())
                    {
                        S.WriteH(22);
                        S.WriteC((byte)Type);
                        if (Type == 0)
                        {
                            S.WriteQ(Clan.OwnerId);
                        }
                        else if (Type == 1)
                        {
                            S.WriteC((byte)(Clan.Name.Length + 1));
                            S.WriteS(Clan.Name, Clan.Name.Length + 1);
                        }
                        else if (Type == 2)
                        {
                            S.WriteC((byte)Clan.NameColor);
                        }
                        GameXender.Sync.SendPacket(S.ToArray(), Sync);
                    }
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        public static void Load(ClanModel Clan, int Type)
        {
            try
            {
                foreach (SChannelModel Server in SChannelXML.Servers)
                {
                    if (Server.Id == 0 || Server.Id == GameXender.Client.ServerId)
                    {
                        continue;
                    }
                    IPEndPoint Sync = SynchronizeXML.GetServer(Server.Port).Connection;
                    using (SyncServerPacket S = new SyncServerPacket())
                    {
                        S.WriteH(21);
                        S.WriteC((byte)Type);
                        S.WriteD(Clan.Id);
                        if (Type == 0)
                        {
                            S.WriteQ(Clan.OwnerId);
                            S.WriteD(Clan.CreationDate);
                            S.WriteC((byte)(Clan.Name.Length + 1));
                            S.WriteS(Clan.Name, Clan.Name.Length + 1);
                            S.WriteC((byte)(Clan.Info.Length + 1));
                            S.WriteS(Clan.Info, Clan.Info.Length + 1);
                        }
                        GameXender.Sync.SendPacket(S.ToArray(), Sync);
                    }
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}