using Plugin.Core.Utility;
using System;

namespace Plugin.Core.Models
{
    public class EventPlaytimeModel
    {
        public int GoodReward1;
        public int GoodReward2;
        public uint StartDate;
        public uint EndDate;
        public string Title;
        public long Time;
        public EventPlaytimeModel()
        {
            Title = "";
        }
        public bool EventIsEnabled()
        {
            uint Date = uint.Parse(DateTimeUtil.Now("yyMMddHHmm"));
            if (StartDate <= Date && Date < EndDate)
            {
                return true;
            }
            return false;
        }
    }
}
