﻿using System;

namespace WorkTimer
{
    public static class WorkTimerExtensions
    {
        public static string ToDisplayString(this TimeSpan timeSpan)
        {
            var sign = (timeSpan.Hours == 0 && (timeSpan.Minutes < 0 || timeSpan.Seconds < 0)) ? "-" : string.Empty;

            var hours = timeSpan.Hours;
            if (timeSpan.Days < 0 && timeSpan.Hours < 0) {
                hours = Math.Abs(timeSpan.Hours + (timeSpan.Days*24));
                sign = "-";
            }

            return string.Format("{0}{1:00}:{2:00}:{3:00}", sign, hours, Math.Abs(timeSpan.Minutes),
                                 Math.Abs(timeSpan.Seconds));
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }
    }
}
