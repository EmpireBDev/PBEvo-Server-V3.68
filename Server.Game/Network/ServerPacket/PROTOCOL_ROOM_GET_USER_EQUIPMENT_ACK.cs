using Plugin.Core.Models;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_GET_USER_EQUIPMENT_ACK : GameServerPacket
    {
        private readonly uint Error;
        private readonly Account Player;
        private readonly SlotModel Slot;
        public PROTOCOL_ROOM_GET_USER_EQUIPMENT_ACK(uint Error, Account Player, SlotModel Slot)
        {
            this.Error = Error;
            this.Player = Player;
            this.Slot = Slot;
        }
        public override void Write()
        {
            WriteH(3922);
            WriteD(0);
            WriteD(Error);
            if (Error == 0)
            {
                WriteH(0);
                WriteC(10); //Chara count EQ
                WriteD(Slot.Id % 2 == 0 ? Slot.Equipment.CharaRedId : Slot.Equipment.CharaBlueId);
                WriteD(Slot.Equipment.PartHead);
                WriteD(Slot.Equipment.PartFace);
                WriteD(Slot.Equipment.PartJacket);
                WriteD(Slot.Equipment.PartPocket);
                WriteD(Slot.Equipment.PartGlove);
                WriteD(Slot.Equipment.PartBelt);
                WriteD(Slot.Equipment.PartHolster);
                WriteD(Slot.Equipment.PartSkin);
                WriteD(Slot.Equipment.BeretItem);
                WriteC(5); //Weapon count EQ
                WriteD(Slot.Equipment.WeaponPrimary);
                WriteD(Slot.Equipment.WeaponSecondary);
                WriteD(Slot.Equipment.WeaponMelee);
                WriteD(Slot.Equipment.WeaponExplosive);
                WriteD(Slot.Equipment.WeaponSpecial);
                WriteC(2);
                WriteD(Slot.Equipment.CharaRedId);
                WriteD(Slot.Equipment.CharaBlueId);
                WriteC((byte)Player.SlotId);
            }
        }
    }
}
