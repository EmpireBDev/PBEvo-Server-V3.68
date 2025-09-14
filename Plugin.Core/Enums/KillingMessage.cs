using System;

namespace Plugin.Core.Enums
{
    [Flags]
    public enum KillingMessage
    {
        None = 0,
        PiercingShot = 1,
        MassKill = 2,
        ChainStopper = 4,
        Headshot = 8,
        ChainHeadshot = 16,
        ChainSlugger = 32,
        Suicide = 64,
        ObjectDefense = 128
    }
}