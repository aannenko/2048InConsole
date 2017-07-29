using _2048InConsole.Helpers;

namespace _2048InConsole.Boards
{
    internal interface IMovingBoard<T> : IBoard<T>
    {
        bool IsMoveAvailable { get; }

        bool TryMove(Direction direction, out IMovingBoard<T> movingBoard);
    }
}
