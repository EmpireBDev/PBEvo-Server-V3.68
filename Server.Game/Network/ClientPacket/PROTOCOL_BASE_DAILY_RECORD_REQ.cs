using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_DAILY_RECORD_REQ : GameClientPacket
    {
        public PROTOCOL_BASE_DAILY_RECORD_REQ(GameClient Client, byte[] Buffer)
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
