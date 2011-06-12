using System;
using NUnit.Framework;

namespace WorkTimer.Test
{
    [TestFixture]
    public class WorkTimerExtensionsTest
    {

        [Test]
        public void TestToDisplayString()
        {
            var timeSpan = new TimeSpan(-1, -45, -30);
            var result = timeSpan.ToDisplayString();
            Assert.AreEqual("-01:45:30", result);
        }
    }
}
