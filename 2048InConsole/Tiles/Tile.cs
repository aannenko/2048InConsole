using System;
using System.Runtime.Serialization;

namespace _2048InConsole.Tiles
{
    [DataContract]
    internal class Tile : ITile
    {
        private static readonly Random Random = new Random();

        [DataMember(IsRequired = true)]
        public int Rank { get; private set; }

        [DataMember(IsRequired = true)]
        public Guid Id { get; private set; }

        [DataMember(IsRequired = true)]
        public Guid? MergedWith { get; private set; }

        internal Tile() : this(Random.Next(10) > 8 ? 4 : 2, Guid.NewGuid(), null)
        {
        }

        internal Tile(int rank, Guid id, Guid? mergedWith)
        {
            Rank = rank;
            Id = id;
            MergedWith = mergedWith;
        }
    }
}