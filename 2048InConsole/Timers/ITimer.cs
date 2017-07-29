using System;
using _2048InConsole.Helpers;

namespace _2048InConsole.Timers
{
    internal interface ITimer
    {
        bool IsTicking { get; }

        TimeSpan ElapsedTime { get; }

        void Start();

        void Stop();

        void SetElapsedTime(TimeSpan elapsedTime);

        event EventHandler<TimerTickEventArgs> Tick;
    }
}
