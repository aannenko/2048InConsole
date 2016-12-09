using System;

namespace _2048Unlimited.Model.Abstraction.Stats
{
    public interface ITileStatistics
    {
        int Rank { get; }

        double Moves { get; }

        TimeSpan Time { get; }
    }
}
