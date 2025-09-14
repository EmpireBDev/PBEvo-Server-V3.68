using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_GET_SLOTINFO_ACK : GameServerPacket
    {
        private readonly RoomModel Room;
        public PROTOCOL_ROOM_GET_SLOTINFO_ACK(RoomModel Room)
        {
            this.Room = Room;
            if (Room != null)
            {
                if (Room.GetLeader() == null)
                {
                    Room.SetNewLeader(-1, SlotState.EMPTY, Room.Leader, false);
                }
            }
        }
        public override void Write()
        {
            WriteH(3848);
            WriteC((byte)Room.Leader);
            foreach (SlotModel Slot in Room.Slots)
            {
                WriteB(SlotInfoData(Slot));
            }
            foreach (SlotModel Slot in Room.Slots)
            {
                WriteC((byte)(Room.RoomType == RoomCondition.FreeForAll ? Slot.Costume : Slot.Id % 2));
            }
        }
        private byte[] SlotInfoData(SlotModel Slot)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                S.WriteH(31);
                S.WriteC((byte)Slot.State);
                Account User = Room.GetPlayerBySlot(Slot);
                if (User != null)
                {
                    ClanModel Clan = ClanManager.GetClan(User.ClanId);
                    S.WriteC((byte)User.GetRank());
                    S.WriteD(Clan.Id);
                    S.WriteD(User.ClanAccess);
                    S.WriteC((byte)Clan.Rank);
                    S.WriteD(Clan.Logo);
                    S.WriteC((byte)User.CafePC);
                    S.WriteC((byte)User.TourneyLevel);
                    S.WriteD((uint)User.Effects);
                    S.WriteC((byte)Clan.Effect);
                    S.WriteC(0);
                    S.WriteH((ushort)NATIONS);
                    S.WriteD(User.Equipment.NameCardId);
                    S.WriteH(0);
                    S.WriteC((byte)(Clan.Name.Length * 2));
                    S.WriteU(Clan.Name, Clan.Name.Length * 2);
                }
                else
                {
                    S.WriteB(new byte[10]);
                    S.WriteD(4294967295);
                    S.WriteB(new byte[17]);
                }
                return S.ToArray();
            }
        }
    }
}