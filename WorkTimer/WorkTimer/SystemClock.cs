using System;

namespace WorkTimer
{
    public class SystemClock : IClock
    {
        #region Implementation of IClock

        public DateTime Now
        {
            get { return DateTime.Now; }
        }

        #endregion
    }
}
