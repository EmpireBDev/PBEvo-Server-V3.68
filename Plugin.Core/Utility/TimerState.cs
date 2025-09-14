using System;
using System.Threading;

namespace Plugin.Core.Utility
{
    public class TimerState
    {
        public Timer Timer = null;
        public DateTime EndDate = new DateTime();
        private readonly object Sync = new object();
        public TimerState()
        {
        }
        public void Start(int Period, TimerCallback Callback)
        {
            lock (Sync)
            {
                Timer = new Timer(Callback, this, Period, -1);
                EndDate = DateTimeUtil.Now().AddMilliseconds(Period);
            }
        }
        public int GetTimeLeft()
        {
            if (Timer == null)
            {
                return 0;
            }
            TimeSpan Span = EndDate - DateTimeUtil.Now();
            int Seconds = (int)Span.TotalSeconds;
            if (Seconds < 0)
            {
                return 0;
            }
            return Seconds;
        }
    }
}