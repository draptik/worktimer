using System;
using System.Globalization;

namespace WorkTimer.Domain
{
    public class WorkTime
    {
        #region Fields

        private readonly IClock _clock; // unit testing
        private readonly Config _config;

        #endregion

        #region CTOR

        public WorkTime(string startTimeString)
            : this(new SystemClock(), startTimeString)
        {
        }

        public WorkTime(IClock clock, string startTimeString)
            : this(clock, startTimeString, null)
        {
        }

        public WorkTime(IClock clock, string startTimeString, DateTime? startDate)
        {
            _config = Config.GetInstance();
            _clock = clock; // unit testing 
            
            var validStartTime = ValidateStartTimeFormat(startTimeString);
            var startDateTime = InitStartDateTime(validStartTime, startDate);
            Init(startDateTime);
        }

        public WorkTime(IClock clock, DateTime startDateTime)
        {
            _config = Config.GetInstance();
            _clock = clock; // unit testing 
            Init(startDateTime);
        }

        public WorkTime(DateTime? startDateTime)
        {
            _config = Config.GetInstance();
            _clock = new SystemClock();

            if (startDateTime.HasValue) {
                Init((DateTime)startDateTime);
            }
        }

        #endregion

        #region Properties

        public DateTime StartTime { get; private set; }
        public DateTime TargetTime { get; private set; }
        public TimeSpan TargetTimeSpan { get { return _config.TargetTimeSpan; } }

        public TimeSpan RemainingTillTarget { get; private set; }

        public TimeSpan TimeSpent { get; private set; }

        public DateTime MinTimeStart { get { return StartTime.Add(_config.MinTimeSpan); } }
        public DateTime MinTimeEnd { get { return StartTime.Add(_config.MinTimeSpan).Add(_config.BreakTimeSpan); } }
        public TimeSpan RemainingTillMinTime { get { return MinTimeStart.Subtract(_clock.Now); } }

        public DateTime MaxTime { get { return StartTime.Add(_config.MaxTimeSpan); } }
        public TimeSpan RemainingTillMaxTime { get { return MaxTime.Subtract(_clock.Now); } }

        public TimeSpan Balance
        {
            get
            {
                var result = -RemainingTillTarget;
                if (TimeSpent.TotalHours > _config.MinTimeStartNum && 
                    TimeSpent.TotalHours < _config.MinTimeStartNum + _config.BreakTimeNum) {
                    result = new TimeSpan(-2, 0, 0);
                }
                else if (TimeSpent >= _config.MaxTimeSpan) {
                    result = new TimeSpan(2, 0, 0);
                }
                else if (TimeSpent.TotalHours <= _config.MinTimeStartNum) {
                    result = -RemainingTillTarget.Subtract(_config.BreakTimeSpan);
                }
                return result;
            }
        }

        #endregion

        public bool WarnIfMaxTimeReached()
        {
            return RemainingTillMaxTime < _config.WarningTimeSpanMax;
        }

        public bool IsLessThanMinTime()
        {
            return RemainingTillMinTime.TotalSeconds > 0;
        }

        public bool IsLessThanTargetTime()
        {
            return RemainingTillTarget.TotalSeconds > 0;
        }

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
            if (DateTime.TryParseExact(startTimeString, _config.TimeFormat, _config.CurrentCultureInfo, DateTimeStyles.None, out startTime)) {
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
