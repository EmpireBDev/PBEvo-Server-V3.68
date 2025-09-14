namespace Plugin.Core.Enums
{
    public enum StageOptions : byte
    {
        None = 0x00,
        Default = 0x01,
        AI = 0x02,
        DieHard = 0x04,
        Infection = 0x06,
        Sniper = 0x60,
        Shotgun = 0x80,
        Knuckle = 0xE0,
    }
}
