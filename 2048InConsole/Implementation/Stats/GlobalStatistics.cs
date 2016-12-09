using System;
using System.Runtime.Serialization;
using _2048Unlimited.Model.Abstraction.Stats;

namespace _2048Unlimited.Model.Implementation.Stats
{
    [DataContract]
    internal class GlobalStatistics : Statistics, IGlobalStatistics
    {
        [DataMember(IsRequired = true)]
        private TimeSpan _previousElapsedTime = TimeSpan.Zero;

        public void Update(double newScore, TimeSpan newElapsedTime, ITileStatistics bestTile)
        {
            var newMoves = Moves < double.MaxValue ? Moves + 1 : double.MaxValue;

            Score = Score < newScore ? newScore : Score;
            Moves = newMoves;

            if (newElapsedTime < _previousElapsedTime)
                _previousElapsedTime = TimeSpan.Zero;
            ElapsedTime = ElapsedTime + (newElapsedTime - _previousElapsedTime);
            _previousElapsedTime = newElapsedTime;

            TryUpdateTilesStatistics(bestTile);
        }
    }
}
