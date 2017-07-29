using System;

namespace _2048InConsole.Helpers
{
    public class TimerTickEventArgs : EventArgs
    {
        public readonly TimeSpan ElapsedTime;

        public TimerTickEventArgs(TimeSpan elapsedTime)
        {
            ElapsedTime = elapsedTime;
        }
    }
}
