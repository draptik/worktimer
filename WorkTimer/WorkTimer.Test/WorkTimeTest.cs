using System;
using NUnit.Framework;

namespace WorkTimer.Test
{
    [TestFixture]
    public class WorkTimeTest
    {
        private WorkTime _workTime;
        private WorkTime w;
        private IClock _clock;

        [SetUp]
        public void Setup()
        {
            _clock = new StaticClock();
            w = new WorkTime(_clock, "8:00");
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
            var inValidInput = "invalid";
            _workTime = new WorkTime(_clock, inValidInput);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void StartTime_InputTimeInvalid_TooSmall()
        {
            var inValidInput = "-8:00";
            _workTime = new WorkTime(_clock, inValidInput);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void StartTime_InputTimeInvalid_TooLarge()
        {
            var inValidInput = "25:00";
            _workTime = new WorkTime(_clock, inValidInput);
        }


        [Test, ExpectedException(typeof(ArgumentException))]
        public void StartTime_InputTimeInvalid_WrongFormat_Point()
        {
            var inValidInput = "8.00";
            _workTime = new WorkTime(_clock, inValidInput);
        }

        #endregion

        #endregion

        #region TargetTime

        [Test]
        public void TargetTime_IsNotNull_For_Valid_StartTime()
        {
            var targetTime =_workTime.TargetTime;
            Assert.IsNotNull(targetTime);
        }

        [Test]
        public void TargetTime_IsCorrect_For_Valid_StartTime()
        {
            var targetTime = w.TargetTime;
            Assert.AreEqual(w.StartTime + new TimeSpan(8, 45, 0), targetTime);
        }

        #endregion

        #region RemainingTime Till Target

        [Test]
        public void RemainingTimeTarget()
        {
            var expected = w.TargetTime - _clock.Now;
            Assert.AreEqual(expected, w.RemainingTillTarget);
        }

        #endregion

        #region Current Time Spent

        [Test]
        public void CurrentTimeSpent()
        {
            var expected = w.TargetTimeSpan - w.RemainingTillTarget;
            Assert.AreEqual(expected, w.TimeSpent);
        }

        #endregion


        #region MinTime

        [Test]
        public void MinTimeStart()
        {
            var expected = w.StartTime.AddHours(6);
            Assert.AreEqual(expected, w.MinTimeStart);
        }

        [Test]
        public void MinTimeEnd()
        {
            var expected = w.StartTime.AddHours(6).AddMinutes(45);
            Assert.AreEqual(expected, w.MinTimeEnd);
        }

        [Test]
        public void TimeTillMinTime()
        {
            var expected = w.MinTimeStart.Subtract(_clock.Now);
            Assert.AreEqual(expected, w.RemainingTillMinTime);
        }

        #endregion

        #region MaxTime

        [Test]
        public void MaxTime()
        {
            var expected = w.StartTime.AddHours(10).AddMinutes(45);
            Assert.AreEqual(expected, w.MaxTime);
        }

        [Test]
        public void TimeTillMaxTime()
        {
            var expected = w.MaxTime.Subtract(_clock.Now);
            Assert.AreEqual(expected, w.RemainingTillMaxTime);
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
            var minusTime = new TimeSpan(-6, -45, 0);
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

        #endregion
    }

}
