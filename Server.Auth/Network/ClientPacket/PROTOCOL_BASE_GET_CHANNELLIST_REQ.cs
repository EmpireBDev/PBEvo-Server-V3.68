using Server.Auth.Network.ServerPacket;
using System;
using System.Collections.Generic;
using Server.Auth.Data.XML;
using Plugin.Core;
using Server.Auth.Data.Models;
using Plugin.Core.Enums;
using Plugin.Core.XML;

namespace Server.Auth.Network.ClientPacket
{
    public class PROTOCOL_BASE_GET_CHANNELLIST_REQ : AuthClientPacket
    {
        private int ServerId;
        public PROTOCOL_BASE_GET_CHANNELLIST_REQ(AuthClient Client, byte[] Buffer)
        {
            Makeme(Client, Buffer);
        }
        public override void Read()
        {
            ServerId = ReadD();
        }
        public override void Run()
        {
            try
            {
                List<ChannelModel> Channels = ChannelsXML.GetChannels(ServerId);
                if (Channels.Count == 11)
                {
                    Client.SendPacket(new PROTOCOL_BASE_GET_CHANNELLIST_ACK(SChannelXML.GetServer(ServerId), Channels));
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}
