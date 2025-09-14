using Plugin.Core;
using Plugin.Core.Enums;
using Server.Auth.Data.Models;
using Server.Auth.Network.ServerPacket;
using System;

namespace Server.Auth.Network.ClientPacket
{
    public class PROTOCOL_AUTH_GET_POINT_CASH_REQ : AuthClientPacket
    {
        public PROTOCOL_AUTH_GET_POINT_CASH_REQ(AuthClient Client, byte[] Buffer)
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
                Client.SendPacket(new PROTOCOL_AUTH_GET_POINT_CASH_ACK(0, Player));

                //Load season pass
                //Client.SendPacket(new PROTOCOL_SEASON_CHALLENGE_SEASON_CHANGE());
                //Client.SendPacket(new PROTOCOL_SEASON_CHALLENGE_INFO_ACK());
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_AUTH_GET_POINT_CASH_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
