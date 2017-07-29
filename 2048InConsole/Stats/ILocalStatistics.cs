using System;

namespace _2048InConsole.Stats
{
    internal interface ILocalStatistics : IStatistics
    {
        ITileStatistics BestTileStatistics { get; }

        ILocalStatistics Update(double newScore, TimeSpan newElapsedTime, int bestTileRank);
    }
}
