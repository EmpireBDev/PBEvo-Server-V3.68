using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.JSON;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using Server.Game.Network;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server.Game
{
    public class GameManager
    {
        private readonly string Host;
        private readonly int Port;
        public readonly int ServerId;
        public ServerConfig Config;
        public Socket MainSocket;
        public bool ServerIsClosed;
        public GameManager(int ServerId, string Host, int Port)
        {
            this.Host = Host;
            this.Port = Port;
            this.ServerId = ServerId;
        }
        public bool Start()
        {
            try
            {
                Config = ServerConfigJSON.GetConfig(ConfigLoader.ConfigId);
                MainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint Local = new IPEndPoint(IPAddress.Parse(Host), Port);
                MainSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                MainSocket.SetIPProtectionLevel(IPProtectionLevel.EdgeRestricted);
                MainSocket.Bind(Local);
                MainSocket.Listen(ConfigLoader.BackLog);
                Thread ThreadGame = new Thread(Read)
                {
                    Priority = ThreadPriority.Highest
                };
                ThreadGame.Start();
                CLogger.Print($"Game Serv Address {Local.Address}:{Local.Port}", LoggerType.Info);
                return true;
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
                return false;
            }
        }
        private void Read()
        {
            try
            {
                MainSocket.BeginAccept(new AsyncCallback(AcceptCallback), MainSocket);
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        private void AcceptCallback(IAsyncResult result)
        {
            if (ServerIsClosed)
            {
                return;
            }
            Socket ClientSocket = (Socket)result.AsyncState;
            try
            {
                Socket Handler = ClientSocket.EndAccept(result);
                if (Handler != null)
                {
                    GameClient client = new GameClient(ServerId, Handler);
                    AddSocket(client);
                    if (client == null)
                    {
                        CLogger.Print("Destroyed after failed to add to list.", LoggerType.Warning);
                    }
                    Thread.Sleep(5);
                }
            }
            catch
            {
                CLogger.Print("Failed a Client Connection", LoggerType.Warning);
            }
            MainSocket.BeginAccept(new AsyncCallback(AcceptCallback), ClientSocket);
        }
        public void AddSocket(GameClient Client)
        {
            if (Client == null)
            {
                return;
            }
            int MaxVal = 0x7FFF;
            DateTime Connect = DateTimeUtil.Now();
            for (int SessionId = 1; SessionId < MaxVal; SessionId++)
            {
                if (!GameXender.SocketList.ContainsKey(SessionId) && GameXender.SocketList.TryAdd(SessionId, Client))
                {
                    Client.SessionDate = Connect;
                    Client.SessionId = SessionId;
                    Client.SessionSeed = (ushort)new Random(Connect.Millisecond).Next(SessionId, MaxVal);
                    Client.Start();
                    return;
                }
            }
            Client.Close(500, true);
        }
        public bool RemoveSocket(GameClient Client)
        {
            try
            {
                if (Client == null || Client.SessionId == 0)
                {
                    return false;
                }
                if (GameXender.SocketList.ContainsKey(Client.SessionId) && GameXender.SocketList.TryGetValue(Client.SessionId, out Client))
                {
                    return GameXender.SocketList.TryRemove(Client.SessionId, out Client);
                }
                Client = null;
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
            return false;
        }
        public int SendPacketToAllClients(GameServerPacket Packet)
        {
            int Count = 0;
            if (GameXender.SocketList.Count == 0)
            {
                return Count;
            }
            byte[] Data = Packet.GetCompleteBytes("GameManager.SendPacketToAllClients");
            foreach (GameClient Client in GameXender.SocketList.Values)
            {
                Account Player = Client.Player;
                if (Player != null && Player.IsOnline)
                {
                    Player.SendCompletePacket(Data, Packet.GetType().Name);
                    Count++;
                }
            }
            return Count;
        }
        public Account SearchActiveClient(long accountId)
        {
            if (GameXender.SocketList.Count == 0)
            {
                return null;
            }
            foreach (GameClient client in GameXender.SocketList.Values)
            {
                Account player = client.Player;
                if (player != null && player.PlayerId == accountId)
                {
                    return player;
                }
            }
            return null;
        }
        public Account SearchActiveClient(uint sessionId)
        {
            if (GameXender.SocketList.Count == 0)
            {
                return null;
            }
            foreach (GameClient client in GameXender.SocketList.Values)
            {
                if (client.Player != null && client.SessionId == sessionId)
                {
                    return client.Player;
                }
            }
            return null;
        }
        public int KickActiveClient(double Hours)
        {
            int count = 0;
            DateTime now = DateTimeUtil.Now();
            foreach (GameClient client in GameXender.SocketList.Values)
            {
                Account pl = client.Player;
                if (pl != null && pl.Room == null && pl.ChannelId > -1 && !pl.IsGM() && (now - pl.LastLobbyEnter).TotalHours >= Hours)
                {
                    count++;
                    pl.Close(5000);
                }
            }
            return count;
        }
        public int KickCountActiveClient(double Hours)
        {
            int count = 0;
            DateTime now = DateTimeUtil.Now();
            foreach (GameClient client in GameXender.SocketList.Values)
            {
                Account pl = client.Player;
                if (pl != null && pl.Room == null && pl.ChannelId > -1 && !pl.IsGM() && (now - pl.LastLobbyEnter).TotalHours >= Hours)
                {
                    count++;
                }
            }
            return count;
        }
    }
}
