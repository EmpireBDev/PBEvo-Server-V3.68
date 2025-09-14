using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.XML;
using Server.Game.Data.Sync;
using System;
using System.Collections.Concurrent;
using System.Net;

namespace Server.Game
{
    public class GameXender
    {
        public static GameSync Sync = null;
        public static GameManager Client = null;
        public static ConcurrentDictionary<int, GameClient> SocketList;
        public static bool GetPlugin(int ServerId, string Host, int Port)
        {
            try
            {
                SocketList = new ConcurrentDictionary<int, GameClient>();
                IPEndPoint EP = SynchronizeXML.GetServer(Port).Connection;
                Sync = new GameSync(EP);
                if (ConfigLoader.AESProxy)
                {
                    Client = new GameManager(ServerId, Host, 47777 + ServerId);
                }
                else
                {
                    Client = new GameManager(ServerId, Host, Port);
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
