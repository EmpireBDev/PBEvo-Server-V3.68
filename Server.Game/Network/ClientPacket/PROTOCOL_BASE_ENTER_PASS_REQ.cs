using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using Server.Game.Data.XML;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_ENTER_PASS_REQ : GameClientPacket
    {
        private int ChannelId;
        private string Password;
        public PROTOCOL_BASE_ENTER_PASS_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            ChannelId = ReadH();
            Password = ReadS(16);
        }
        public override void Run()
        {
            try
            {
                Account Player = Client.Player;
                if (Player == null || Player.ChannelId >= 0)
                {
                    return;
                }
                ChannelModel Channel = ChannelsXML.GetChannel(Client.ServerId, ChannelId);
                if (Channel != null)
                {
                    if (!Password.Equals(Channel.Password))
                    {
                        Client.SendPacket(new PROTOCOL_BASE_ENTER_PASS_ACK(0x80000000));
                    }
                    else
                    {
                        Client.SendPacket(new PROTOCOL_BASE_ENTER_PASS_ACK(0));
                    }
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}
