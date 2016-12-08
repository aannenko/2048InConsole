using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2048Unlimited.Model.Abstraction.Boards;
using _2048Unlimited.Model.Abstraction.Tiles;
using _2048Unlimited.Model.Helpers;
using _2048Unlimited.Model.Helpers.Comparers;

namespace _2048Unlimited.Model.Implementation.Boards
{
    internal class TilesMover : IBoardItemsMover<ITile>
    {
        private const string ColliderNullMessage = "Board Items Collider can't be null";

        private readonly IBoardItemsCollider<ITile> _collider;

        internal TilesMover(IBoardItemsCollider<ITile> collider)
        {
            ValidateInput(collider);
            _collider = collider;
        }

        public bool TryMove(IBoard<ITile> board, Direction direction, out IDictionary<Position, ITile> items)
        {
            var isMoveDone = false;
            var newItems = new ConcurrentDictionary<Position, ITile>();
            Parallel.ForEach(GetLines(board, direction), line =>
            {
                foreach (var position in line)
                {
                    ITile sourceItem;
                    if (!board.Items.TryGetValue(position, out sourceItem)) continue;

                    var lastGoodPosition = position;
                    ITile lastGoodItem;
                    _collider.TryCollide(sourceItem, null, out lastGoodItem);

                    var positionToCheck = position.GetPositionFrom(direction);
                    while (board.IsPositionWithinBoard(positionToCheck))
                    {
                        ITile destinationItem;
                        newItems.TryGetValue(positionToCheck, out destinationItem);

                        ITile resultingItem;
                        if (!_collider.TryCollide(sourceItem, destinationItem, out resultingItem)) break;

                        isMoveDone = true;
                        lastGoodPosition = positionToCheck;
                        lastGoodItem = resultingItem;
                        positionToCheck = positionToCheck.GetPositionFrom(direction);
                    }

                    newItems[lastGoodPosition] = lastGoodItem;
                }
            });

            items = newItems;
            return isMoveDone;
        }

        public bool GetIsMoveAvailable(IBoard<ITile> board)
        {
            if (board.Items.Count < board.Columns * board.Rows) return true;

            var isMoveAvailable = false;
            var directionsToCheck = Enum.GetValues(typeof(Direction)).Cast<Direction>();
            Parallel.ForEach(board.Items, (pair, loopState) =>
            {
                foreach (var direction in directionsToCheck)
                {
                    var positionToCheck = pair.Key.GetPositionFrom(direction);
                    if (!board.IsPositionWithinBoard(positionToCheck)) continue;

                    ITile destinationItem;
                    board.Items.TryGetValue(positionToCheck, out destinationItem);

                    ITile resultingItem;
                    if (!_collider.TryCollide(pair.Value, destinationItem, out resultingItem)) continue;

                    isMoveAvailable = true;
                    loopState.Stop();
                }
            });

            return isMoveAvailable;
        }

        private static IEnumerable<List<Position>> GetLines(IBoard<ITile> board, Direction direction)
        {
            var isRow = direction == Direction.Left || direction == Direction.Right;

            var lines = new ConcurrentBag<List<Position>>();
            Parallel.ForEach(isRow
                ? board.Items.Keys.Select(k => k.Row).Distinct()
                : board.Items.Keys.Select(k => k.Column).Distinct(),
                item =>
            {
                var line = new List<Position>(
                    board.Items.Keys.Where(key => isRow ? key.Row == item : key.Column == item));
                line.Sort(PositionComparer.Factory.GetComparer(direction));
                lines.Add(line);
            });

            return lines;
        }

        private void ValidateInput(IBoardItemsCollider<ITile> collider)
        {
            if (collider == null)
                throw new ArgumentNullException(nameof(collider), ColliderNullMessage);
        }
    }
}
