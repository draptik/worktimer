using System;
using NUnit.Framework;

namespace WorkTimer.Test
{
    [TestFixture]
    public class WorkTimerExtensionsTest
    {

        [Test]
        public void TestToDisplayString_AllNegative()
        {
            var timeSpan = new TimeSpan(-1, -45, -30);
            var result = timeSpan.ToDisplayString();
            Assert.AreEqual("-01:45:30", result);
        }

        [Test]
        public void TestToDisplayString_LeadingZero_AllNegative()
        {
            var timeSpan = new TimeSpan(0, -45, -30);
            var result = timeSpan.ToDisplayString();
            Assert.AreEqual("-00:45:30", result);
        }

        [Test]
        public void TestToDisplayString_LeadingZeroNegativeSeconds()
        {
            var timeSpan = new TimeSpan(0, 0, -30);
            var result = timeSpan.ToDisplayString();
            Assert.AreEqual("-00:00:30", result);
        }

        [Test]
        public void TestToDisplayString_Zero()
        {
            var timeSpan = new TimeSpan(0, 0, 0);
            var result = timeSpan.ToDisplayString();
            Assert.AreEqual("00:00:00", result);
        }

        [Test]
        public void TestToDisplayString_NegativeDays()
        {
            var timeSpan = new TimeSpan(-1, -23, -59, -59);
            var result = timeSpan.ToDisplayString();
            Assert.AreEqual("-47:59:59", result);
        }

        [Test]
        public void TestToDisplayString_NegativeDaysSmall()
        {
            var timeSpan = new TimeSpan(0, -23, -59, -59);
            var result = timeSpan.ToDisplayString();
            Assert.AreEqual("-23:59:59", result);
        }

        [Test]
        public void TestToDisplayString_NegativeDaysVerySmall()
        {
            var timeSpan = new TimeSpan(0, 0, 0, -59);
            var result = timeSpan.ToDisplayString();
            Assert.AreEqual("-00:00:59", result);
        }

        [Test]
        public void TestToDisplayString_WorkedMoreThanADay()
        {
            var timeSpan = new TimeSpan(1, 0, 0, 1); // 1 day, 1 second
            var result = timeSpan.ToDisplayString();
            Assert.AreEqual("24:00:01", result);
        }
    }


}
