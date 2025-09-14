using Plugin.Core;
using Plugin.Core.Managers;
using Plugin.Core.Network;
using Server.Game.Network.ServerPacket;
using Server.Game.Data.Managers;
using System.Threading;
using Plugin.Core.XML;
using Server.Game.Data.Models;
using Plugin.Core.Enums;
using System.Net.Sockets;

namespace Server.Game.Data.Sync.Client
{
    public class ServerWarning
    {
        public static void LoadGMWarning(SyncClientPacket C)
        {
            string Username = C.ReadS(C.ReadC());
            string Password = C.ReadS(C.ReadC());
            string Message = C.ReadS(C.ReadH());
            Account Player = AccountManager.GetAccountDB(Username, 0, 31);
            if (Player != null && Player.Password == Password && Player.Access >= AccessLevel.GAMEMASTER)
            {
                int Count = 0;
                using (PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK Packet = new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(Message))
                {
                    Count = GameXender.Client.SendPacketToAllClients(Packet);
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
            Account Player = AccountManager.GetAccountDB(Username, 0, 31);
            if (Player != null && Player.Password == Password && Player.Access >= AccessLevel.GAMEMASTER)
            {
                int Count = 0;
                foreach (GameClient Client in GameXender.SocketList.Values)
                {
                    Client.Client.Shutdown(SocketShutdown.Both);
                    Client.Client.Close(10000);
                    Count += 1;
                }
                CLogger.Print($"Disconnected Players: {Count} (By: {Username})", LoggerType.Warning);
                GameXender.Client.ServerIsClosed = true;
                GameXender.Client.MainSocket.Close(5000);
                CLogger.Print("1/2 Step", LoggerType.Warning);
                Thread.Sleep(5000);
                GameXender.Sync.Close();
                CLogger.Print("2/2 Step", LoggerType.Warning);
                foreach (GameClient Client in GameXender.SocketList.Values)
                {
                    Client.Close(0, true);
                }
                CLogger.Print($"Shutdowned Server: {Count} players disconnected; by Login: '" + Username + ";", LoggerType.Command);
            }
        }
    }
}