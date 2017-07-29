using System.Collections.Generic;
using _2048InConsole.Helpers;

namespace _2048InConsole.Boards
{
    internal interface IBoard<T>
    {
        byte Columns { get; }

        byte Rows { get; }

        IReadOnlyDictionary<Position, T> Items { get; }

        bool IsPositionWithinBoard(Position position);
    }
}
