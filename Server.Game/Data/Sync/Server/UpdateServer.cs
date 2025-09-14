using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Plugin.Core.Utility;
using Plugin.Core.XML;
using Server.Game.Data.Models;
using Server.Game.Data.XML;
using System;
using System.Net;

namespace Server.Game.Data.Sync.Server
{
    public class UpdateServer
    {
        private static DateTime LastSyncCount;
        public static void RefreshSChannel(int serverId)
        {
            try
            {
                double PingMS = ComDiv.GetDuration(LastSyncCount);
                if (PingMS < 5)
                {
                    return;
                }
                LastSyncCount = DateTimeUtil.Now();
                int Players = 0;
                foreach (ChannelModel Channel in ChannelsXML.Channels)
                {
                    Players += Channel.Players.Count;
                }
                foreach (SChannelModel Server in SChannelXML.Servers)
                {
                    if (Server.Id == serverId)
                    {
                        Server.LastPlayers = Players;
                    }
                    else
                    {
                        IPEndPoint Sync = SynchronizeXML.GetServer(Server.Port).Connection;
                        using (SyncServerPacket S = new SyncServerPacket())
                        {
                            S.WriteH(15);
                            S.WriteD(serverId);
                            S.WriteD(Players);
                            GameXender.Sync.SendPacket(S.ToArray(), Sync);
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
