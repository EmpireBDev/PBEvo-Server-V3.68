namespace Plugin.Core.Models
{
    public class PlayerEvent
    {
        public long OwnerId;
        public int LastQuestFinish, LastPlaytimeFinish, LastVisitEventId, LastVisitSequence1, LastVisitSequence2, NextVisitDate, DayCheckedIdx;
        public long LastPlaytimeValue;
        public uint LastPlaytimeDate, LastLoginDate, LastXmasRewardDate, LastQuestDate;
    }
}