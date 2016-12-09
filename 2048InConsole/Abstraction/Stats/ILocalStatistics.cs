using System;

namespace _2048Unlimited.Model.Abstraction.Stats
{
    internal interface ILocalStatistics : IStatistics
    {
        ITileStatistics BestTileStatistics { get; }

        ILocalStatistics Update(double newScore, TimeSpan newElapsedTime, int bestTileRank);
    }
}
