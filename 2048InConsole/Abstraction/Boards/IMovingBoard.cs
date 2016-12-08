using _2048Unlimited.Model.Helpers;

namespace _2048Unlimited.Model.Abstraction.Boards
{
    internal interface IMovingBoard<T> : IBoard<T>
    {
        bool IsMoveAvailable { get; }

        bool TryMove(Direction direction, out IMovingBoard<T> movingBoard);
    }
}
