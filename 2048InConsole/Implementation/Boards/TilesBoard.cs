using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using _2048Unlimited.Model.Abstraction.Boards;
using _2048Unlimited.Model.Abstraction.Tiles;
using _2048Unlimited.Model.Helpers;
using _2048Unlimited.Model.Implementation.Tiles;

namespace _2048Unlimited.Model.Implementation.Boards
{
    [DataContract]
    internal class TilesBoard : Board<ITile>, IMovingBoard<ITile>
    {
        private const string MoverNullMessage = "Board Items Mover can't be null";

        private static readonly Random Random = new Random();

        [DataMember(IsRequired = true)]
        private static IBoardItemsMover<ITile> _mover;

        [DataMember(IsRequired = true)]
        public bool IsMoveAvailable { get; private set; }

        public TilesBoard(byte columns, byte rows, IBoardItemsMover<ITile> mover)
            : this(null, columns, rows)
        {
            if (mover == null)
                throw new ArgumentNullException(nameof(mover), MoverNullMessage);

            _mover = mover;
        }

        private TilesBoard(IDictionary<Position, ITile> tiles, byte columns, byte rows)
            : base(AddTiles(tiles, columns, rows), columns, rows)
        {
        }

        public bool TryMove(Direction direction, out IMovingBoard<ITile> newBoard)
        {
            newBoard = null;

            IDictionary<Position, ITile> newTiles;
            if (!_mover.TryMove(this, direction, out newTiles)) return false;

            var board = new TilesBoard(newTiles, Columns, Rows);
            Task.Factory.StartNew(() => board.IsMoveAvailable = _mover.GetIsMoveAvailable(board));
            newBoard = board;

            return true;
        }

        private static IDictionary<Position, ITile> AddTiles(IDictionary<Position, ITile> tiles, int columns, int rows)
        {
            if (tiles == null)
                tiles = new Dictionary<Position, ITile>();

            var positions = GetFreePositions(tiles, columns, rows);
            if (tiles.Count < 2)
            {
                var usedIndeces = new List<int>();
                for (var i = 0; i < 2; i++)
                {
                    int index;
                    do
                    {
                        index = Random.Next(positions.Length);
                    } while (usedIndeces.Contains(index));

                    tiles.Add(positions[index], new Tile());
                    usedIndeces.Add(index);
                }
            }
            else if (tiles.Count < columns*rows)
                tiles.Add(positions[Random.Next(positions.Length)], new Tile());

            return tiles;
        }

        private static Position[] GetFreePositions(IDictionary<Position, ITile> tiles, int columns, int rows)
        {
            var isDictionaryEmpty = !tiles.Any();
            var positions = new ConcurrentBag<Position>();
            Parallel.For(0, columns, i =>
            {
                Parallel.For(0, rows, j =>
                {
                    var position = new Position(i, j);
                    if (isDictionaryEmpty || !tiles.ContainsKey(position))
                        positions.Add(position);
                });
            });

            return positions.ToArray();
        }
    }
}
