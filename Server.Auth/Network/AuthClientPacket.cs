using Microsoft.Win32.SafeHandles;
using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Plugin.Core.Utility;
using System;
using System.IO;

namespace Server.Auth.Network
{
    public abstract class AuthClientPacket : BaseClientPacket, IDisposable
    {
        public AuthClient Client;
        public AuthClientPacket()
        {
            Handle = new SafeFileHandle(IntPtr.Zero, true);
            Disposed = false;
            SECURITY_KEY = Bitwise.CRYPTO[0];
            HASH_CODE = Bitwise.CRYPTO[1];
            SEED_LENGTH = Bitwise.CRYPTO[2];
            NATIONS = ConfigLoader.National;
        }
        protected internal void Makeme(AuthClient Client, byte[] Buffer)
        {
            this.Client = Client;
            MStream = new MemoryStream(Buffer, 4, Buffer.Length - 4);
            BReader = new BinaryReader(MStream);
            Read();
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
                MStream.Dispose();
                BReader.Dispose();
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
        public abstract void Read();
        public abstract void Run();
    }
}