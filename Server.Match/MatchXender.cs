using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.XML;
using Server.Match.Data.Sync;
using System;
using System.Net;

namespace Server.Match
{
    public class MatchXender
    {
        public static MatchSync Sync = null;
        public static MatchManager Client = null;
        public static bool GetPlugin(string Host, int Port)
        {
            try
            {
                IPEndPoint EP = SynchronizeXML.GetServer(Port).Connection;
                Sync = new MatchSync(EP);
                Client = new MatchManager(Host, Port);
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
