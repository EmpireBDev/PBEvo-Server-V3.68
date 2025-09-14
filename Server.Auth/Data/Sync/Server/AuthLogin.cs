using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Plugin.Core.XML;
using Server.Auth.Data.Models;
using System;
using System.Net;

namespace Server.Auth.Data.Sync.Server
{
    public class AuthLogin
    {
        public static void SendLoginKickInfo(Account Player)
        {
            try
            {
                int ServerId = Player.Status.ServerId;
                if (ServerId != 255 && ServerId != 0)
                {
                    SChannelModel Server = SChannelXML.GetServer(ServerId);
                    if (Server == null)
                    {
                        return;
                    }
                    IPEndPoint Sync = SynchronizeXML.GetServer(Server.Port).Connection;
                    using (SyncServerPacket S = new SyncServerPacket())
                    {
                        S.WriteH(10);
                        S.WriteQ(Player.PlayerId);
                        AuthXender.Sync.SendPacket(S.ToArray(), Sync);
                    }
                }
                else
                {
                    Player.SetOnlineStatus(false);
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}
