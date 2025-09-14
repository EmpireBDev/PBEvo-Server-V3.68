using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Plugin.Core.Utility;
using Server.Auth.Data.Sync.Client;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server.Auth.Data.Sync
{
    public class AuthSync
    {
        protected UdpClient Client;
        public AuthSync(IPEndPoint Conn)
        {
            Client = new UdpClient(Conn);
        }
        public bool Start()
        {
            try
            {
                IPEndPoint EP = Client.Client.LocalEndPoint as IPEndPoint;
                uint IOC_IN = 0x80000000, IOC_VENDOR = 0x18000000, SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
                Client.Client.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);
                new Thread(Read).Start();
                CLogger.Print($"Auth Sync Address {EP.Address}:{EP.Port}", LoggerType.Info);
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
                Client.BeginReceive(new AsyncCallback(AcceptCallback), null);
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        private void AcceptCallback(IAsyncResult Result)
        {
            if (AuthXender.Client.ServerIsClosed)
            {
                return;
            }
            try
            {
                IPEndPoint Remote = new IPEndPoint(IPAddress.Any, 8000);
                byte[] Buffer = Client.EndReceive(Result, ref Remote);
                Thread.Sleep(5);
                new Thread(Read).Start();
                if (Buffer.Length >= 2)
                {
                    LoadPacket(Buffer);
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        private void LoadPacket(byte[] buffer)
        {
            try
            {
                SyncClientPacket C = new SyncClientPacket(buffer);
                short Opcode = C.ReadH();
                switch (Opcode)
                {
                    case 11: FriendInfo.Load(C); break;
                    case 13: AccountInfo.Load(C); break;
                    case 15: ServerCache.Load(C); break;
                    case 16: ClanSync.Load(C); break;
                    case 17: FriendSync.Load(C); break;
                    case 19: PlayerSync.Load(C); break;
                    case 20: ServerWarning.LoadGMWarning(C); break;
                    case 22: ServerWarning.LoadShopRestart(C); break;
                    case 23: ServerWarning.LoadServerUpdate(C); break;
                    case 24: ServerWarning.LoadShutdown(C); break;
                    case 31: EventInfo.LoadEventInfo(C); break;
                    case 32: ReloadConfig.Load(C); break;
                    case 33: ChannelCache.Load(C); break;
                    case 34: ReloadPermn.Load(C); break;
                    case 7171: ServerMessage.Load(C); break;
                    default: CLogger.Print(Bitwise.ToHexData($"Auth - Opcode Not Found: [{Opcode}]", C.ToArray()), LoggerType.Opcode); break;
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        public void SendPacket(byte[] Data, IPEndPoint Address)
        {
            Client.Send(Data, Data.Length, Address);
        }
        public void Close() => Client.Close();
    }
}