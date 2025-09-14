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
    public class PROTOCOL_BATTLE_MISSION_GENERATOR_INFO_REQ : GameClientPacket
    {
        private ushort FRBar, CTBar;
        private List<ushort> Damages = new List<ushort>();
        public PROTOCOL_BATTLE_MISSION_GENERATOR_INFO_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            FRBar = ReadUH();
            CTBar = ReadUH();
            for (int i = 0; i < 16; i++)
            {
                Damages.Add(ReadUH());
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
                    room.Bar1 = FRBar;
                    room.Bar2 = CTBar;
                    for (int i = 0; i < 16; i++)
                    {
                        SlotModel slotR = room.Slots[i];
                        if (slotR.PlayerId > 0 && slotR.State == SlotState.BATTLE)
                        {
                            slotR.DamageBar1 = Damages[i];
                            slotR.EarnedEXP = Damages[i] / 600;
                        }
                    }
                    using (PROTOCOL_BATTLE_MISSION_GENERATOR_INFO_ACK packet = new PROTOCOL_BATTLE_MISSION_GENERATOR_INFO_ACK(room))
                    {
                        room.SendPacketToPlayers(packet, SlotState.BATTLE, 0);
                    }
                    if (FRBar == 0)
                    {
                        RoomSabotageSync.EndRound(room, TeamEnum.CT_TEAM);
                    }
                    else if (CTBar == 0)
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