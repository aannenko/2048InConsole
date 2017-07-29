using System;
using System.Runtime.Serialization;

namespace _2048InConsole.Stats
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
