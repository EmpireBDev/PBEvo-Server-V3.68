using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BATTLE_CHANGE_DIFFICULTY_LEVEL_REQ : GameClientPacket
    {
        public PROTOCOL_BATTLE_CHANGE_DIFFICULTY_LEVEL_REQ(GameClient client, byte[] data)
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
                RoomModel Room = Player.Room;
                if (Room != null && Room.State == RoomState.Battle && Room.IngameAiLevel < 10)
                {
                    SlotModel slot = Room.GetSlot(Player.SlotId);
                    if (slot != null && slot.State == SlotState.BATTLE)
                    {
                        if (Room.IngameAiLevel <= 9)
                        {
                            Room.IngameAiLevel++;
                        }
                        using (PROTOCOL_BATTLE_CHANGE_DIFFICULTY_LEVEL_ACK packet = new PROTOCOL_BATTLE_CHANGE_DIFFICULTY_LEVEL_ACK(Room))
                        {
                            Room.SendPacketToPlayers(packet, SlotState.READY, 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BATTLE_CHANGE_DIFFICULTY_LEVEL_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
