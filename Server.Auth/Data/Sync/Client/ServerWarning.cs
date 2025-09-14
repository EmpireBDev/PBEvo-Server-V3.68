using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Managers;
using Plugin.Core.Network;
using Plugin.Core.XML;
using Server.Auth.Data.Managers;
using Server.Auth.Data.Models;
using Server.Auth.Network.ServerPacket;
using System.Net.Sockets;
using System.Threading;

namespace Server.Auth.Data.Sync.Client
{
    public static class ServerWarning
    {
        public static void LoadGMWarning(SyncClientPacket C)
        {
            string Username = C.ReadS(C.ReadC());
            string Password = C.ReadS(C.ReadC());
            string Message = C.ReadS(C.ReadH());
            Account Player = AccountManager.GetAccountDB(Username, Password, 2, 31);
            if (Player != null && Player.Password == Password && Player.Access >= AccessLevel.GAMEMASTER)
            {
                int Count = 0;
                using (PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK Packet = new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(Message))
                {
                    Count = AuthXender.Client.SendPacketToAllClients(Packet);
                }
                CLogger.Print($"Message sent to '{Count}' Players: '{Message}'; by Username: '{Username}'", LoggerType.Command);
            }
        }
        public static void LoadShopRestart(SyncClientPacket C)
        {
            int Type = C.ReadC();
            ShopManager.Reset();
            ShopManager.Load(Type);
            CLogger.Print($"Shop restarted. (Type: {Type})", LoggerType.Command);
        }
        public static void LoadServerUpdate(SyncClientPacket C)
        {
            int serverId = C.ReadC();
            SChannelXML.UpdateServer(serverId);
            CLogger.Print($"Server updated. (Id: {serverId})", LoggerType.Command);
        }
        public static void LoadShutdown(SyncClientPacket C)
        {
            string Username = C.ReadS(C.ReadC());
            string Password = C.ReadS(C.ReadC());
            Account Player = AccountManager.GetAccountDB(Username, Password, 2, 31);
            if (Player != null && Player.Password == Password && Player.Access >= AccessLevel.GAMEMASTER)
            {
                int Count = 0;
                foreach (AuthClient Client in AuthXender.SocketList.Values)
                {
                    Client.Client.Shutdown(SocketShutdown.Both);
                    Client.Client.Close(10000);
                    Count += 1;
                }
                CLogger.Print($"Disconnected Players: {Count} (By: {Username})", LoggerType.Warning);
                AuthXender.Client.ServerIsClosed = true;
                AuthXender.Client.MainSocket.Close(5000);
                CLogger.Print("1/2 Step", LoggerType.Warning);
                Thread.Sleep(5000);
                AuthXender.Sync.Close();
                CLogger.Print("2/2 Step", LoggerType.Warning);
                foreach (AuthClient Client in AuthXender.SocketList.Values)
                {
                    Client.Close(0, true);
                }
                CLogger.Print($"Shutdowned Server: {Count} players disconnected; by Username: '{Username};", LoggerType.Command);
            }
        }
    }
}