using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Server.Game.Data.Utils;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BATTLE_MISSION_TOUCHDOWN_COUNT_REQ : GameClientPacket
    {
        private int WeaponId, TRex;
        public PROTOCOL_BATTLE_MISSION_TOUCHDOWN_COUNT_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            TRex = ReadC();
            WeaponId = ReadD();
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
                if (Room != null && Room.RoundTime.Timer == null && Room.State == RoomState.Battle && Room.TRex == TRex)
                {
                    SlotModel Slot = Room.GetSlot(Player.SlotId);
                    if (Slot == null || Slot.State != SlotState.BATTLE)
                    {
                        return;
                    }
                    if (Slot.Team == 0)
                    {
                        Room.FRDino += 5;
                    }
                    else
                    {
                        Room.CTDino += 5;
                    }
                    using (PROTOCOL_BATTLE_MISSION_TOUCHDOWN_COUNT_ACK Packet = new PROTOCOL_BATTLE_MISSION_TOUCHDOWN_COUNT_ACK(Room))
                    {
                        Room.SendPacketToPlayers(Packet, SlotState.BATTLE, 0);
                    }
                    AllUtils.CompleteMission(Room, Player, Slot, MissionType.DEATHBLOW, WeaponId);
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BATTLE_MISSION_TOUCHDOWN_COUNT_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}