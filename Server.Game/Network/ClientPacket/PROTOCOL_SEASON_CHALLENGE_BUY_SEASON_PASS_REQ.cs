using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_SEASON_CHALLENGE_BUY_SEASON_PASS_REQ : GameClientPacket
    {
        //private uint Error;
        public PROTOCOL_SEASON_CHALLENGE_BUY_SEASON_PASS_REQ(GameClient Client, byte[] Buffer)
        {
            Makeme(Client, Buffer);
        }
        public override void Read()
        {
            //Error = 0x8000000;
            //Console.WriteLine("Season pass buy!");
        }
        public override void Run()
        {
            try
            {
                Client.SendPacket(new PROTOCOL_SEASON_CHALLENGE_BUY_SEASON_PASS_ACK(0));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_MATCH_CLAN_SEASON_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
