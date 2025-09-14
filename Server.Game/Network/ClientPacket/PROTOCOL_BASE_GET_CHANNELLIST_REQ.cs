using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.XML;
using Server.Game.Data.Models;
using Server.Game.Data.XML;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_GET_CHANNELLIST_REQ : GameClientPacket
    {
        private int ServerId;
        public PROTOCOL_BASE_GET_CHANNELLIST_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
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