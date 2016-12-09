using System;

namespace _2048Unlimited.Model.Abstraction.Stats
{
    internal interface IGlobalStatistics : IStatistics
    {
        void Update(double newScore, TimeSpan newElapsedTime, ITileStatistics bestTile);
    }
}
