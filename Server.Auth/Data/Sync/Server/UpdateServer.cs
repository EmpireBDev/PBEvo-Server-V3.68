using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Plugin.Core.Utility;
using Plugin.Core.XML;
using System;
using System.Net;

namespace Server.Auth.Data.Sync.Server
{
    public class UpdateServer
    {
        private static DateTime LastSyncCount;
        public static void RefreshSChannel(int ServerId)
        {
            try
            {
                double PingMS = ComDiv.GetDuration(LastSyncCount);
                if (PingMS < 2.5)
                {
                    return;
                }
                LastSyncCount = DateTimeUtil.Now();
                int Players = AuthXender.SocketList.Count;
                foreach (SChannelModel Server in SChannelXML.Servers)
                {
                    if (Server.Id == ServerId)
                    {
                        Server.LastPlayers = Players;
                    }
                    else
                    {
                        IPEndPoint Sync = SynchronizeXML.GetServer(Server.Port).Connection;
                        using (SyncServerPacket S = new SyncServerPacket())
                        {
                            S.WriteH(15);
                            S.WriteD(ServerId);
                            S.WriteD(Players);
                            AuthXender.Sync.SendPacket(S.ToArray(), Sync);
                        }
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
