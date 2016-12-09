using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using _2048Unlimited.Model.Abstraction.Stats;

namespace _2048Unlimited.Model.Implementation.Stats
{
    [DataContract]
    internal abstract class Statistics : IStatistics
    {
        private const int MinTileRankForDictionaries = 8;

        [DataMember(IsRequired = true)]
        public double Score { get; protected set; }

        [DataMember(IsRequired = true)]
        public double Moves { get; protected set; }

        [DataMember(IsRequired = true)]
        public TimeSpan ElapsedTime { get; protected set; }

        public IReadOnlyDictionary<int, ITileStatistics> TilesStatistics =>
            new ReadOnlyDictionary<int, ITileStatistics>(TilesStatisticsProtected);

        [DataMember(IsRequired = true)]
        protected SortedDictionary<int, ITileStatistics> TilesStatisticsProtected { get; set; } =
            new SortedDictionary<int, ITileStatistics>();
        
        protected bool TryUpdateTilesStatistics(ITileStatistics statistics)
        {
            if (statistics == null || statistics.Rank < MinTileRankForDictionaries) return false;

            if (TilesStatisticsProtected.ContainsKey(statistics.Rank))
            {
                var stats = TilesStatisticsProtected[statistics.Rank];
                var moves = stats.Moves;
                var time = stats.Time;
                var isUpdateNeeded = false;

                if (moves > statistics.Moves)
                {
                    moves = statistics.Moves;
                    isUpdateNeeded = true;
                }

                if (time > statistics.Time)
                {
                    time = statistics.Time;
                    isUpdateNeeded = true;
                }

                if (!isUpdateNeeded) return false;

                TilesStatisticsProtected[statistics.Rank] = new TileStatistics(statistics.Rank, moves, time);
                return true;
            }

            TilesStatisticsProtected.Add(statistics.Rank, new TileStatistics(statistics.Rank, statistics.Moves, statistics.Time));
            return true;
        }
    }
}
