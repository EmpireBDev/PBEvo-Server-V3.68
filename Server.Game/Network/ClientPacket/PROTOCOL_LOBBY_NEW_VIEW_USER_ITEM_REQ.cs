using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_LOBBY_NEW_VIEW_USER_ITEM_REQ : GameClientPacket
    {
        private int sessionId;
        public PROTOCOL_LOBBY_NEW_VIEW_USER_ITEM_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            sessionId = ReadD();
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
                PlayerSession Session = Player.GetChannel().GetPlayer(sessionId);
                if (Session != null)
                {
                    Client.SendPacket(new PROTOCOL_LOBBY_NEW_VIEW_USER_ITEM_ACK(Session));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_LOBBY_NEW_VIEW_USER_ITEM_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}