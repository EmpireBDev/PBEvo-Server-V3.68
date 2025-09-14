using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_GM_LOG_LOBBY_REQ : GameClientPacket
    {
        private int SessionId;
        public PROTOCOL_GM_LOG_LOBBY_REQ(GameClient client, byte[] data)
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
                if (Player == null || !Player.IsGM())
                {
                    return;
                }
                long TargetPlayerId = Player.GetChannel().GetPlayer(SessionId).PlayerId;
                if (TargetPlayerId > 0)
                {
                    Client.SendPacket(new PROTOCOL_GM_LOG_LOBBY_ACK(0, TargetPlayerId));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_GM_LOG_LOBBY_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}