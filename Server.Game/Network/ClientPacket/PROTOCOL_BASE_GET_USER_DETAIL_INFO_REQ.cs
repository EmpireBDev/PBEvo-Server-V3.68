using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_GET_USER_DETAIL_INFO_REQ : GameClientPacket
    {
        private int SessionId;
        public PROTOCOL_BASE_GET_USER_DETAIL_INFO_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            SessionId = ReadD();
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
                PlayerSession Session = Player.GetChannel().GetPlayer(SessionId);
                if (Session != null)
                {
                    Client.SendPacket(new PROTOCOL_BASE_GET_USER_DETAIL_INFO_ACK(0, AccountManager.GetAccount(Session.PlayerId, true)));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_LOBBY_USER_STATISTICS_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}