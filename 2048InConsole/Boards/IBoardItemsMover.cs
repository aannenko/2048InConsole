using System.Collections.Generic;
using _2048InConsole.Helpers;

namespace _2048InConsole.Boards
{
    internal interface IBoardItemsMover<T>
    {
        bool TryMove(IBoard<T> board, Direction direction, out IDictionary<Position, T> items);

        bool GetIsMoveAvailable(IBoard<T> board);
    }
}
