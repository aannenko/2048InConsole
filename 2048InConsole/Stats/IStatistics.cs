using System;
using System.Collections.Generic;

namespace _2048InConsole.Stats
{
    public interface IStatistics
    {
        double Score { get; }

        double Moves { get; }

        TimeSpan ElapsedTime { get; }

        IReadOnlyDictionary<int, ITileStatistics> TilesStatistics { get; }
    }
}