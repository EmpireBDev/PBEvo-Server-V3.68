using System;

namespace Server.Match.Data.Enums
{
    [Flags]
    public enum CharaMoves
    {
        Stop = 0,
        Left = 1,
        Back = 2,
        Right = 4,
        Front = 8,
        HeliInMove = 16,
        HeliUnknown = 32,
        HeliLeave = 64,
        HeliStopped = 128
    }
}