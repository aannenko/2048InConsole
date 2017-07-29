using System;
using System.Collections.Generic;

namespace _2048InConsole.Helpers.Comparers
{
    internal abstract class PositionComparer : Comparer<Position>
    {
        private static readonly Lazy<PositionComparerFactory> FactoryInstance = new Lazy<PositionComparerFactory>(() => new PositionComparerFactory());

        internal static PositionComparerFactory Factory => FactoryInstance.Value;

        public abstract override int Compare(Position x, Position y);
    }
}
