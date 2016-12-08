using System;

namespace _2048Unlimited.Model.Abstraction.Stats
{
    internal interface IStatisticsInternal : IStatistics
    {
        IStatisticsInternal Update(double newScore, TimeSpan newElapsedTime, int bestTileRank);
    }
}
