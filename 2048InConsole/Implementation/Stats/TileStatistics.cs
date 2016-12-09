using System;
using _2048Unlimited.Model.Abstraction.Stats;

namespace _2048Unlimited.Model.Implementation.Stats
{
    internal class TileStatistics : ITileStatistics
    {
        public int Rank { get; }

        public double Moves { get; }

        public TimeSpan Time { get; }

        internal TileStatistics(int rank, double moves, TimeSpan time)
        {
            Rank = rank;
            Moves = moves;
            Time = time;
        }
    }
}
