using System.Collections.Generic;
using _2048Unlimited.Model.Helpers;

namespace _2048Unlimited.Model.Abstraction.Boards
{
    internal interface IBoard<T>
    {
        byte Columns { get; }

        byte Rows { get; }

        IReadOnlyDictionary<Position, T> Items { get; }

        bool IsPositionWithinBoard(Position position);
    }
}
