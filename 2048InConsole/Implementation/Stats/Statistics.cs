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
        protected const int MinTileRankForDictionaries = 256;

        [DataMember(IsRequired = true)]
        public double Score { get; protected set; }

        [DataMember(IsRequired = true)]
        public double Moves { get; protected set; }

        [DataMember(IsRequired = true)]
        public TimeSpan ElapsedTime { get; protected set; }

        public IReadOnlyDictionary<int, TimeSpan> TimeToTileDictionary => new ReadOnlyDictionary<int, TimeSpan>(TimeToTileDictionaryProtected);

        public IReadOnlyDictionary<int, double> MovesToTileDictionary => new ReadOnlyDictionary<int, double>(MovesToTileDictionaryProtected);
        
        [DataMember(IsRequired = true)]
        protected SortedDictionary<int, TimeSpan> TimeToTileDictionaryProtected { get; set; } = new SortedDictionary<int, TimeSpan>();

        [DataMember(IsRequired = true)]
        protected SortedDictionary<int, double> MovesToTileDictionaryProtected { get; set; } = new SortedDictionary<int, double>();

        protected bool TryUpdateTimesDictionary(int bestTileRank, TimeSpan elapsedTime) =>
            TryUpdateDictionary(TimeToTileDictionaryProtected, bestTileRank, elapsedTime);

        protected bool TryUpdateMovesDictionary(int bestTileRank, double moves) =>
            TryUpdateDictionary(MovesToTileDictionaryProtected, bestTileRank, moves);

        private static bool TryUpdateDictionary<T>(IDictionary<int, T> dictionary, int bestTileRank, T value)
            where T : IComparable<T>
        {
            if (dictionary.ContainsKey(bestTileRank))
            {
                if (dictionary[bestTileRank].CompareTo(value) <= 0) return false;

                dictionary[bestTileRank] = value;
                return true;
            }

            dictionary.Add(bestTileRank, value);
            return true;
        }
    }
}
