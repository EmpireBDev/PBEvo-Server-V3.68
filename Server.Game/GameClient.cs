using Microsoft.Win32.SafeHandles;
using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using Server.Game.Data.Sync.Server;
using Server.Game.Data.Utils;
using Server.Game.Network;
using Server.Game.Network.ClientPacket;
using Server.Game.Network.ServerPacket;
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace Server.Game
{
    public class GameClient : IDisposable
    {
        public int ServerId;
        public long PlayerId;
        public Socket Client;
        public Account Player;
        public DateTime SessionDate;
        public int SessionId;
        public ushort SessionSeed;
        private ushort NextSessionSeed;
        public int SessionShift, FirstPacketId;
        private bool Disposed = false, Closed = false;
        private readonly SafeHandle Handle = new SafeFileHandle(IntPtr.Zero, true);
        public GameClient(int ServerId, Socket Client)
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
                PlayerId = 0;
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
        public void SendPacket(GameServerPacket Packet)
        {
            try
            {
                using (Packet)
                {
                    byte[] Data = Packet.GetBytes("GameClient.SendPacket");
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
                    Checkout(EcryptedPacket, PacketLength);
                    new Thread(ReadPacket).Start();
                }
            }
            catch
            {
                Close(0, true);
            }
        }
        public void Checkout(byte[] BufferTotal, int FirstLength)
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
                Checkout(EcryptedPacket, PacketLength);
            }
            catch
            {
                Close(0, true);
            }
        }
        public void Close(int TimeMS, bool DestroyConnection, bool Kicked = false)
        {
            if (Closed)
            {
                return;
            }
            try
            {
                Closed = true;
                GameXender.Client.RemoveSocket(this);
                Account Cache = Player;
                if (DestroyConnection)
                {
                    if (PlayerId > 0 && Cache != null)
                    {
                        Cache.SetOnlineStatus(false);
                        RoomModel Room = Cache.Room;
                        if (Room != null)
                        {
                            Room.RemovePlayer(Cache, false, Kicked ? 1 : 0);
                        }
                        MatchModel Match = Cache.Match;
                        if (Match != null)
                        {
                            Match.RemovePlayer(Cache);
                        }
                        ChannelModel Channel = Cache.GetChannel();
                        if (Channel != null)
                        {
                            Channel.RemovePlayer(Cache);
                        }
                        Cache.Status.ResetData(PlayerId);
                        AllUtils.SyncPlayerToFriends(Cache, false);
                        AllUtils.SyncPlayerToClanMembers(Cache);
                        Cache.SimpleClear();
                        Cache.UpdateCacheInfo();
                        Player = null;
                    }
                    PlayerId = 0;
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
                CLogger.Print("GameClient.Close: " + ex.Message, LoggerType.Error, ex);
            }
        }
        private void FirstPacketCheck(ushort PacketId)
        {
            if (FirstPacketId != 0)
            {
                return;
            }
            FirstPacketId = PacketId;
            if (PacketId != 538 && PacketId != 517)
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
                GameClientPacket Packet = null;
                switch (Opcode)
                {
                    case 515: Packet = new PROTOCOL_BASE_LOGOUT_REQ(this, Buff); break;
                    case 517: Packet = new PROTOCOL_BASE_UNKNOWN_PACKET_REQ(this, Buff); break;
                    case 520: Packet = new PROTOCOL_BASE_GAMEGUARD_REQ(this, Buff); break;
                    case 530: Packet = new PROTOCOL_BASE_OPTION_SAVE_REQ(this, Buff); break;
                    case 534: Packet = new PROTOCOL_BASE_CREATE_NICK_REQ(this, Buff); break;
                    case 536: Packet = new PROTOCOL_BASE_USER_LEAVE_REQ(this, Buff); break;
                    case 538: Packet = new PROTOCOL_BASE_USER_ENTER_REQ(this, Buff); break;
                    case 540: Packet = new PROTOCOL_BASE_GET_CHANNELLIST_REQ(this, Buff); break;
                    case 542: Packet = new PROTOCOL_BASE_SELECT_CHANNEL_REQ(this, Buff); break;
                    case 544: Packet = new PROTOCOL_BASE_ATTENDANCE_REQ(this, Buff); break;
                    case 546: Packet = new PROTOCOL_BASE_ATTENDANCE_CLEAR_ITEM_REQ(this, Buff); break;
                    case 548: Packet = new PROTOCOL_BASE_GUIDE_COMPLETE_REQ(this, Buff); break;
                    case 568: Packet = new PROTOCOL_BASE_QUEST_ACTIVE_IDX_CHANGE_REQ(this, Buff); break;
                    case 572: Packet = new PROTOCOL_BASE_QUEST_BUY_CARD_SET_REQ(this, Buff); break;
                    case 574: Packet = new PROTOCOL_BASE_QUEST_DELETE_CARD_SET_REQ(this, Buff); break;
                    case 584: Packet = new PROTOCOL_BASE_USER_TITLE_CHANGE_REQ(this, Buff); break;
                    case 586: Packet = new PROTOCOL_BASE_USER_TITLE_EQUIP_REQ(this, Buff); break;
                    case 588: Packet = new PROTOCOL_BASE_USER_TITLE_RELEASE_REQ(this, Buff); break;
                    case 592: Packet = new PROTOCOL_BASE_CHATTING_REQ(this, Buff); break;
                    case 600: Packet = new PROTOCOL_BASE_MISSION_SUCCESS_REQ(this, Buff); break;
                    case 607: Packet = new PROTOCOL_BASE_GAME_SERVER_STATE_REQ(this, Buff); break;
                    case 622: Packet = new PROTOCOL_BASE_DAILY_RECORD_REQ(this, Buff); break;
                    case 630: Packet = new PROTOCOL_BASE_GET_USER_DETAIL_INFO_REQ(this, Buff); break;
                    case 632: Packet = new PROTOCOL_BASE_GET_ROOM_USER_DETAIL_INFO_REQ(this, Buff); break;
                    case 633: Packet = new PROTOCOL_BASE_GET_USER_BASIC_INFO_REQ(this, Buff); break;
                    case 634: Packet = new PROTOCOL_AUTH_FIND_USER_REQ(this, Buff); break;
                    case 655: Packet = new PROTOCOL_BASE_GET_USER_SUBTASK_REQ(this, Buff); break;
                    case 673: Packet = new PROTOCOL_BASE_URL_LIST_REQ(this, Buff); break;
                    case 695: Packet = new PROTOCOL_BASE_ENTER_PASS_REQ(this, Buff); break;
                    case 792: Packet = new PROTOCOL_AUTH_FRIEND_ACCEPT_REQ(this, Buff); break;
                    case 794: Packet = new PROTOCOL_AUTH_FRIEND_INSERT_REQ(this, Buff); break;
                    case 897: Packet = new PROTOCOL_MESSENGER_NOTE_SEND_REQ(this, Buff); break;
                    case 902: Packet = new PROTOCOL_MESSENGER_NOTE_CHECK_READED_REQ(this, Buff); break;
                    case 904: Packet = new PROTOCOL_MESSENGER_NOTE_DELETE_REQ(this, Buff); break;
                    case 906: Packet = new PROTOCOL_MESSENGER_NOTE_RECEIVE_REQ(this, Buff); break;
                    case 697: Packet = new PROTOCOL_BASE_CHANNELTYPE_CHANGE_CONDITION_REQ(this, Buff); break;
                    case 699: Packet = new PROTOCOL_LOBBY_NEW_MYINFO_REQ(this, Buff); break;
                    case 706: Packet = new PROTOCOL_SHOP_GET_SAILLIST_REQ(this, Buff); break; //Packet = new PROTOCOL_BASE_RANDOMBOX_LIST_REQ(this, Buff); break;
                    case 716: Packet = new PROTOCOL_BASE_TICKET_UPDATE_REQ(this, Buff); break;
                    case 718: Packet = new PROTOCOL_BASE_EVENT_PORTAL_REQ(this, Buff); break;
                    case 787: Packet = new PROTOCOL_AUTH_FRIEND_INVITED_REQ(this, Buff); break;
                    case 796: Packet = new PROTOCOL_AUTH_FRIEND_DELETE_REQ(this, Buff); break;
                    case 1025: Packet = new PROTOCOL_SHOP_ENTER_REQ(this, Buff); break;
                    case 1027: Packet = new PROTOCOL_SHOP_LEAVE_REQ(this, Buff); break;
                    case 1029: Packet = new PROTOCOL_SHOP_GET_SAILLIST_REQ(this, Buff); break;
                    case 1041: Packet = new PROTOCOL_AUTH_SHOP_GET_GIFTLIST_REQ(this, Buff); break;
                    case 1043: Packet = new PROTOCOL_AUTH_SHOP_GOODS_BUY_REQ(this, Buff); break;
                    case 1045: Packet = new PROTOCOL_AUTH_SHOP_GOODS_GIFT_REQ(this, Buff); break;
                    case 1047: Packet = new PROTOCOL_AUTH_SHOP_ITEM_AUTH_REQ(this, Buff); break;
                    case 1049: Packet = new PROTOCOL_INVENTORY_USE_ITEM_REQ(this, Buff); break;
                    case 1053: Packet = new PROTOCOL_AUTH_SHOP_AUTH_GIFT_REQ(this, Buff); break;
                    case 1055: Packet = new PROTOCOL_AUTH_SHOP_DELETE_ITEM_REQ(this, Buff); break;
                    case 1057: Packet = new PROTOCOL_AUTH_GET_POINT_CASH_REQ(this, Buff); break;
                    case 1061: Packet = new PROTOCOL_AUTH_USE_ITEM_CHECK_NICK_REQ(this, Buff); break;
                    case 1076: Packet = new PROTOCOL_SHOP_REPAIR_REQ(this, Buff); break;
                    case 1084: Packet = new PROTOCOL_AUTH_SHOP_USE_GIFTCOUPON_REQ(this, Buff); break;
                    case 1087: Packet = new PROTOCOL_AUTH_SHOP_ITEM_CHANGE_DATA_REQ(this, Buff); break;
                    case 1793: Packet = new PROTOCOL_CS_CLIENT_ENTER_REQ(this, Buff); break;
                    case 1795: Packet = new PROTOCOL_CS_CLIENT_LEAVE_REQ(this, Buff); break;
                    case 1824: Packet = new PROTOCOL_CS_DETAIL_INFO_REQ(this, Buff); break;
                    case 1826: Packet = new PROTOCOL_CS_MEMBER_CONTEXT_REQ(this, Buff); break;
                    case 1828: Packet = new PROTOCOL_CS_MEMBER_LIST_REQ(this, Buff); break;
                    case 1830: Packet = new PROTOCOL_CS_CREATE_CLAN_REQ(this, Buff); break;
                    case 1832: Packet = new PROTOCOL_CS_CLOSE_CLAN_REQ(this, Buff); break;
                    case 1834: Packet = new PROTOCOL_CS_CHECK_JOIN_AUTHORITY_ERQ(this, Buff); break;
                    case 1836: Packet = new PROTOCOL_CS_JOIN_REQUEST_REQ(this, Buff); break;
                    case 1838: Packet = new PROTOCOL_CS_CANCEL_REQUEST_REQ(this, Buff); break;
                    case 1840: Packet = new PROTOCOL_CS_REQUEST_CONTEXT_REQ(this, Buff); break;
                    case 1842: Packet = new PROTOCOL_CS_REQUEST_LIST_REQ(this, Buff); break;
                    case 1844: Packet = new PROTOCOL_CS_REQUEST_INFO_REQ(this, Buff); break;
                    case 1846: Packet = new PROTOCOL_CS_ACCEPT_REQUEST_REQ(this, Buff); break;
                    case 1849: Packet = new PROTOCOL_CS_DENIAL_REQUEST_REQ(this, Buff); break;
                    case 1852: Packet = new PROTOCOL_CS_SECESSION_CLAN_REQ(this, Buff); break;
                    case 1878: Packet = new PROTOCOL_CS_CHATTING_REQ(this, Buff); break;
                    case 1880: Packet = new PROTOCOL_CS_CHECK_MARK_REQ(this, Buff); break;
                    case 1912: Packet = new PROTOCOL_CS_INVITE_REQ(this, Buff); break;
                    case 1914: Packet = new PROTOCOL_CS_INVITE_ACCEPT_REQ(this, Buff); break;
                    case 1938: Packet = new PROTOCOL_CS_CREATE_CLAN_CONDITION_REQ(this, Buff); break;
                    case 1940: Packet = new PROTOCOL_CS_CHECK_DUPLICATE_REQ(this, Buff); break;
                    case 2021: Packet = new PROTOCOL_CS_FILTER_CLAN_LIST_REQ(this, Buff); break;
                    case 2023: Packet = new PROTOCOL_CS_SIMPLE_CLAN_INFO_REQ(this, Buff); break;
                    case 2826: Packet = new PROTOCOL_COMMUNITY_USER_REPORT_REQ(this, Buff); break;
                    case 2828: Packet = new PROTOCOL_COMMUNITY_USER_REPORT_CONDITION_CHECK_REQ(this, Buff); break;
                    case 3073: Packet = new PROTOCOL_LOBBY_ENTER_REQ(this, Buff); break;
                    case 3075: Packet = new PROTOCOL_LOBBY_LEAVE_REQ(this, Buff); break;
                    case 3077: Packet = new PROTOCOL_LOBBY_GET_ROOMLIST_REQ(this, Buff); break;
                    case 3083: Packet = new PROTOCOL_LOBBY_GET_ROOMINFOADD_REQ(this, Buff); break;
                    case 3329: Packet = new PROTOCOL_INVENTORY_ENTER_REQ(this, Buff); break;
                    case 3331: Packet = new PROTOCOL_INVENTORY_LEAVE_REQ(this, Buff); break;
                    case 3841: Packet = new PROTOCOL_ROOM_CREATE_REQ(this, Buff); break;
                    case 3843: Packet = new PROTOCOL_ROOM_JOIN_REQ(this, Buff); break;
                    case 3852: Packet = new PROTOCOL_ROOM_GET_PLAYERINFO_REQ(this, Buff); break;
                    case 3858: Packet = new PROTOCOL_ROOM_CHANGE_PASSWD_REQ(this, Buff); break;
                    case 3860: Packet = new PROTOCOL_ROOM_CHANGE_SLOT_REQ(this, Buff); break;
                    case 3865: Packet = new PROTOCOL_ROOM_PERSONAL_TEAM_CHANGE_REQ(this, Buff); break;
                    case 3875: Packet = new PROTOCOL_ROOM_REQUEST_MAIN_REQ(this, Buff); break;
                    case 3877: Packet = new PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_REQ(this, Buff); break;
                    case 3879: Packet = new PROTOCOL_ROOM_REQUEST_MAIN_CHANGE_WHO_REQ(this, Buff); break;
                    case 3881: Packet = new PROTOCOL_ROOM_CHECK_MAIN_REQ(this, Buff); break;
                    case 3883: Packet = new PROTOCOL_ROOM_TOTAL_TEAM_CHANGE_REQ(this, Buff); break;
                    case 3895: Packet = new PROTOCOL_ROOM_CHANGE_ROOMINFO_REQ(this, Buff); break;
                    case 3913: Packet = new PROTOCOL_ROOM_LOADING_START_REQ(this, Buff); break;
                    case 3921: Packet = new PROTOCOL_ROOM_GET_USER_EQUIPMENT_REQ(this, Buff); break;
                    case 3927: Packet = new PROTOCOL_ROOM_INFO_ENTER_REQ(this, Buff); break;
                    case 3929: Packet = new PROTOCOL_ROOM_INFO_LEAVE_REQ(this, Buff); break;
                    case 3931: Packet = new PROTOCOL_ROOM_INVITE_LOBBY_USER_LIST_REQ(this, Buff); break;
                    case 3933: Packet = new PROTOCOL_ROOM_CHANGE_COSTUME_REQ(this, Buff); break;
                    case 3935: Packet = new PROTOCOL_ROOM_SELECT_SLOT_CHANGE_REQ(this, Buff); break;
                    case 3936: Packet = new PROTOCOL_ROOM_GET_ACEMODE_PLAYERINFO_REQ(this, Buff); break;
                    case 3938: Packet = new PROTOCOL_ROOM_SELECT_SLOT_CHANGE_REQ(this, Buff); break;
                    case 4099: Packet = new PROTOCOL_BATTLE_READYBATTLE_REQ(this, Buff); break;
                    case 4105: Packet = new PROTOCOL_BATTLE_PRESTARTBATTLE_REQ(this, Buff); break;
                    case 4107: Packet = new PROTOCOL_BATTLE_STARTBATTLE_REQ(this, Buff); break;
                    case 4109: Packet = new PROTOCOL_BATTLE_GIVEUPBATTLE_REQ(this, Buff); break;
                    case 4111: Packet = new PROTOCOL_BATTLE_DEATH_REQ(this, Buff); break;
                    case 4113: Packet = new PROTOCOL_BATTLE_RESPAWN_REQ(this, Buff); break;
                    case 4121: Packet = new PROTOCOL_BASE_UNKNOWN_PACKET_REQ(this, Buff); break;
                    case 4122: Packet = new PROTOCOL_BATTLE_SENDPING_REQ(this, Buff); break;
                    case 4132: Packet = new PROTOCOL_BATTLE_MISSION_BOMB_INSTALL_REQ(this, Buff); break;
                    case 4134: Packet = new PROTOCOL_BATTLE_MISSION_BOMB_UNINSTALL_REQ(this, Buff); break;
                    case 4142: Packet = new PROTOCOL_BATTLE_MISSION_GENERATOR_INFO_REQ(this, Buff); break;
                    case 4144: Packet = new PROTOCOL_BATTLE_TIMERSYNC_REQ(this, Buff); break;
                    case 4148: Packet = new PROTOCOL_BATTLE_CHANGE_DIFFICULTY_LEVEL_REQ(this, Buff); break;
                    case 4150: Packet = new PROTOCOL_BATTLE_RESPAWN_FOR_AI_REQ(this, Buff); break;
                    case 4156: Packet = new PROTOCOL_BATTLE_MISSION_DEFENCE_INFO_REQ(this, Buff); break;
                    case 4158: Packet = new PROTOCOL_BATTLE_MISSION_TOUCHDOWN_COUNT_REQ(this, Buff); break;
                    case 4164: Packet = new PROTOCOL_BATTLE_MISSION_TUTORIAL_ROUND_END_REQ(this, Buff); break;
                    case 4238: Packet = new PROTOCOL_BATTLE_NEW_JOIN_ROOM_SCORE_REQ(this, Buff); break;
                    case 4252: Packet = new PROTOCOL_BATTLE_USER_SOPETYPE_REQ(this, Buff); break;
                    case 4609: Packet = new PROTOCOL_BASE_UNKNOWN_PACKET_REQ(this, Buff); break;
                    case 5377: Packet = new PROTOCOL_LOBBY_QUICKJOIN_ROOM_REQ(this, Buff); break;
                    case 6145: Packet = new PROTOCOL_CHAR_CREATE_CHARA_REQ(this, Buff); break;
                    case 6149: Packet = new PROTOCOL_CHAR_CHANGE_EQUIP_REQ(this, Buff); break;
                    case 6151: Packet = new PROTOCOL_CHAR_DELETE_CHARA_REQ(this, Buff); break;
                    case 6657: Packet = new PROTOCOL_GMCHAT_START_CHAT_REQ(this, Buff); break;
                    //case 6659: Packet = new PROTOCOL_GMCHAT_SEND_CHAT_REQ(this, Buff); break;
                    case 6661: Packet = new PROTOCOL_GMCHAT_END_CHAT_REQ(this, Buff); break;
                    case 6663: Packet = new PROTOCOL_GMCHAT_APPLY_PENALTY_REQ(this, Buff); break;
                    //case 6665: Packet = new PROTOCOL_GMCHAT_NOTI_USER_PENALTY_REQ(this, Buff); break;
                    case 7681: Packet = new PROTOCOL_MATCH_SERVER_IDX_REQ(this, Buff); break;
                    case 7699: Packet = new PROTOCOL_MATCH_CLAN_SEASON_REQ(this, Buff); break;
                    case 8453: Packet = new PROTOCOL_SEASON_CHALLENGE_BUY_SEASON_PASS_REQ(this, Buff); break;

                    /*
                    case 672: Packet = new PROTOCOL_BASE_UNKNOWN_PACKET_REQ(this, Buff); break; 
                    case 685: Packet = new PROTOCOL_BASE_UNKNOWN_PACKET_REQ(this, Buff); break; 
                    case 1553: Packet = new PROTOCOL_CLAN_WAR_MATCH_PROPOSE_REQ(this, Buff); break; 
                    case 1558: Packet = new PROTOCOL_CLAN_WAR_INVITE_ACCEPT_REQ(this, Buff); break; 
                    case 1565: Packet = new PROTOCOL_CLAN_WAR_CREATE_ROOM_REQ(this, Buff); break; 
                    case 1567: Packet = new PROTOCOL_CLAN_WAR_JOIN_ROOM_REQ(this, Buff); break; 
                    case 1569: Packet = new PROTOCOL_CLAN_WAR_MATCH_TEAM_INFO_REQ(this, Buff); break; 
                    case 1571: Packet = new PROTOCOL_CLAN_WAR_INVITE_MERCENARY_RECEIVER_REQ(this, Buff); break; 
                    case 1799: Packet = new PROTOCOL_CS_CLIENT_CLAN_CONTEXT_REQ(this, Buff); break; 
                    
                    case 1854: Packet = new PROTOCOL_CS_DEPORTATION_REQ(this, Buff); break; 
                    case 1857: Packet = new PROTOCOL_CS_COMMISSION_MASTER_REQ(this, Buff); break; 
                    case 1860: Packet = new PROTOCOL_CS_COMMISSION_STAFF_REQ(this, Buff); break; 
                    case 1863: Packet = new PROTOCOL_CS_COMMISSION_REGULAR_REQ(this, Buff); break; 

                    case 1882: Packet = new PROTOCOL_CS_REPLACE_NOTICE_REQ(this, Buff); break; 
                    case 1884: Packet = new PROTOCOL_CS_REPLACE_INTRO_REQ(this, Buff); break; 
                    case 1892: Packet = new PROTOCOL_CS_REPLACE_MANAGEMENT_REQ(this, Buff); break; 
                    case 1901: Packet = new PROTOCOL_CS_ROOM_INVITED_REQ(this, Buff); break; 
                    case 1910: Packet = new PROTOCOL_CS_PAGE_CHATTING_REQ(this, Buff); break; 
                    
                  
                    case 1916: Packet = new PROTOCOL_CS_NOTE_REQ(this, Buff); break; 
                    
                    case 1946: Packet = new PROTOCOL_CS_CLAN_LIST_DETAIL_INFO_REQ(this, Buff); break; 
                    case 1954: Packet = new PROTOCOL_CS_CLAN_MATCH_RESULT_CONTEXT_REQ(this, Buff); break; 
                    case 1956: Packet = new PROTOCOL_CS_CLAN_MATCH_RESULT_LIST_REQ(this, Buff); break;
                    case 789: Packet = new PROTOCOL_AUTH_FRIEND_INVITED_REQUEST_REQ(this, Buff); break;

                    case 997: Packet = new PROTOCOL_CS_CLIENT_CLAN_LIST_REQ(this, Buff); break;
                    case 1080: Packet = new PROTOCOL_AUTH_GIFTSHOP_ENTER_REQ(this, Buff); break;
                    case 1082: Packet = new PROTOCOL_AUTH_GIFTSHOP_LEAVE_REQ(this, Buff); break;
                    case 1826: Packet = new PROTOCOL_AUTH_SEND_WHISPER_REQ(this, Buff); break;
                    case 1829: Packet = new PROTOCOL_AUTH_RECV_WHISPER_REQ(this, Buff); break;
                    case 2350: Packet = new PROTOCOL_BASE_GET_RECORD_INFO_DB_REQ(this, Buff); break;
                    
                    case 2422: Packet = new PROTOCOL_LOBBY_NEW_VIEW_USER_ITEM_REQ(this, Buff); break;
                    case 2447: Packet = new PROTOCOL_BASE_GET_USER_SUBTASK_REQ(this, Buff); break;
                    case 2498: Packet = new PROTOCOL_SHOP_GET_SAILLIST_REQ(this, Buff); break;
                    
                    case 6657: Packet = new PROTOCOL_BASE_GET_USER_MANAGEMENT_POPUP_REQ(this, Buff); break;
                    case 3095: Packet = new PROTOCOL_BASE_SELECT_AGE_REQ(this, Buff); break; 
                    case 4166: Packet = new PROTOCOL_BATTLE_START_KICKVOTE_REQ(this, Buff); break; 
                    case 4170: Packet = new PROTOCOL_BATTLE_NOTIFY_CURRENT_KICKVOTE_REQ(this, Buff); break; 
                    case 3867: Packet = new PROTOCOL_ROOM_GET_LOBBY_USER_LIST_REQ(this, Buff); break; 
                    case 3869: Packet = new PROTOCOL_ROOM_INVITE_SELECTED_LOBBY_USER_REQ(this, Buff); break; 
                    case 3910: Packet = new PROTOCOL_BASE_PLAYTIME_REWARD_REQ(this, Buff); break; 
                    case 4097: Packet = new PROTOCOL_BATTLE_HOLE_CHECK_REQ(this, Buff); break; 
                    case 4099: Packet = new PROTOCOL_BATTLE_READYBATTLE_REQ(this, Buff); break; 
                    case 4119: Packet = new PROTOCOL_BATTLE_TIMEOUTCLIENT_REQ(this, Buff); break; 
                    case 5127: Packet = new PROTOCOL_GACHA_ENTER_REQ(this, Buff); break; 
                    case 5129: Packet = new PROTOCOL_GACHA_LEAVE_REQ(this, Buff); break; 
                    case 6914: Packet = new PROTOCOL_CLAN_WAR_MATCH_TEAM_COUNT_REQ(this, Buff); break; 
                    case 6916: Packet = new PROTOCOL_CLAN_WAR_MATCH_TEAM_LIST_REQ(this, Buff); break; 
                    case 6918: Packet = new PROTOCOL_CLAN_WAR_CREATE_TEAM_REQ(this, Buff); break; 
                    case 6920: Packet = new PROTOCOL_CLAN_WAR_JOIN_TEAM_REQ(this, Buff); break; 
                    case 6922: Packet = new PROTOCOL_CLAN_WAR_LEAVE_TEAM_REQ(this, Buff); break; 
                    case 6934: Packet = new PROTOCOL_CLAN_WAR_CANCEL_MATCHMAKING_REQ(this, Buff); break; 
                    case 6940: Packet = new PROTOCOL_CLAN_WAR_REMOVE_MERCENARY_REQ(this, Buff); break; 
                    case 6963: Packet = new PROTOCOL_CLAN_WAR_RESULT_REQ(this, Buff); break; 
                    */
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
