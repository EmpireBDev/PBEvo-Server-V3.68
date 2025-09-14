using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Server.Game.Data.Sync.Client;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BATTLE_MISSION_BOMB_INSTALL_REQ : GameClientPacket
    {
        private int SlotIdx;
        private float X, Y, Z;
        private byte Zone;
        private ushort Unk;
        public PROTOCOL_BATTLE_MISSION_BOMB_INSTALL_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            SlotIdx = ReadD();
            Zone = ReadC();
            Unk = ReadUH();
            X = ReadT();
            Y = ReadT();
            Z = ReadT();
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
                if (Room != null && Room.RoundTime.Timer == null && Room.State == RoomState.Battle && !Room.ActiveC4)
                {
                    SlotModel Slot = Room.GetSlot(SlotIdx);
                    if (Slot == null || Slot.State != SlotState.BATTLE)
                    {
                        return;
                    }
                    RoomBombC4.InstallBomb(Room, Slot, Zone, Unk, X, Y, Z);
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BATTLE_MISSION_BOMB_INSTALL_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}