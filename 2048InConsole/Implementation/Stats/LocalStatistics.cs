using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using _2048Unlimited.Model.Abstraction.Stats;

namespace _2048Unlimited.Model.Implementation.Stats
{
    [DataContract]
    internal class LocalStatistics : Statistics, IStatisticsInternal
    {
        public IStatisticsInternal Update(double newScore, TimeSpan newElapsedTime, int bestTileRank)
        {
            var newMoves = Moves < double.MaxValue ? Moves + 1 : double.MaxValue;

            var newStat = new LocalStatistics
            {
                Score = Score < newScore ? newScore : Score,
                Moves = newMoves,
                ElapsedTime = ElapsedTime < newElapsedTime ? newElapsedTime : ElapsedTime,
                TimeToTileDictionaryProtected = new SortedDictionary<int, TimeSpan>(TimeToTileDictionaryProtected),
                MovesToTileDictionaryProtected = new SortedDictionary<int, double>(MovesToTileDictionaryProtected)
            };

            if (bestTileRank >= MinTileRankForDictionaries && newStat.TryUpdateTimesDictionary(bestTileRank, newElapsedTime))
                newStat.TryUpdateMovesDictionary(bestTileRank, newMoves);

            return newStat;
        }
    }
}
