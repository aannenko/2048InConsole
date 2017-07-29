using System;

namespace _2048InConsole.Stats
{
    public interface IGlobalStatistics : IStatistics
    {
        void Update(double newScore, TimeSpan newElapsedTime, ITileStatistics bestTile);
    }
}
