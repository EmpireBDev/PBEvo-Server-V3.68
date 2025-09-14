using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Auth.Data.Models;
using Server.Auth.Network.ServerPacket;
using System;

namespace Server.Auth.Network.ClientPacket
{
    public class PROTOCOL_BASE_DAILY_RECORD_REQ : AuthClientPacket
    {
        public PROTOCOL_BASE_DAILY_RECORD_REQ(AuthClient Client, byte[] Buffer)
        {
            Makeme(Client, Buffer);
        }
        public override void Read()
        {
        }
        public override void Run()
        {
            try
            {
                Account Player = Client.Player;
                if (Player == null)
                {
                    return;
                }
                StatDaily Dailies = Player.Statistic.Daily;
                if (Dailies != null)
                {
                    Client.SendPacket(new PROTOCOL_BASE_DAILY_RECORD_ACK(Dailies));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print("PROTOCOL_BASE_DAILY_RECORD_REQ: ", LoggerType.Error, ex);
            }
        }
    }
}
