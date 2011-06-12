using System;

namespace WorkTimer
{
    public static class WorkTimerExtensions
    {
        public static string ToDisplayString(this TimeSpan timeSpan)
        {

            return string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, Math.Abs(timeSpan.Minutes), Math.Abs(timeSpan.Seconds));
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }
    }
}
