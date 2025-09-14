using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BATTLE_RESPAWN_FOR_AI_REQ : GameClientPacket
    {
        private int SlotIdx;
        public PROTOCOL_BATTLE_RESPAWN_FOR_AI_REQ(GameClient Client, byte[] Buff)
        {
            Makeme(Client, Buff);
        }
        public override void Read()
        {
            SlotIdx = ReadD();
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
                if (Room != null && Room.State == RoomState.Battle && Player.SlotId == Room.Leader)
                {
                    SlotModel Slot = Room.GetSlot(SlotIdx);
                    if (Slot != null)
                    {
                        Slot.AiLevel = Room.IngameAiLevel;
                        Room.SpawnsCount++;
                    }
                    using (PROTOCOL_BATTLE_RESPAWN_FOR_AI_ACK Packet = new PROTOCOL_BATTLE_RESPAWN_FOR_AI_ACK(SlotIdx))
                    {
                        Room.SendPacketToPlayers(Packet, SlotState.BATTLE, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BATTLE_RESPAWN_FOR_AI_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}