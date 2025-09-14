using System;

namespace Plugin.Core.Enums
{
    [Flags]
    public enum TicketType
    {
        NONE = 0,
        ITEM = 1,
        VALUE = 2,
    }
}
