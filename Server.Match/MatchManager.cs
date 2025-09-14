using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Utility;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server.Match
{
    public class MatchManager
    {
        private readonly string Host;
        private readonly int Port;
        public Socket MainSocket;
        public bool ServerIsClosed;
        private StateObject State;
        public MatchManager(string Host, int Port)
        {
            this.Host = Host;
            this.Port = Port;
        }
        public bool Start()
        {
            try
            {
                MainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                uint IOC_IN = 0x80000000, IOC_VENDOR = 0x18000000, SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
                MainSocket.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);
                IPEndPoint Local = new IPEndPoint(IPAddress.Parse(Host), Port);
                State = new StateObject() { WorkSocket = MainSocket, LocalPoint = Local };
                MainSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                MainSocket.SetIPProtectionLevel(IPProtectionLevel.EdgeRestricted);
                MainSocket.Bind(Local);
                Thread ThreadMatch = new Thread(Read)
                {
                    Priority = ThreadPriority.Highest
                };
                ThreadMatch.Start();
                CLogger.Print($"Match Serv Address {Local.Address}:{Local.Port}", LoggerType.Info);
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
                MainSocket.BeginReceiveFrom(State.Buffer, 0, StateObject.BufferSize, SocketFlags.None, ref State.LocalPoint, new AsyncCallback(AcceptCallback), State);
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        private void AcceptCallback(IAsyncResult Result)
        {
            if (ServerIsClosed)
            {
                return;
            }
            if (!Result.IsCompleted)
            {
                CLogger.Print("IAsyncResult is not completed.", LoggerType.Warning);
            }
            Result.AsyncWaitHandle.WaitOne(5000);
            EndPoint Address = new IPEndPoint(IPAddress.Any, 0);
            Socket WorkSocket = (Result.AsyncState as StateObject).WorkSocket;
            EndPoint Local = (Result.AsyncState as StateObject).LocalPoint;
            try
            {
                int Received = WorkSocket.EndReceiveFrom(Result, ref Address);
                if (Received > StateObject.BufferSize)
                {
                    CLogger.Print($"Received Sized Is Over The Limit! (Size: {Received})", LoggerType.Warning);
                    return;
                }
                byte[] NewBuffer = new byte[Received];
                Buffer.BlockCopy(State.Buffer, 0, NewBuffer, 0, Received);
                if (NewBuffer.Length >= 22)
                {
                    MatchClient Client = new MatchClient(MainSocket, Address as IPEndPoint);
                    BeginReceive(Client, NewBuffer);
                    if (Client == null)
                    {
                        CLogger.Print("Destroyed after failed to receive the udp.", LoggerType.Warning);
                    }
                    Thread.Sleep(5);
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"Failed a Client Connection: {ex.Message}", LoggerType.Error, ex);
            }
            MainSocket.BeginReceiveFrom(State.Buffer, 0, StateObject.BufferSize, SocketFlags.None, ref State.LocalPoint, new AsyncCallback(AcceptCallback), new StateObject() { WorkSocket = WorkSocket, LocalPoint = Local });
        }
        public void BeginReceive(MatchClient Client, byte[] Buffer)
        {
            try
            {
                if (Client == null)
                {
                    return;
                }
                Client.BeginReceive(Buffer, DateTimeUtil.Now());
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        public void SendPacket(byte[] Data, IPEndPoint Address)
        {
            MainSocket.SendTo(Data, 0, Data.Length, SocketFlags.None, Address);
        }
    }
}