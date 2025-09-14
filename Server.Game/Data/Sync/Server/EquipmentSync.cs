using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Server.Game.Data.Models;
using System;

namespace Server.Game.Data.Sync.Server
{
    public class EquipmentSync
    {
        public static void SendUDPPlayerSync(RoomModel Room, SlotModel Slot, CouponEffects Effects, int Type)
        {
            try
            {
                if (Room == null || Slot == null)
                {
                    return;
                }
                using (SyncServerPacket S = new SyncServerPacket())
                {
                    S.WriteH(1);
                    S.WriteD(Room.UniqueRoomId);
                    S.WriteD(Room.Seed);
                    S.WriteQ(Room.StartTick);
                    S.WriteC((byte)Type);
                    S.WriteC((byte)Room.Rounds);
                    S.WriteC((byte)Slot.Id);
                    S.WriteC((byte)Slot.SpawnsCount);
                    S.WriteC(BitConverter.GetBytes(Slot.PlayerId)[0]);
                    if (Type == 0 || Type == 2)
                    {
                        int CharaId = 0;
                        if (Room.IsDinoMode())
                        {
                            if (Room.Rounds == 1 && Slot.Team == TeamEnum.CT_TEAM || Room.Rounds == 2 && Slot.Team == TeamEnum.FR_TEAM)
                            {
                                CharaId = Room.Rounds == 2 ? Slot.Equipment.CharaRedId : Slot.Equipment.CharaBlueId;
                            }
                            else if (Room.TRex == Slot.Id)
                            {
                                CharaId = -1;
                            }
                            else
                            {
                                CharaId = Slot.Equipment.DinoItem;
                            }
                        }
                        else
                        {
                            CharaId = Slot.Team == TeamEnum.FR_TEAM ? Slot.Equipment.CharaRedId : Slot.Equipment.CharaBlueId;
                        }
                        int HPBonus = 0;
                        if (Effects.HasFlag(CouponEffects.HP5))
                        {
                            HPBonus += 5;
                        }
                        if (Effects.HasFlag(CouponEffects.HP10))
                        {
                            HPBonus += 10;
                        }
                        S.WriteD(CharaId);
                        S.WriteC((byte)HPBonus);
                        S.WriteC((byte)(Effects.HasFlag(CouponEffects.C4SpeedKit) ? 1 : 0));
                        S.WriteD(Slot.Equipment.WeaponPrimary);
                        S.WriteD(Slot.Equipment.WeaponSecondary);
                        S.WriteD(Slot.Equipment.WeaponMelee);
                        S.WriteD(Slot.Equipment.WeaponExplosive);
                        S.WriteD(Slot.Equipment.WeaponSpecial);
                    }
                    GameXender.Sync.SendPacket(S.ToArray(), Room.UdpServer.Connection);
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}
