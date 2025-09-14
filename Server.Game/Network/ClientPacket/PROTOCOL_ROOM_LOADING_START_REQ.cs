using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_ROOM_LOADING_START_REQ : GameClientPacket
    {
        private string MapName;
        public PROTOCOL_ROOM_LOADING_START_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            MapName = ReadS(ReadC());
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
                if (Room != null && Room.IsPreparing() && Room.GetSlot(Player.SlotId, out SlotModel Slot) && Slot.State == SlotState.LOAD)
                {
                    Slot.PreLoadDate = DateTimeUtil.Now();
                    Room.StartCounter(0, Player, Slot);
                    Room.ChangeSlotState(Slot, SlotState.RENDEZVOUS, true);
                    Room.MapName = MapName;
                    if (Slot.Id == Room.Leader)
                    {
                        Room.State = RoomState.Rendezvous;
                        Room.UpdateRoomInfo();
                    }
                }
                Client.SendPacket(new PROTOCOL_ROOM_LOADING_START_ACK(0));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_ROOM_LOADING_START_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}