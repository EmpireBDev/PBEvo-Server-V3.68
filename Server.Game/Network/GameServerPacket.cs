using Microsoft.Win32.SafeHandles;
using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Plugin.Core.Utility;
using System;
using System.IO;

namespace Server.Game.Network
{
    public abstract class GameServerPacket : BaseServerPacket, IDisposable
    {
        public GameServerPacket()
        {
            MStream = new MemoryStream();
            BWriter = new BinaryWriter(MStream);
            Handle = new SafeFileHandle(IntPtr.Zero, true);
            Disposed = false;
            SECURITY_KEY = Bitwise.CRYPTO[0];
            HASH_CODE = Bitwise.CRYPTO[1];
            SEED_LENGTH = Bitwise.CRYPTO[2];
            NATIONS = ConfigLoader.National;
        }
        public byte[] GetBytes(string Name)
        {
            try
            {
                Write();
                return MStream.ToArray();
            }
            catch (Exception Ex)
            {
                CLogger.Print($"GetBytes problem at: {Name}; {Ex.Message}", LoggerType.Error, Ex);
                return new byte[0];
            }
        }
        public byte[] GetCompleteBytes(string Name)
        {
            try
            {
                byte[] Data = GetBytes("GameServerPacket.GetCompleteBytes");
                if (Data.Length >= 2)
                {
                    byte[] List = new byte[Data.Length + 2];
                    byte[] Size = BitConverter.GetBytes(Convert.ToUInt16(Data.Length - 2));
                    Array.Copy(Size, 0, List, 0, Size.Length);
                    Array.Copy(Data, 0, List, Size.Length, Data.Length);
                    return List;
                }
                return new byte[0];
            }
            catch (Exception ex)
            {
                CLogger.Print($"GetCompleteBytes problem at: {Name}; {ex.Message}", LoggerType.Error, ex);
                return new byte[0];
            }
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
                BWriter.Dispose();
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
        public abstract void Write();
    }
}
