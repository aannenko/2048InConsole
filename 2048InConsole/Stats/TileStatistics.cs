using System;
using System.Runtime.Serialization;

namespace _2048InConsole.Stats
{
    [DataContract]
    internal class TileStatistics : ITileStatistics
    {
        [DataMember(IsRequired = true)]
        public int Rank { get; private set; }

        [DataMember(IsRequired = true)]
        public double Moves { get; private set; }

        [DataMember(IsRequired = true)]
        public TimeSpan Time { get; private set; }

        internal TileStatistics(int rank, double moves, TimeSpan time)
        {
            Rank = rank;
            Moves = moves;
            Time = time;
        }
    }
}
