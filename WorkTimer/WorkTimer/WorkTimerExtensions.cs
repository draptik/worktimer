using System;

namespace WorkTimer
{
    public static class WorkTimerExtensions
    {
        public static string ToDisplayString(this TimeSpan timeSpan)
        {
            var sign = (timeSpan.Hours == 0 && (timeSpan.Minutes < 0 || timeSpan.Seconds < 0)) ? "-" : string.Empty;
            return string.Format("{0}{1:00}:{2:00}:{3:00}", sign, timeSpan.Hours, Math.Abs(timeSpan.Minutes),
                                 Math.Abs(timeSpan.Seconds));
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }
    }
}
