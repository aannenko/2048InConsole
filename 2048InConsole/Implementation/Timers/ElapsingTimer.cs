using System;
using System.Timers;
using _2048Unlimited.Model.Abstraction.Timers;
using _2048Unlimited.Model.Helpers;

namespace _2048Unlimited.Model.Implementation.Timers
{
    internal class ElapsingTimer : ITimer
    {
        private readonly Timer _timer = new Timer(1000);

        public TimeSpan ElapsedTime { get; private set; }

        internal ElapsingTimer(TimeSpan elapsedTime = default(TimeSpan))
        {
            ElapsedTime = elapsedTime;
            _timer.Elapsed += OnElapsed;
            _timer.Start();
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void SetElapsedTime(TimeSpan elapsedTime)
        {
            Stop();
            ElapsedTime = elapsedTime;
            Start();
        }

        public event EventHandler<TimerTickEventArgs> Tick;

        private void OnElapsed(object sender, object o)
        {
            ElapsedTime += TimeSpan.FromSeconds(1);
            Tick?.Invoke(this, new TimerTickEventArgs(ElapsedTime));
        }
    }
}
