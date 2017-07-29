using System;
using System.Runtime.Serialization;
using System.Timers;
using _2048InConsole.Helpers;

namespace _2048InConsole.Timers
{
    [DataContract]
    internal class ElapsingTimer : ITimer
    {
        private Timer _timer;

        public bool IsTicking => Timer.Enabled;

        private Timer Timer
        {
            get
            {
                if (_timer == null)
                {
                    _timer = new Timer(1000);
                    _timer.Elapsed += OnElapsed;
                    _timer.Start();
                }

                return _timer;
            }
        }

        [DataMember(IsRequired = true)]
        public TimeSpan ElapsedTime { get; private set; }

        internal ElapsingTimer(TimeSpan elapsedTime = default(TimeSpan))
        {
            ElapsedTime = elapsedTime;
            Timer.Start();
        }

        public void Start()
        {
            Timer.Start();
        }

        public void Stop()
        {
            Timer.Stop();
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
