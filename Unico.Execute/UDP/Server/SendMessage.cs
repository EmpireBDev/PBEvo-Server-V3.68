using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Plugin.Core.XML;
using System;
using System.Net;

namespace Executable.UDP.Server
{
    public class SendMessage
    {
        public static void FromServer(string Message)
        {
            try
            {
                foreach (SChannelModel Server in SChannelXML.Servers)
                {
                    if (Server == null)
                    {
                        return;
                    }
                    IPEndPoint Sync = SynchronizeXML.GetServer(Server.Port).Connection;
                    using (SyncServerPacket S = new SyncServerPacket())
                    {
                        S.WriteH(7171);
                        S.WriteC((byte)Message.Length);
                        S.WriteS(Message, Message.Length);
                        Communication.SendPacket(S.ToArray(), Sync);
                    }
                }
                //
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}
