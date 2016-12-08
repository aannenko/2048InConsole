using System;
using _2048Unlimited.Model.Helpers;

namespace _2048Unlimited.Model.Abstraction.Timers
{
    internal interface ITimer
    {
        TimeSpan ElapsedTime { get; }

        void Start();

        void Stop();

        void SetElapsedTime(TimeSpan elapsedTime);

        event EventHandler<TimerTickEventArgs> Tick;
    }
}
