using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_URL_LIST_REQ : GameClientPacket
    {
        public PROTOCOL_BASE_URL_LIST_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
        }
        public override void Run()
        {
            try
            {
                ServerConfig CFG = GameXender.Client.Config;
                if (CFG != null & CFG.OfficialBannerEnabled)
                {
                        Client.SendPacket(new PROTOCOL_BASE_URL_LIST_ACK(CFG));
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}
