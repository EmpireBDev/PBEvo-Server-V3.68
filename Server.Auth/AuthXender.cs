using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.XML;
using Server.Auth.Data.Sync;
using System;
using System.Collections.Concurrent;
using System.Net;

namespace Server.Auth
{
    public class AuthXender
    {
        public static AuthSync Sync = null;
        public static AuthManager Client = null;
        public static ConcurrentDictionary<int, AuthClient> SocketList;
        public static bool GetPlugin(string Host, int Port)
        {
            try
            {
                SocketList = new ConcurrentDictionary<int, AuthClient>();
                IPEndPoint EP = SynchronizeXML.GetServer(Port).Connection;
                Sync = new AuthSync(EP);
                if (ConfigLoader.AESProxy)
                {
                    Client = new AuthManager(0, Host, 47777);
                }
                else
                {
                    Client = new AuthManager(0, Host, Port);
                }
                Sync.Start();
                Client.Start();
                return true;
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
                return false;
            }
        }
    }
}
