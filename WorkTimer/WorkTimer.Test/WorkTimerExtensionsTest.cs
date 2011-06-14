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

    }


}
