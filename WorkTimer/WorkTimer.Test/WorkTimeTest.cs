using System;
using NUnit.Framework;

namespace WorkTimer.Test
{
    [TestFixture]
    public class WorkTimeTest
    {
        private WorkTime _workTime;
        private WorkTime _w;
        private IClock _clock;

        [SetUp]
        public void Setup()
        {
            _clock = new StaticClock();
            _w = new WorkTime(_clock, "8:00");
        }

        #region StartTime

        #region StartTime Valid

        [Test]
        public void StartTime_InputValid()
        {
            const string validInput = "8:00";
            _workTime = new WorkTime(_clock, validInput);
            var startTime = _workTime.StartTime;
            Assert.IsNotNull(startTime);
        }

        #endregion

        #region StartTime Invalid

        [Test, ExpectedException(typeof(ArgumentException))]
        public void StartTime_InputTimeInvalid_Empty()
        {
            var inValidInput = string.Empty;
            _workTime = new WorkTime(_clock, inValidInput);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void StartTime_InputTimeInvalid_String()
        {
            const string inValidInput = "invalid";
            _workTime = new WorkTime(_clock, inValidInput);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void StartTime_InputTimeInvalid_TooSmall()
        {
            const string inValidInput = "-8:00";
            _workTime = new WorkTime(_clock, inValidInput);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void StartTime_InputTimeInvalid_TooLarge()
        {
            const string inValidInput = "25:00";
            _workTime = new WorkTime(_clock, inValidInput);
        }


        [Test, ExpectedException(typeof(ArgumentException))]
        public void StartTime_InputTimeInvalid_WrongFormat_Point()
        {
            const string inValidInput = "8.00";
            _workTime = new WorkTime(_clock, inValidInput);
        }

        #endregion

        #endregion

        #region TargetTime

        [Test]
        public void TargetTime_IsNotNull_For_Valid_StartTime()
        {
            var targetTime = _workTime.TargetTime;
            Assert.IsNotNull(targetTime);
        }

        [Test]
        public void TargetTime_IsCorrect_For_Valid_StartTime()
        {
            var targetTime = _w.TargetTime;
            Assert.AreEqual(_w.StartTime + new TimeSpan(8, 45, 0), targetTime);
        }

        [Test]
        public void TargetTime_With_StartTime_TwoDaysAgo()
        {
            IClock currentTime = new StaticClock(new DateTime(2011, 06, 10, 11, 0, 0));
            var startDate = new DateTime(2011, 06, 9);
            var workTime = new WorkTime(currentTime, "8:00", startDate);
            var expectedTargetTime = startDate.Add(new TimeSpan(16, 45, 0));
            Assert.AreEqual(expectedTargetTime, workTime.TargetTime);
        }

        #endregion

        #region RemainingTime Till Target

        [Test]
        public void RemainingTimeTarget()
        {
            var expected = _w.TargetTime - _clock.Now;
            Assert.AreEqual(expected, _w.RemainingTillTarget);
        }

        [Test]
        public void RemainingTimeTarget_With_StartTime_TwoDaysAgo()
        {
            IClock currentTime = new StaticClock(new DateTime(2011, 06, 10, 11, 0, 0));
            var startDate = new DateTime(2011, 06, 9);
            var workTime = new WorkTime(currentTime, "8:00", startDate);
            var expectedRemainingTargetTime = new TimeSpan(-18, -15, 0);
            Assert.AreEqual(expectedRemainingTargetTime, workTime.RemainingTillTarget);
        }

        #endregion

        #region Current Time Spent

        [Test]
        public void CurrentTimeSpent()
        {
            var expected = _w.TargetTimeSpan - _w.RemainingTillTarget;
            Assert.AreEqual(expected, _w.TimeSpent);
        }

        [Test]
        public void CurrentTimeSpent_With_StartTime_TwoDaysAgo()
        {
            IClock currentTime = new StaticClock(new DateTime(2011, 06, 10, 11, 0, 0));
            var startDate = new DateTime(2011, 06, 9);
            var workTime = new WorkTime(currentTime, "8:00", startDate);
            const double expectedCurrentTimeSpent = 27d;
            Assert.AreEqual(expectedCurrentTimeSpent, workTime.TimeSpent.TotalHours);
        }

        #endregion


        #region MinTime

        [Test]
        public void MinTimeStart()
        {
            var expected = _w.StartTime.AddHours(6);
            Assert.AreEqual(expected, _w.MinTimeStart);
        }

        [Test]
        public void MinTimeEnd()
        {
            var expected = _w.StartTime.AddHours(6).AddMinutes(45);
            Assert.AreEqual(expected, _w.MinTimeEnd);
        }

        [Test]
        public void TimeTillMinTime()
        {
            var expected = _w.MinTimeStart.Subtract(_clock.Now);
            Assert.AreEqual(expected, _w.RemainingTillMinTime);
        }

        #endregion

        #region MaxTime

        [Test]
        public void MaxTime()
        {
            var expected = _w.StartTime.AddHours(10).AddMinutes(45);
            Assert.AreEqual(expected, _w.MaxTime);
        }

        [Test]
        public void TimeTillMaxTime()
        {
            var expected = _w.MaxTime.Subtract(_clock.Now);
            Assert.AreEqual(expected, _w.RemainingTillMaxTime);
        }

        [Test]
        public void TimeTillMaxTime_WorkedTooLong()
        {
            IClock workedTooLong = new StaticClock(new DateTime(2011, 06, 10, 19, 0, 0));
            var workTime = new WorkTime(workedTooLong, "8:00");
            var expected = workTime.MaxTime.Subtract(workedTooLong.Now);
            Assert.AreEqual(expected, workTime.RemainingTillMaxTime);
        }

        #endregion

        #region Balance

        [Test]
        public void Balance_UnderMinTimeStart()
        {
            // current time: 10:00
            IClock workedTooShort = new StaticClock(new DateTime(2011, 06, 10, 10, 0, 0));
            var workTime = new WorkTime(workedTooShort, "8:00");
            var minusTime = new TimeSpan(-6, 0, 0);
            Assert.AreEqual(minusTime, workTime.Balance);
        }

        [Test]
        public void Balance_UnderMinTimeStart_JustStarted()
        {
            // current time: 8:15
            IClock workedTooShort = new StaticClock(new DateTime(2011, 06, 10, 8, 15, 0));
            var workTime = new WorkTime(workedTooShort, "8:00");
            var minusTime = new TimeSpan(-7, -45, 0);
            Assert.AreEqual(minusTime, workTime.Balance);
        }

        [Test]
        public void Balance_UnderMinTimeEnd()
        {
            // current time: 14:30
            IClock workedTooShort = new StaticClock(new DateTime(2011, 06, 10, 14, 30, 0));
            var workTime = new WorkTime(workedTooShort, "8:00");
            var minusTime = new TimeSpan(-2, 0, 0);
            Assert.AreEqual(minusTime, workTime.Balance);
        }

        [Test]
        public void Balance_UnderTargetTime()
        {
            // current time: 15:30
            IClock workedTooShort = new StaticClock(new DateTime(2011, 06, 10, 15, 30, 0));
            var workTime = new WorkTime(workedTooShort, "8:00");
            var minusTime = new TimeSpan(-1, -15, 0);
            Assert.AreEqual(minusTime, workTime.Balance);
        }

        [Test]
        public void Balance_AboveTargetTime()
        {
            // current time: 18:30
            IClock workedTooShort = new StaticClock(new DateTime(2011, 06, 10, 18, 30, 0));
            var workTime = new WorkTime(workedTooShort, "8:00");
            var minusTime = new TimeSpan(1, 45, 0);
            Assert.AreEqual(minusTime, workTime.Balance);
        }

        [Test]
        public void Balance_AboveMaxTime()
        {
            // current time: 19:30
            IClock workedTooShort = new StaticClock(new DateTime(2011, 06, 10, 19, 30, 0));
            var workTime = new WorkTime(workedTooShort, "8:00");
            var minusTime = new TimeSpan(2, 0, 0);
            Assert.AreEqual(minusTime, workTime.Balance);
        }

        [Test]
        public void TargetTime_NextDay()
        {
            // current time: 00:05
            // start time: 23:00 (previous day)
            IClock currentTime = new StaticClock(new DateTime(2011, 06, 10, 0, 5, 0));
            var startDate = new DateTime(2011, 06, 9);
            var workTime = new WorkTime(currentTime, "23:00", startDate);
            var minusTime = new TimeSpan(-6, -55, 0);
            Assert.AreEqual(minusTime, workTime.Balance);
        }

        #endregion
    }

}
