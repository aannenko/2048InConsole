using System.Collections.Generic;

namespace _2048InConsole.Helpers.Comparers
{
    internal class PositionComparerFactory
    {
        internal Comparer<Position> GetComparer(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new PositionComparerForMoveUp();
                case Direction.Left:
                    return new PositionComparerForMoveLeft();
                case Direction.Down:
                    return new PositionComparerForMoveDown();
                default:
                    return new PositionComparerForMoveRight();
            }
        }
    }
}
