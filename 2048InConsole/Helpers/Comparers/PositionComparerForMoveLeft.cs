namespace _2048Unlimited.Model.Helpers.Comparers
{
    internal class PositionComparerForMoveLeft : PositionComparer
    {
        public override int Compare(Position x, Position y)
        {
            if (x.Row > y.Row) return 1;
            if (x.Row < y.Row) return -1;
            if (x.Column > y.Column) return 1;
            return x.Column < y.Column ? -1 : 0;
        }
    }
}