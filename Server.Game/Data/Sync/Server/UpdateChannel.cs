using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Plugin.Core.XML;
using System;
using System.Net;

namespace Server.Game.Data.Sync.Server
{
    public class UpdateChannel
    {
        public static void RefreshChannel(int ServerId, int ChannelId, int Count)
        {
            try
            {
                SChannelModel Server = GameXender.Sync.GetServer(0);
                if (Server == null)
                {
                    return;
                }
                IPEndPoint Sync = SynchronizeXML.GetServer(Server.Port).Connection;
                using (SyncServerPacket S = new SyncServerPacket())
                {
                    S.WriteH(33);
                    S.WriteD(ServerId);
                    S.WriteD(ChannelId);
                    S.WriteD(Count);
                    GameXender.Sync.SendPacket(S.ToArray(), Sync);
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}
