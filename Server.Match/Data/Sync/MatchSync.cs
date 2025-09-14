using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Plugin.Core.Utility;
using Server.Match.Data.Sync.Client;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server.Match.Data.Sync
{
    public class MatchSync
    {
        protected UdpClient Client;
        public MatchSync(IPEndPoint Conn)
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
                CLogger.Print($"Match Sync Address {EP.Address}:{EP.Port}", LoggerType.Info);
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
            if (MatchXender.Client.ServerIsClosed)
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
        private void LoadPacket(byte[] Buffer)
        {
            try
            {
                SyncClientPacket C = new SyncClientPacket(Buffer);
                short Opcode = C.ReadH();
                switch (Opcode)
                {
                    case 1: RespawnSync.Load(C); break;
                    case 2: RemovePlayerSync.Load(C); break;
                    case 3: MatchRoundSync.Load(C); break;
                    default: CLogger.Print(Bitwise.ToHexData($"Match - Opcode Not Found: [{Opcode}]", C.ToArray()), LoggerType.Opcode); break;
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        public void SendPacket(byte[] Data, IPEndPoint Address)
        {
            if (ConfigLoader.SendInfoToServ)
            {
                Client.Send(Data, Data.Length, Address);
            }
        }
        public void Close() => Client.Close();
    }
}