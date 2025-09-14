using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_GM_LOG_ROOM_REQ : GameClientPacket
    {
        private int slot;
        public PROTOCOL_GM_LOG_ROOM_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            slot = ReadC();
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
                RoomModel Room = Player.Room;
                if (Room != null && Room.GetPlayerBySlot(slot, out Account Member))
                {
                    Client.SendPacket(new PROTOCOL_GM_LOG_ROOM_ACK(0, Member));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_GM_LOG_ROOM_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}