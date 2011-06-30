using System;

namespace WorkTimer
{
    public interface IClock
    {
        DateTime Now { get; }
    }
}
