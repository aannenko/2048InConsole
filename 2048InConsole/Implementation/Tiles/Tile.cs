using System;
using System.Runtime.Serialization;
using _2048Unlimited.Model.Abstraction.Tiles;

namespace _2048Unlimited.Model.Implementation.Tiles
{
    [DataContract]
    internal class Tile : ITile
    {
        private static readonly Random Random = new Random();

        [DataMember(IsRequired = true)]
        public int Rank { get; }

        [DataMember(IsRequired = true)]
        public Guid Id { get; }

        [DataMember(IsRequired = true)]
        public Guid? MergedWith { get; }

        internal Tile() : this(Random.Next(10) > 8 ? 4 : 2, Guid.NewGuid(), null)
        {
        }

        protected Tile(int rank, Guid id, Guid? mergedWith)
        {
            Rank = rank;
            Id = id;
            MergedWith = mergedWith;
        }
    }
}