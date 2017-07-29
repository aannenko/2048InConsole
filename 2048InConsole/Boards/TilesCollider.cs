using System.Runtime.Serialization;
using _2048InConsole.Tiles;

namespace _2048InConsole.Boards
{
    [DataContract]
    internal class TilesCollider : IBoardItemsCollider<ITile>
    {
        public bool TryCollide(ITile sourceTile, ITile destinationTile, out ITile resultingTile)
        {
            resultingTile = null;

            if (sourceTile == null) return false;

            if (destinationTile == null)
            {
                resultingTile = new Tile(sourceTile.Rank, sourceTile.Id, null);
                return true;
            }

            if (destinationTile.MergedWith.HasValue || sourceTile.Rank != destinationTile.Rank) return false;

            resultingTile = new Tile(sourceTile.Rank * 2, sourceTile.Id, destinationTile.Id);
            return true;
        }
    }
}
