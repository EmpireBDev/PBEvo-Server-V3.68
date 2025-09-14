namespace Plugin.Core.Enums
{
    public enum AccountFeatures : uint
    {
        PLAYTIME_ONLY = 0x100, // not sure? (3.24 correct)
        CLAN_ONLY = 0x1000,
        COUPON_ONLY = 0x4000,
        CLAN_COUPON = 0x777E,
        TAGS_ONLY = 0x4000000,
        TOKEN_ONLY = 0x7E770000,
        TOKEN_CLAN = 0x7E779934,
        RATING_BOTH = 0x7FFFFEF8,
        TEST_MODE = 0x7FFFFEF9,
        FROMSNIFF = 0x8E66777A,
        ALL = 0x8E66777E,

        PLAYTIME = 256,
        CLAN = 4096,
        TICKET = 16384,
        TAGS = 67108864,
    }
}
