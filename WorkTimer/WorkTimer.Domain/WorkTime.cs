using System;
using System.Globalization;

namespace WorkTimer.Domain
{
    public class WorkTime
    {
        #region Fields

        private const string TimeFormat = "H:mm";
        private readonly CultureInfo _currentCultureInfo = new CultureInfo("de-DE");
        
        private readonly IClock _clock; // unit testing

        #endregion

        #region Properties

        public DateTime StartTime { get; private set; }
        public DateTime TargetTime { get; private set; }
        public TimeSpan TargetTimeSpan { get { return new TimeSpan(8, 45, 0); } }

        public TimeSpan RemainingTillTarget { get; private set; }

        public TimeSpan TimeSpent { get; private set; }

        public DateTime MinTimeStart { get { return StartTime.AddHours(6); } }
        public DateTime MinTimeEnd { get { return StartTime.AddHours(6).AddMinutes(45); } }
        public TimeSpan RemainingTillMinTime { get { return MinTimeStart.Subtract(_clock.Now); } }

        public DateTime MaxTime { get { return StartTime.AddHours(10).AddMinutes(45); } }
        public TimeSpan RemainingTillMaxTime { get { return MaxTime.Subtract(_clock.Now); } }

        public TimeSpan Balance
        {
            get
            {
                var result = -RemainingTillTarget;
                if (TimeSpent.TotalHours > 6 && TimeSpent.TotalHours < 6.75)
                {
                    result = new TimeSpan(-2, 0, 0);
                }
                else if (TimeSpent >= new TimeSpan(10, 45, 0))
                {
                    result = new TimeSpan(2, 0, 0);
                }
                else if (TimeSpent.TotalHours <= 6)
                {
                    result = -RemainingTillTarget.Subtract(new TimeSpan(0, 45, 0));
                }
                return result;
            }
        }

        #endregion

        #region CTOR

        public WorkTime(string startTimeString) : this(new SystemClock(), startTimeString)
        {
        }

        public WorkTime(IClock clock, string startTimeString)
            : this(clock, startTimeString, null)
        {
        }

        public WorkTime(IClock clock, string startTimeString, DateTime? startDate)
        {
            _clock = clock; // unit testing 

            var validStartTime = ValidateStartTimeFormat(startTimeString);
            var startDateTime = InitStartDateTime(validStartTime, startDate);
            Init(startDateTime);
        }

        public WorkTime(IClock clock, DateTime startDateTime)
        {
            _clock = clock; // unit testing 
            Init(startDateTime);
        }

        #endregion

        #region Private

        private void Init(DateTime startDateTime)
        {
            if (IsStartDateTimeInFuture(startDateTime)) {
                throw new ArgumentException("Invalid start time (start time is in future)!");
            }

            InitAllTimes(startDateTime);
        }

        private bool IsStartDateTimeInFuture(DateTime startDateTime)
        {
            return startDateTime > _clock.Now;
        }

        private DateTime ValidateStartTimeFormat(string startTimeString)
        {
            DateTime startTime;
            if (DateTime.TryParseExact(startTimeString, TimeFormat, _currentCultureInfo, DateTimeStyles.None, out startTime)) {
                return startTime;
            }
            throw new ArgumentException("Invalid start time! Required format: H:mm");
        }

        /// <summary>
        /// Note: "Merge" with "_clock" (used for unit testing...)
        /// </summary>
        private DateTime InitStartDateTime(DateTime startTime, DateTime? startDate)
        {
            if (!startDate.HasValue) {
                startDate = new DateTime(_clock.Now.Year, _clock.Now.Month, _clock.Now.Day);
            }
            var startDateTime = new DateTime(startDate.Value.Year, startDate.Value.Month, startDate.Value.Day,
                                             startTime.Hour, startTime.Minute, 0);
            return startDateTime;
        }

        private void InitAllTimes(DateTime startTime)
        {
            StartTime = startTime;

            TargetTime = StartTime + TargetTimeSpan;

            RemainingTillTarget = TargetTime.Subtract(_clock.Now);

            TimeSpent = TargetTimeSpan - RemainingTillTarget;
        }

        #endregion

    }
}
