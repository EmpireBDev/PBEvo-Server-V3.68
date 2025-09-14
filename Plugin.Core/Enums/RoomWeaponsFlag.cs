using System;

namespace Plugin.Core.Enums
{
    [Flags]
    public enum RoomWeaponsFlag
    {
        None = 0,
        Grenade = 1,
        Melee = 2,
        Secondary = 4,
        Accessory = 8,
        Assault = 16,
        SMG = 32,
        Sniper = 64,
        Shotgun = 128,
        Machinegun = 256,
        Barefist = 512,
        RPG7 = 1024
    }
}