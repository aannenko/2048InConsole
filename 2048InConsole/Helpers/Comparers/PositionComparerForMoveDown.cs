namespace _2048InConsole.Helpers.Comparers
{
    internal class PositionComparerForMoveDown : PositionComparer
    {
        public override int Compare(Position x, Position y)
        {
            if (x.Column > y.Column) return 1;
            if (x.Column < y.Column) return -1;
            if (x.Row > y.Row) return -1;
            return x.Row < y.Row ? 1 : 0;
        }
    }
}