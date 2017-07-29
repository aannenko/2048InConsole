using System;

namespace _2048InConsole.Stats
{
    public interface ITileStatistics
    {
        int Rank { get; }

        double Moves { get; }

        TimeSpan Time { get; }
    }
}
