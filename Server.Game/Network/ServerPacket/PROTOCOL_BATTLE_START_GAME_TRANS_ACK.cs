using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_START_GAME_TRANS_ACK : GameServerPacket
    {
        private readonly RoomModel Room;
        private readonly SlotModel Slot;
        private readonly PlayerTitles Title;
        public PROTOCOL_BATTLE_START_GAME_TRANS_ACK(RoomModel Room, SlotModel Slot, PlayerTitles Title)
        {
            this.Room = Room;
            this.Slot = Slot;
            this.Title = Title;
        }
        public override void Write()
        {
            WriteH(4104);
            WriteH(0);
            WriteD((uint)Slot.PlayerId);
            WriteC(2);
            WriteC((byte)Slot.Id);
            WriteD(Slot.Id % 2 == 0 ? Slot.Equipment.CharaRedId : Slot.Equipment.CharaBlueId);
            WriteD(Slot.Equipment.WeaponPrimary);
            WriteD(Slot.Equipment.WeaponSecondary);
            WriteD(Slot.Equipment.WeaponMelee);
            WriteD(Slot.Equipment.WeaponExplosive);
            WriteD(Slot.Equipment.WeaponSpecial);
            WriteB(CharaData(Room, Slot));
            WriteD(Slot.Equipment.PartHead);
            WriteD(Slot.Equipment.PartFace);
            WriteD(Slot.Equipment.PartJacket);
            WriteD(Slot.Equipment.PartPocket);
            WriteD(Slot.Equipment.PartGlove);
            WriteD(Slot.Equipment.PartBelt);
            WriteD(Slot.Equipment.PartHolster);
            WriteD(Slot.Equipment.PartSkin);
            WriteD(Slot.Equipment.BeretItem);
            WriteB(new byte[5] { 0x64, 0x64, 0x64, 0x64, 0x64 });
            WriteC((byte)Title.Equiped1);
            WriteC((byte)Title.Equiped2);
            WriteC((byte)Title.Equiped3);
            WriteD(Slot.Equipment.AccessoryId);
            WriteD(Slot.Equipment.SprayId);
            WriteD(Slot.Equipment.NameCardId);
        }
        private byte[] CharaData(RoomModel Room, SlotModel Slot)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                if (Room.IsDinoMode())
                {
                    if (!Room.SwapRound)
                    {
                        S.WriteD(Slot.Id % 2 == 0 ? Slot.Equipment.DinoItem : Slot.Equipment.CharaBlueId);
                    }
                    else
                    {
                        S.WriteD(Slot.Id % 2 == 0 ? Slot.Equipment.CharaRedId : Slot.Equipment.DinoItem);
                    }
                }
                else
                {
                    S.WriteD(Slot.Id % 2 == 0 ? Slot.Equipment.CharaRedId : Slot.Equipment.CharaBlueId);
                }
                return S.ToArray();
            }
        }
    }
}