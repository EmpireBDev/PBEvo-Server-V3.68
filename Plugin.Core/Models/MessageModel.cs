using Plugin.Core.Enums;
using Plugin.Core.Utility;
using System;
using System.Globalization;

namespace Plugin.Core.Models
{
    public class MessageModel
    {
        public int ClanId;
        public long ObjectId, SenderId, ExpireDate;
        public string SenderName = "", Text = "";
        public NoteMessageState State;
        public NoteMessageType Type;
        public NoteMessageClan ClanNote;
        public int DaysRemaining;
        public MessageModel()
        {
        }
        public MessageModel(long expire, DateTime start)
        {
            ExpireDate = expire;
            SetDaysRemaining(start);
        }
        public MessageModel(double days)
        {
            DateTime date = DateTimeUtil.Now().AddDays(days);
            ExpireDate = long.Parse(date.ToString("yyMMddHHmm"));
            SetDaysRemaining(date, DateTimeUtil.Now());
        }
        private void SetDaysRemaining(DateTime now)
        {
            DateTime end = DateTime.ParseExact(ExpireDate.ToString(), "yyMMddHHmm", CultureInfo.InvariantCulture);
            SetDaysRemaining(end, now);
        }
        private void SetDaysRemaining(DateTime end, DateTime now)
        {
            int days = (int)Math.Ceiling((end - now).TotalDays);
            DaysRemaining = days < 0 ? 0 : days;
        }
    }
}