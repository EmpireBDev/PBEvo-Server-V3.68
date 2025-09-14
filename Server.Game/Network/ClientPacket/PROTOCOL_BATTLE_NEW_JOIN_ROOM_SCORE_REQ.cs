using Plugin.Core.Enums;
using Server.Game.Network.ServerPacket;
using Server.Game.Data.Models;
using System;
using Plugin.Core;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BATTLE_NEW_JOIN_ROOM_SCORE_REQ : GameClientPacket
    {
        public PROTOCOL_BATTLE_NEW_JOIN_ROOM_SCORE_REQ(GameClient client, byte[] data)
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
                Account player = Client.Player;
                if (player == null)
                {
                    return;
                }
                RoomModel room = player.Room;
                if (room != null && room.State >= RoomState.Loading && room.Slots[player.SlotId].State == SlotState.NORMAL)
                {
                    Client.SendPacket(new PROTOCOL_BATTLE_NEW_JOIN_ROOM_SCORE_ACK(room));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BATTLE_NEW_JOIN_ROOM_SCORE_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}