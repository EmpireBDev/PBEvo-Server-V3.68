using System;

namespace Server.Match.Data.Enums
{
    [Flags]
    public enum UdpGameEvent : uint
    {
        ActionState = 0x100,
        Animation = 0x02,
        PosRotation = 0x8000000,
        SoundPosRotation = 0x800000,
        UseObject = 0x04,
        ActionForObjectSync = 0x10,
        RadioChat = 0x20,
        WeaponSync = 0x4000000,
        WeaponRecoil = 0x80,
        HpSync = 0x08,
        Suicide = 0x100000,
        MissionData = 0x800,
        RetriveDataForClient = 0x1000,
        SeizeDataForClient = 0x8000,
        DropWeapon = 0x400000,
        GetWeaponForClient = 0x1000000,
        FireData = 0x2000000,
        CharaFireNHitData = 0x400,
        HitData = 0x20000,
        GrenadeHit = 0x10000000,
        GetWeaponForHost = 0x200,
        FireDataOnObject = 0x40000000,
        FireNHitDataOnObject = 0x2000,
        BattleRoyalItem = 0x40,
        DirectPickUp = 0x4000,
        DeathDataForClient = 0x400,
    }
}
