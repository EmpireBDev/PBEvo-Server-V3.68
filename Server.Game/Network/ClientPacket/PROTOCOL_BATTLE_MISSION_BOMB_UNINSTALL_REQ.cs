using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Server.Game.Data.Sync.Client;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BATTLE_MISSION_BOMB_UNINSTALL_REQ : GameClientPacket
    {
        private int slotIdx;
        public PROTOCOL_BATTLE_MISSION_BOMB_UNINSTALL_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            slotIdx = ReadD();
        }
        public override void Run()
        {
            Account player = Client.Player;
            if (player == null)
            {
                return;
            }
            RoomModel room = player.Room;
            if (room != null && room.RoundTime.Timer == null && room.State == RoomState.Battle && room.ActiveC4)
            {
                SlotModel slot = room.GetSlot(slotIdx);
                if (slot == null || slot.State != SlotState.BATTLE)
                {
                    return;
                }
                RoomBombC4.UninstallBomb(room, slot);
            }
        }
    }
}