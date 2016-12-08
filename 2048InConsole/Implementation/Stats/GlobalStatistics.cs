using System;
using System.Runtime.Serialization;
using _2048Unlimited.Model.Abstraction.Stats;

namespace _2048Unlimited.Model.Implementation.Stats
{
    [DataContract]
    internal class GlobalStatistics : Statistics, IStatisticsInternal
    {
        [DataMember(IsRequired = true)]
        private TimeSpan _previousElapsedTime = TimeSpan.Zero;

        public IStatisticsInternal Update(double newScore, TimeSpan newElapsedTime, int bestTileRank)
        {
            var newMoves = Moves < double.MaxValue ? Moves + 1 : double.MaxValue;

            Score = Score < newScore ? newScore : Score;
            Moves = newMoves;
            ElapsedTime = ElapsedTime + (newElapsedTime - _previousElapsedTime);
            _previousElapsedTime = newElapsedTime;

            if (bestTileRank >= MinTileRankForDictionaries && TryUpdateTimesDictionary(bestTileRank, newElapsedTime))
                TryUpdateMovesDictionary(bestTileRank, newMoves);

            return this;
        }
    }
}
