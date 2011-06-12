using System;
using System.Globalization;

namespace WorkTimer
{
    public class WorkTime
    {
        #region Fields

        private const string TimeFormat = "H:mm";
        private readonly CultureInfo _currentCultureInfo = new CultureInfo("de-DE");
        
        private readonly IClock _clock;

        #endregion

        #region Properties

        public DateTime StartTime { get; private set; }
        public DateTime TargetTime { get; private set; }
        public TimeSpan TargetTimeSpan { get { return new TimeSpan(8, 45, 0); } }

        public TimeSpan RemainingTillTarget { get; private set; }
        public string RemainingTillTargetString { get { return RemainingTillTarget.ToDisplayString(); } }

        public TimeSpan TimeSpent { get; private set; }

        public DateTime MinTimeStart { get { return StartTime.AddHours(6); } }
        public DateTime MinTimeEnd { get { return StartTime.AddHours(6).AddMinutes(45); } }
        public TimeSpan RemainingTillMinTime { get { return MinTimeStart.Subtract(_clock.Now); } }

        public DateTime MaxTime { get { return StartTime.AddHours(10).AddMinutes(45); } }
        public TimeSpan RemainingTillMaxTime { get { return MaxTime.Subtract(_clock.Now); } }

        #endregion

        #region CTOR

        public WorkTime(string startTimeString) : this(new SystemClock(), startTimeString)
        {
        }

        public WorkTime(IClock clock, string startTimeString)
        {
            _clock = clock;
            var validStartTime = ValidateStartTime(startTimeString);
            CalcTimes(InitStartTime(validStartTime));
        }

        #endregion

        #region Private

        private DateTime ValidateStartTime(string startTimeString)
        {
            DateTime startTime;
            if (DateTime.TryParseExact(startTimeString, TimeFormat, _currentCultureInfo, DateTimeStyles.None, out startTime)) {
                return startTime;
            }
            throw new ArgumentException("Invalid start time!");
        }

        private DateTime InitStartTime(DateTime startTime)
        {
            return new DateTime(_clock.Now.Year, _clock.Now.Month, _clock.Now.Day, startTime.Hour,
                                startTime.Minute, startTime.Second);
        }

        private void CalcTimes(DateTime startTime)
        {
            StartTime = startTime;

            TargetTime = StartTime + TargetTimeSpan;

            RemainingTillTarget = TargetTime.Subtract(_clock.Now);

            TimeSpent = TargetTimeSpan - RemainingTillTarget;
        }

        

        #endregion

    }
}
