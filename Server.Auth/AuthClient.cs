using Microsoft.Win32.SafeHandles;
using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Utility;
using Server.Auth.Data.Models;
using Server.Auth.Data.Sync.Server;
using Server.Auth.Network;
using Server.Auth.Network.ClientPacket;
using Server.Auth.Network.ServerPacket;
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace Server.Auth
{
    public class AuthClient : IDisposable
    {
        public int ServerId;
        public Socket Client;
        public Account Player;
        public DateTime SessionDate;
        public int SessionId;
        public ushort SessionSeed;
        private ushort NextSessionSeed;
        public int SessionShift, FirstPacketId;
        private bool Disposed = false, Closed = false;
        private readonly SafeHandle Handle = new SafeFileHandle(IntPtr.Zero, true);
        public AuthClient(int ServerId, Socket Client)
        {
            this.ServerId = ServerId;
            this.Client = Client;
            this.Client.DontFragment = false;
            this.Client.NoDelay = true;
        }
        public void Dispose()
        {
            try
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (Disposed)
                {
                    return;
                }
                Player = null;
                if (Client != null)
                {
                    Client.Dispose();
                    Client = null;
                }
                if (disposing)
                {
                    Handle.Dispose();
                }
                Disposed = true;
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        public void Start()
        {
            try
            {
                NextSessionSeed = SessionSeed;
                SessionShift = ((SessionId + Bitwise.CRYPTO[0]) % 7 + 1);
                new Thread(Connect).Start();
                new Thread(ReadPacket).Start();
                new Thread(ConnectionCheck).Start();
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
                Close(0, true);
            }
        }
        private void ConnectionCheck()
        {
            Thread.Sleep(10000);
            if (Client != null && FirstPacketId == 0)
            {
                CLogger.Print("Connection destroyed due to no responses.", LoggerType.Warning);
                Close(0, true);
            }
        }
        public string GetIPAddress()
        {
            try
            {
                if (Client != null && Client.RemoteEndPoint != null)
                {
                    return ((IPEndPoint)Client.RemoteEndPoint).Address.ToString();
                }
                return "";
            }
            catch
            {
                return "";
            }
        }
        public IPAddress GetAddress()
        {
            try
            {
                if (Client != null && Client.RemoteEndPoint != null)
                {
                    return ((IPEndPoint)Client.RemoteEndPoint).Address;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        private void Connect() => SendPacket(new PROTOCOL_BASE_CONNECT_ACK(this));
        public void SendCompletePacket(byte[] Data, string PacketName)
        {
            try
            {
                if (Data.Length < 4)
                {
                    return;
                }
                byte[] Logical = new byte[5];
                byte[] Result = new byte[Data.Length + 1 + Logical.Length];
                Array.Copy(Data, 0, Result, 0, Data.Length);
                for (int i = Data.Length; i < Result.Length; i++)
                {
                    Result[i] = 0;
                }
                if (ConfigLoader.DebugMode)
                {
                    ushort Opcode = BitConverter.ToUInt16(Result, 2);
                    string DebugData = Bitwise.ToByteString(Result);
                    CLogger.Print($"{PacketName}; Address: {Client.RemoteEndPoint}; Opcode: [{Opcode}]", LoggerType.Debug);
                    //CLogger.Print(ComDiv.ToHexData($"Server Packet: [{Opcode}] | Secondary", Result), LoggerType.Opcode);
                }
                if (Result.Length > 0)
                {
                    byte[] FinalResult = Result;
                    if (Logical.Length == 3)
                    {
                        FinalResult = Bitwise.Encrypt(Result, SessionShift);
                    }
                    Client.BeginSend(FinalResult, 0, FinalResult.Length, SocketFlags.None, new AsyncCallback(SendCallback), Client);
                }
                Result = null;
            }
            catch
            {
                Close(0, true);
            }
        }
        public void SendPacket(byte[] Data, string PacketName)
        {
            try
            {
                if (Data.Length < 2)
                {
                    return;
                }
                byte[] List = new byte[Data.Length + 2];
                byte[] Size = BitConverter.GetBytes(Convert.ToUInt16(Data.Length - 2));
                Array.Copy(Size, 0, List, 0, Size.Length);
                Array.Copy(Data, 0, List, Size.Length, Data.Length);
                byte[] Logical = new byte[5];
                byte[] Result = new byte[List.Length + 1 + Logical.Length];
                Array.Copy(List, 0, Result, 0, List.Length);
                for (int i = List.Length; i < Result.Length; i++)
                {
                    Result[i] = 0;
                }
                if (ConfigLoader.DebugMode)
                {
                    ushort Opcode = BitConverter.ToUInt16(Result, 2);
                    string DebugData = Bitwise.ToByteString(Result);
                    CLogger.Print($"{PacketName}; Address: {Client.RemoteEndPoint}; Opcode: [{Opcode}]", LoggerType.Debug);
                    //CLogger.Print(ComDiv.ToHexData($"Server Packet: [{Opcode}] | Secondary", Result), LoggerType.Opcode);
                }
                if (Result.Length > 0)
                {
                    byte[] FinalResult = Result;
                    if (Logical.Length == 3)
                    {
                        FinalResult = Bitwise.Encrypt(Result, SessionShift);
                    }
                    Client.BeginSend(FinalResult, 0, FinalResult.Length, SocketFlags.None, new AsyncCallback(SendCallback), Client);
                }
                Result = null;
            }
            catch
            {
                Close(0, true);
            }
        }
        public void SendPacket(AuthServerPacket Packet)
        {
            try
            {
                using (Packet)
                {
                    byte[] Data = Packet.GetBytes("AuthClient.SendPacket");
                    if (Data.Length < 2)
                    {
                        return;
                    }
                    byte[] List = new byte[Data.Length + 2];
                    byte[] Size = BitConverter.GetBytes(Convert.ToUInt16(Data.Length - 2));
                    Array.Copy(Size, 0, List, 0, Size.Length);
                    Array.Copy(Data, 0, List, Size.Length, Data.Length);
                    byte[] Logical = new byte[5];
                    byte[] Result = new byte[List.Length + 1 + Logical.Length];
                    Array.Copy(List, 0, Result, 0, List.Length);
                    for (int i = List.Length; i < Result.Length; i++)
                    {
                        Result[i] = 0;
                    }
                    if (ConfigLoader.DebugMode)
                    {
                        ushort Opcode = BitConverter.ToUInt16(Result, 2);
                        string DebugData = Bitwise.ToByteString(Result);
                        CLogger.Print($"{Packet.GetType().Name}; Address: {Client.RemoteEndPoint}; Opcode: [{Opcode}]", LoggerType.Debug);
                        //CLogger.Print(ComDiv.ToHexData($"Server Packet: [{Opcode}] | Primary", Result), LoggerType.Opcode);
                    }
                    if (Result.Length > 0)
                    {
                        byte[] FinalResult = Result;
                        if (Logical.Length == 3)
                        {
                            FinalResult = Bitwise.Encrypt(Result, SessionShift);
                        }
                        Client.BeginSend(FinalResult, 0, FinalResult.Length, SocketFlags.None, new AsyncCallback(SendCallback), Client);
                    }
                    Packet.Dispose();
                    Result = null;
                }
            }
            catch
            {
                Close(0, true);
            }
        }
        private void SendCallback(IAsyncResult Result)
        {
            try
            {
                Socket Handler = (Socket)Result.AsyncState;
                if (Handler != null && Handler.Connected)
                {
                    Handler.EndSend(Result);
                }
            }
            catch
            {
                Close(0, true);
            }
        }
        private void ReadPacket()
        {
            try
            {
                StateObject State = new StateObject()
                {
                    WorkSocket = Client
                };
                Client.BeginReceive(State.Buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(OnReceiveCallback), State);
            }
            catch
            {
                Close(0, true);
            }
        }
        public void Close(int TimeMS, bool DestroyConnection)
        {
            if (Closed)
            {
                return;
            }
            try
            {
                Closed = true;
                AuthXender.Client.RemoveSocket(this);
                Account Cache = Player;
                if (DestroyConnection)
                {
                    if (Cache != null)
                    {
                        Cache.SetOnlineStatus(false);
                        if (Cache.Status.ServerId == 0)
                        {
                            SendRefresh.RefreshAccount(Cache, false);
                        }
                        Cache.Status.ResetData(Cache.PlayerId);
                        Cache.SimpleClear();
                        Cache.UpdateCacheInfo();
                        Player = null;
                    }
                    if (Client != null)
                    {
                        Client.Close(TimeMS);
                    }
                    Thread.Sleep(TimeMS);
                    Dispose();
                }
                else if (Cache != null)
                {
                    Cache.SimpleClear();
                    Cache.UpdateCacheInfo();
                    Player = null;
                }
                UpdateServer.RefreshSChannel(ServerId);
            }
            catch (Exception ex)
            {
                CLogger.Print($"AuthClient.Close: {ex.Message}", LoggerType.Error, ex);
            }
        }
        private void OnReceiveCallback(IAsyncResult Result)
        {
            StateObject Packet = (StateObject)Result.AsyncState;
            try
            {
                int BytesCount = Packet.WorkSocket.EndReceive(Result);
                if (BytesCount > 0)
                {
                    byte[] EcryptedPacket = new byte[BytesCount];
                    Array.Copy(Packet.Buffer, 0, EcryptedPacket, 0, BytesCount);
                    int PacketLength = BitConverter.ToUInt16(EcryptedPacket, 0) & 0x7FFF;
                    byte[] BufferPacket = new byte[PacketLength];
                    Array.Copy(EcryptedPacket, 2, BufferPacket, 0, BufferPacket.Length);
                    byte[] DecryptedPacket = Bitwise.Decrypt(BufferPacket, SessionShift);
                    ushort PacketId = BitConverter.ToUInt16(DecryptedPacket, 0);
                    ushort PacketSeed = BitConverter.ToUInt16(DecryptedPacket, 2);
                    FirstPacketCheck(PacketId);
                    if (!CheckSeed(PacketSeed, true))
                    {
                        Close(0, true);
                        return;
                    }
                    if (Closed)
                    {
                        return;
                    }
                    RunPacket(PacketId, DecryptedPacket, "Primary");
                    CheckOut(EcryptedPacket, PacketLength);
                    new Thread(ReadPacket).Start();
                }
            }
            catch
            {
                Close(0, true);
            }
        }
        public void CheckOut(byte[] BufferTotal, int FirstLength)
        {
            int BufferLength = BufferTotal.Length;
            try
            {
                byte[] EcryptedPacket = new byte[BufferLength - FirstLength - 3];
                Array.Copy(BufferTotal, FirstLength + 3, EcryptedPacket, 0, EcryptedPacket.Length);
                if (EcryptedPacket.Length == 0)
                {
                    return;
                }
                int PacketLength = BitConverter.ToUInt16(EcryptedPacket, 0) & 0x7FFF;
                byte[] BufferPacket = new byte[PacketLength];
                Array.Copy(EcryptedPacket, 2, BufferPacket, 0, BufferPacket.Length);
                byte[] DecryptedPacket = Bitwise.Decrypt(BufferPacket, SessionShift);
                ushort PacketId = BitConverter.ToUInt16(DecryptedPacket, 0);
                ushort PacketSeed = BitConverter.ToUInt16(DecryptedPacket, 2);
                if (!CheckSeed(PacketSeed, false))
                {
                    Close(0, true);
                    return;
                }
                RunPacket(PacketId, DecryptedPacket, "Secondary");
                CheckOut(EcryptedPacket, PacketLength);
            }
            catch
            {
                Close(0, true);
            }
        }
        private void FirstPacketCheck(ushort PacketId)
        {
            if (FirstPacketId != 0)
            {
                return;
            }
            FirstPacketId = PacketId;
            if (PacketId != 257 && PacketId != 517)
            {
                CLogger.Print($"Connection destroyed due to unknown first packet. [{PacketId}]", LoggerType.Warning);
                Close(0, true);
            }
        }
        public bool CheckSeed(ushort PacketSeed, bool IsTheFirstPacket)
        {
            if (PacketSeed == GetNextSessionSeed())
            {
                return true;
            }
            CLogger.Print($"Connection blocked. IP: {GetIPAddress()} Date ({DateTimeUtil.Now()}) SessionId ({SessionId}) PacketSeed ({PacketSeed}) / NextSessionSeed ({NextSessionSeed}) PrimarySeed ({SessionSeed})", LoggerType.Hack);
            if (IsTheFirstPacket)
            {
                new Thread(ReadPacket).Start();
            }
            return false;
        }
        private ushort GetNextSessionSeed()
        {
            try
            {
                unchecked
                {
                    NextSessionSeed = (ushort)((((NextSessionSeed * 214013) + 2531011) >> 16) & short.MaxValue);
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
            return NextSessionSeed;
        }
        private void RunPacket(ushort Opcode, byte[] Buff, string Value)
        {
            try
            {
                AuthClientPacket Packet = null;
                switch (Opcode)
                {
                    case 257: Packet = new PROTOCOL_BASE_LOGIN_REQ(this, Buff); break;
                    case 515: Packet = new PROTOCOL_BASE_LOGOUT_REQ(this, Buff); break;
                    case 517: Packet = new PROTOCOL_BASE_UNKNOWN_PACKET_REQ(this, Buff); break;
                    case 520: Packet = new PROTOCOL_BASE_GAMEGUARD_REQ(this, Buff); break;
                    case 522: Packet = new PROTOCOL_BASE_GET_SYSTEM_INFO_REQ(this, Buff); break;
                    case 524: Packet = new PROTOCOL_BASE_GET_USER_INFO_REQ(this, Buff); break;
                    case 526: Packet = new PROTOCOL_BASE_GET_INVEN_INFO_REQ(this, Buff); break;
                    case 528: Packet = new PROTOCOL_BASE_GET_OPTION_REQ(this, Buff); break;
                    case 530: Packet = new PROTOCOL_BASE_OPTION_SAVE_REQ(this, Buff); break;
                    case 536: Packet = new PROTOCOL_BASE_USER_LEAVE_REQ(this, Buff); break;
                    case 540: Packet = new PROTOCOL_BASE_GET_CHANNELLIST_REQ(this, Buff); break;
                    case 607: Packet = new PROTOCOL_BASE_GAME_SERVER_STATE_REQ(this, Buff); break;
                    case 622: Packet = new PROTOCOL_BASE_DAILY_RECORD_REQ(this, Buff); break;
                    case 667: Packet = new PROTOCOL_BASE_GET_MAP_INFO_REQ(this, Buff); break;
                    case 697: Packet = new PROTOCOL_BASE_CHANNELTYPE_CHANGE_CONDITION_REQ(this, Buff); break;
                    case 1057: Packet = new PROTOCOL_AUTH_GET_POINT_CASH_REQ(this, Buff); break;
                    case 7681: Packet = new PROTOCOL_MATCH_SERVER_IDX_REQ(this, Buff); break;
                    case 7699: Packet = new PROTOCOL_MATCH_CLAN_SEASON_REQ(this, Buff); break;
                    default: CLogger.Print(Bitwise.ToHexData($"Opcode Not Found: [{Opcode}] | {Value}", Buff), LoggerType.Opcode); break;
                }
                if (Packet != null)
                {
                    if (ConfigLoader.DebugMode)
                    {
                        CLogger.Print($"{Packet.GetType().Name}; Address: {Client.RemoteEndPoint}; Opcode: [{Opcode}]", LoggerType.Debug);
                        //CLogger.Print(ComDiv.ToHexData($"Client Packet: [{Opcode}] | {Value}", Buff), LoggerType.Opcode);
                    }
                    new Thread(Packet.Run).Start();
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}
