using System.Collections.Generic;
using System.Runtime.Serialization;

namespace _2048InConsole.Helpers
{
    [DataContract]
    public struct Position
    {
        [DataMember(IsRequired = true)]
        private readonly int _column;

        [DataMember(IsRequired = true)]
        private readonly int _row;

        public int Column => _column;

        public int Row => _row;

        internal Position(int column, int row)
        {
            _column = column;
            _row = row;
        }

        internal Position GetPositionFrom(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return new Position(_column - 1, _row);
                case Direction.Right:
                    return new Position(_column + 1, _row);
                case Direction.Up:
                    return new Position(_column, _row - 1);
                default:
                    return new Position(_column, _row + 1);
            }
        }

        internal IEnumerable<Position> GetPositionsRange(Position lastPosition)
        {
            for (int col = Column; Column <= lastPosition.Column ? col <= lastPosition.Column : lastPosition.Column <= col; col = Column <= lastPosition.Column ? col + 1 : col - 1)
            {
                for (int row = Row; Row <= lastPosition.Row ? row <= lastPosition.Row : lastPosition.Row <= row; row = Row <= lastPosition.Row ? row + 1 : row - 1)
                {
                    yield return new Position(col, row);
                }
            }
        }

        public static bool operator ==(Position first, Position second)
        {
            return first._column == second._column && first._row == second._row;
        }

        public static bool operator !=(Position first, Position second)
        {
            return !(first == second);
        }

        public bool Equals(Position other)
        {
            return _column == other._column && _row == other._row;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Position && Equals((Position) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_column*397) ^ _row;
            }
        }
    }
}