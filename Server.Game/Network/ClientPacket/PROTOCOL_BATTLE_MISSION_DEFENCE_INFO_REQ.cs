using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Server.Game.Data.Sync.Client;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BATTLE_MISSION_DEFENCE_INFO_REQ : GameClientPacket
    {
        private ushort TankA, TankB;
        private List<ushort> DamageTankA = new List<ushort>(), DamageTankB = new List<ushort>();
        public PROTOCOL_BATTLE_MISSION_DEFENCE_INFO_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            TankA = ReadUH();
            TankB = ReadUH();
            for (int i = 0; i < 16; i++)
            {
                DamageTankA.Add(ReadUH());
            }
            for (int i = 0; i < 16; i++)
            {
                DamageTankB.Add(ReadUH());
            }
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
                if (room != null && room.RoundTime.Timer == null && room.State == RoomState.Battle && !room.SwapRound)
                {
                    SlotModel slot = room.GetSlot(player.SlotId);
                    if (slot == null || slot.State != SlotState.BATTLE)
                    {
                        return;
                    }
                    room.Bar1 = TankA;
                    room.Bar2 = TankB;
                    for (int i = 0; i < 16; i++)
                    {
                        SlotModel slotR = room.Slots[i];
                        if (slotR.PlayerId > 0 && slotR.State == SlotState.BATTLE)
                        {
                            slotR.DamageBar1 = DamageTankA[i];
                            slotR.DamageBar2 = DamageTankB[i];
                        }
                    }
                    using (PROTOCOL_BATTLE_MISSION_DEFENCE_INFO_ACK packet = new PROTOCOL_BATTLE_MISSION_DEFENCE_INFO_ACK(room))
                    {
                        room.SendPacketToPlayers(packet, SlotState.BATTLE, 0);
                    }
                    if (TankA == 0 && TankB == 0)
                    {
                        RoomSabotageSync.EndRound(room, TeamEnum.FR_TEAM);
                    }
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}