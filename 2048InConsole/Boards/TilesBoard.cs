using _2048InConsole.Helpers;
using _2048InConsole.Tiles;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace _2048InConsole.Boards
{
    [DataContract]
    internal class TilesBoard : Board<ITile>, IMovingBoard<ITile>
    {
        private const string MoverNullMessage = "Board Items Mover can't be null";

        private static readonly Random Random = new Random();

        [DataMember(IsRequired = true)]
        private IBoardItemsMover<ITile> _mover;

        [DataMember(IsRequired = true)]
        public bool IsMoveAvailable { get; private set; }

        public TilesBoard(byte columns, byte rows, IBoardItemsMover<ITile> mover)
            : this(null, columns, rows, mover)
        {
        }

        private TilesBoard(IDictionary<Position, ITile> tiles, byte columns, byte rows, IBoardItemsMover<ITile> mover)
            : base(AddTiles(tiles, columns, rows), columns, rows)
        {
            _mover = mover ?? throw new ArgumentNullException(nameof(mover), MoverNullMessage);
        }

        public bool TryMove(Direction direction, out IMovingBoard<ITile> newBoard)
        {
            newBoard = null;

            IDictionary<Position, ITile> newTiles;
            if (!_mover.TryMove(this, direction, out newTiles)) return false;

            var board = new TilesBoard(newTiles, Columns, Rows, _mover);
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
