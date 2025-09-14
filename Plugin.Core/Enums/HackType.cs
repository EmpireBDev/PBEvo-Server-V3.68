namespace Plugin.Core.Enums
{
    public enum HackType
    {
        DEFAULT = 0,
        AMMO_COUNT = 1, // (410.0) Grenade Count/ RPG Count
        UNK_1 = 12, // (1012.0)
        UNK_2 = 14, // (1012.0)
        UNK_3 = 16, // (1014.0)
        UNK_4 = 18, // (1016.0)
        UNK_5 = 19, // (1017.0) (Value > 2.0)
        STAGE = 21, // (1019.0) Shoot at more than 1 person, and do not go Shotgun
        SPEED = 23, // (1021.0) - PosRotation (Value > 1.0) Velocity
        UNK_6 = 24, // (1022.0)  
        UNK_7 = 25, // (1023.0) 
        INVALID_USAGE_EFFECT = 26, // (1024.0) - Invalid item usage (DeathType != 10) Use Healing without the Medical Kit
        DISSTANCE = 28, // (1026.0) - Shoot (Value > 40.0)
        UNK_8 = 32, // (1030.0)
        JUMP_CHECK = 34, // (1032.0)
    }
}