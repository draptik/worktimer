using System;

namespace WorkTimer
{
    public class StaticClock : IClock
    {
        #region Implementation of IClock

        public DateTime Now { get; private set; }

        #endregion

        public StaticClock()
            : this(new DateTime(2011, 06, 3, 9, 6, 0))
        {
        }

        public StaticClock(DateTime dateTime)
        {
            Now = dateTime;
        }
    }
}
