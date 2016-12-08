using System;
using System.Collections.Generic;

namespace _2048Unlimited.Model.Abstraction.Stats
{
    public interface IStatistics
    {
        double Score { get; }

        double Moves { get; }

        TimeSpan ElapsedTime { get; }

        IReadOnlyDictionary<int, TimeSpan> TimeToTileDictionary { get; }

        IReadOnlyDictionary<int, double> MovesToTileDictionary { get; }
    }
}