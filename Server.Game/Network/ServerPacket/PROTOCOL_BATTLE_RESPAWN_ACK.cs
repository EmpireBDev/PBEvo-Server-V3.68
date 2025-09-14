using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Server.Game.Data.Models;
using Server.Game.Data.Utils;
using System.Collections.Generic;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_RESPAWN_ACK : GameServerPacket
    {
        private readonly SlotModel Slot;
        private readonly RoomModel Room;
        public PROTOCOL_BATTLE_RESPAWN_ACK(RoomModel Room, SlotModel Slot)
        {
            this.Slot = Slot;
            this.Room = Room;
        }
        public override void Write()
        {
            WriteH(4114);
            WriteD(Slot.Id);
            WriteD(Room.SpawnsCount++);
            WriteD(++Slot.SpawnsCount);
            WriteD(Slot.Equipment.WeaponPrimary);
            WriteD(Slot.Equipment.WeaponSecondary);
            WriteD(Slot.Equipment.WeaponMelee);
            WriteD(Slot.Equipment.WeaponExplosive);
            WriteD(Slot.Equipment.WeaponSpecial);
            WriteB(new byte[5] { 0x64, 0x64, 0x64, 0x64, 0x64 });
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
            WriteD(Slot.Equipment.AccessoryId);
            WriteB(DinoData(Room, AllUtils.GetDinossaurs(Room, false, Slot.Id)));
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
        private byte[] DinoData(RoomModel Room, List<int> Dinos)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                if (Room.IsDinoMode())
                {
                    int TRex = Dinos.Count == 1 || Room.IsDinoMode("CC") ? 255 : Room.TRex;
                    S.WriteC((byte)TRex);
                    S.WriteC(10);
                    for (int i = 0; i < Dinos.Count; i++)
                    {
                        int slotId = Dinos[i];
                        if (slotId != Room.TRex && Room.IsDinoMode("DE") || Room.IsDinoMode("CC"))
                        {
                            S.WriteC((byte)slotId);
                        }
                    }
                    int Value = 8 - Dinos.Count - (TRex == 255 ? 1 : 0);
                    for (int i = 0; i < Value; i++)
                    {
                        S.WriteC(255);
                    }
                    S.WriteC(255);
                }
                else
                {
                    S.WriteB(new byte[10]);
                }
                return S.ToArray();
            }
        }
    }
}
