using System;
using Plugin.Core.Utility;

namespace Plugin.Core.Models
{
    public class BanHistory
    {
        public long ObjectId, PlayerId;
        public string Type, Value, Reason;
        public DateTime StartDate, EndDate;
        public BanHistory()
        {
            StartDate = DateTimeUtil.Now();
            Type = "";
            Value = "";
            Reason = "";
        }
    }
}