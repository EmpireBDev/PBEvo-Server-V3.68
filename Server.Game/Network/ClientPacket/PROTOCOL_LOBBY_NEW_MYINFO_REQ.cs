using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_LOBBY_NEW_MYINFO_REQ :GameClientPacket
    {
        public PROTOCOL_LOBBY_NEW_MYINFO_REQ(GameClient client, byte[] data)
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
                Account Player = Client.Player;
                if (Player == null)
                {
                    return;
                }
                Client.SendPacket(new PROTOCOL_LOBBY_NEW_MYINFO_ACK(Player.PlayerId));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_LOBBY_NEW_MYINFO_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
