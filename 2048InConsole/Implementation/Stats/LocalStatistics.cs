using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using _2048Unlimited.Model.Abstraction.Stats;

namespace _2048Unlimited.Model.Implementation.Stats
{
    [DataContract]
    internal class LocalStatistics : Statistics, ILocalStatistics
    {
        public ITileStatistics BestTileStatistics { get; private set; }

        public ILocalStatistics Update(double newScore, TimeSpan newElapsedTime, int bestTileRank)
        {
            var newMoves = Moves < double.MaxValue ? Moves + 1 : double.MaxValue;

            var newStat = new LocalStatistics
            {
                Score = Score < newScore ? newScore : Score,
                Moves = newMoves,
                ElapsedTime = ElapsedTime < newElapsedTime ? newElapsedTime : ElapsedTime,
                TilesStatisticsProtected = new SortedDictionary<int, ITileStatistics>(TilesStatisticsProtected),
                BestTileStatistics = new TileStatistics(bestTileRank, newMoves, newElapsedTime)
            };

            newStat.TryUpdateTilesStatistics(BestTileStatistics);

            return newStat;
        }
    }
}
