using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_START_GAME_ACK : GameServerPacket
    {
        private readonly RoomModel Room;
        public PROTOCOL_BATTLE_START_GAME_ACK(RoomModel Room)
        {
            this.Room = Room;
        }
        public override void Write()
        {
            WriteH(4103);
            WriteH(0);
            WriteB(PlayerIdData(Room));
            WriteB(RoomSlotData(Room));
            WriteB(SlotInfoData(Room));
            WriteC((byte)Room.MapId);
            WriteC((byte)Room.Rule);
            WriteC((byte)Room.Stage);
            WriteC((byte)Room.RoomType);
        }
        private byte[] PlayerIdData(RoomModel Room)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                S.WriteC((byte)Room.GetReadyPlayers());
                for (int i = 0; i < Room.Slots.Length; i++)
                {
                    SlotModel Slot = Room.Slots[i];
                    if (Slot.State >= SlotState.READY && Slot.Equipment != null)
                    {
                        Account Player = Room.GetPlayerBySlot(Slot);
                        if (Player != null && Player.SlotId == i)
                        {
                            S.WriteD((uint)Player.PlayerId);
                        }
                    }
                }
                return S.ToArray();
            }
        }
        private byte[] RoomSlotData(RoomModel Room)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                S.WriteC((byte)Room.Slots.Length);
                for (int i = 0; i < Room.Slots.Length; i++)
                {
                    S.WriteC((byte)(i % 2));
                }
                return S.ToArray();
            }
        }
        private byte[] SlotInfoData(RoomModel Room)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                S.WriteC((byte)Room.GetReadyPlayers());
                for (int i = 0; i < Room.Slots.Length; i++)
                {
                    SlotModel Slot = Room.Slots[i];
                    if (Slot.State >= SlotState.READY && Slot.Equipment != null)
                    {
                        Account Player = Room.GetPlayerBySlot(Slot);
                        if (Player != null && Player.SlotId == i)
                        {
                            S.WriteC((byte)Slot.Id);
                            S.WriteD(Slot.Id % 2 == 0 ? Slot.Equipment.CharaRedId : Slot.Equipment.CharaBlueId);
                            S.WriteD(Slot.Equipment.WeaponPrimary);
                            S.WriteD(Slot.Equipment.WeaponSecondary);
                            S.WriteD(Slot.Equipment.WeaponMelee);
                            S.WriteD(Slot.Equipment.WeaponExplosive);
                            S.WriteD(Slot.Equipment.WeaponSpecial);
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
                            S.WriteD(Slot.Equipment.PartHead);
                            S.WriteD(Slot.Equipment.PartFace);
                            S.WriteD(Slot.Equipment.PartJacket);
                            S.WriteD(Slot.Equipment.PartPocket);
                            S.WriteD(Slot.Equipment.PartGlove);
                            S.WriteD(Slot.Equipment.PartBelt);
                            S.WriteD(Slot.Equipment.PartHolster);
                            S.WriteD(Slot.Equipment.PartSkin);
                            S.WriteD(Slot.Equipment.BeretItem);
                            S.WriteB(new byte[5] { 0x64, 0x64, 0x64, 0x64, 0x64 });
                            if (Player != null)
                            {
                                S.WriteC((byte)Player.Title.Equiped1);
                                S.WriteC((byte)Player.Title.Equiped2);
                                S.WriteC((byte)Player.Title.Equiped3);
                            }
                            else
                            {
                                S.WriteB(new byte[3] { 0xFF, 0xFF, 0xFF });
                            }
                            S.WriteD(Slot.Equipment.AccessoryId);
                            S.WriteD(Slot.Equipment.SprayId);
                            S.WriteD(Slot.Equipment.NameCardId);
                        }
                    }
                }
                return S.ToArray();
            }
        }
    }
}