using System;

namespace Plugin.Core.Enums
{
    [Flags]
    public enum CouponEffects
    {
        Defense90 = 1,
        Ketupat = 2,
        Defense20 = 4,
        HollowPointPlus = 8,
        Defense10 = 0x10,
        HP5 = 0x20,
        JackHollowPoint = 0x40,
        ExtraGrenade = 0x80,
        C4SpeedKit = 0x100,
        HollowPoint = 0x200,
        FullMetalJack = 0x400,
        Defense5 = 0x800,
        Invincible = 0x1000,
        HP10 = 0x2000,
        QuickChangeReload = 0x4000,
        QuickChangeWeapon = 0x8000,
        FlashProtect = 0x10000,
        GetDroppedWeapon = 0x20000,
        Ammo40 = 0x40000,
        Respawn20 = 0x80000,
        Respawn30 = 0x100000,
        Respawn50 = 0x200000,
        Respawn100 = 0x400000,
        Ammo10 = 0x800000,
        ExtraThrowGrenade = 0x4000000,
    }
}