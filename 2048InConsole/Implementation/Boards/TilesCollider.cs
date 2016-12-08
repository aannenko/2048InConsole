using System;
using _2048Unlimited.Model.Abstraction.Boards;
using _2048Unlimited.Model.Abstraction.Tiles;
using _2048Unlimited.Model.Implementation.Tiles;

namespace _2048Unlimited.Model.Implementation.Boards
{
    internal class TilesCollider : IBoardItemsCollider<ITile>
    {
        public bool TryCollide(ITile sourceTile, ITile destinationTile, out ITile resultingTile)
        {
            resultingTile = null;

            if (sourceTile == null) return false;

            if (destinationTile == null)
            {
                resultingTile = new PrivateTile(sourceTile.Rank, sourceTile.Id, null);
                return true;
            }

            if (destinationTile.MergedWith.HasValue || sourceTile.Rank != destinationTile.Rank) return false;

            resultingTile = new PrivateTile(sourceTile.Rank * 2, sourceTile.Id, destinationTile.Id);
            return true;
        }

        private class PrivateTile : Tile
        {
            internal PrivateTile(int rank, Guid id, Guid? mergedWith) : base(rank, id, mergedWith)
            {
            }
        }
    }
}
