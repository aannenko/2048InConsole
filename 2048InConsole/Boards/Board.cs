using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using _2048InConsole.Helpers;

namespace _2048InConsole.Boards
{
    [DataContract]
    internal abstract class Board<T> : IBoard<T>
    {
        private const string NullItemsDictionaryMessage = "The board's Items collection can't be null.";
        private const string NotEnoughColumnsOrRowsMessage = "The board should have at least 2 columns and 2 rows";
        private const string ItemsBeyondBoardMessage = "The board can't contain items which position is beyond the edge of the board.";
        private const string NullItemMessage = "The board's Items collection can't contain null items.";

        [DataMember(IsRequired = true)]
        public byte Columns { get; private set; }

        [DataMember(IsRequired = true)]
        public byte Rows { get; private set; }

        [DataMember(IsRequired = true)]
        public IReadOnlyDictionary<Position, T> Items { get; private set; }

        internal Board(IDictionary<Position, T> items, byte columns, byte rows)
        {
            ValidateColumnsAndRows(columns, rows);
            Columns = columns;
            Rows = rows;

            ValidateItems(items);
            Items = new ReadOnlyDictionary<Position, T>(items);
        }

        public bool IsPositionWithinBoard(Position position)
        {
            return position.Column > -1
                && position.Column < Columns
                && position.Row > -1
                && position.Row < Rows;
        }

        private void ValidateColumnsAndRows(byte columns, byte rows)
        {
            if (columns < 2 || rows < 2)
                throw new ArgumentOutOfRangeException(columns < 2 ? nameof(columns) : nameof(rows), NotEnoughColumnsOrRowsMessage);
        }

        private void ValidateItems(IDictionary<Position, T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items), NullItemsDictionaryMessage);

            var isOutOfBoardItemPresent = false;
            var isNullItemPresent = false;
            Parallel.ForEach(items, (item, loopState) =>
            {
                isOutOfBoardItemPresent = !IsPositionWithinBoard(item.Key);
                isNullItemPresent = item.Value == null;
                if (isOutOfBoardItemPresent || isNullItemPresent) loopState.Stop();
            });

            var exceptions = new List<Exception>();
            if (isOutOfBoardItemPresent)
                exceptions.Add(new ArgumentOutOfRangeException(nameof(items), ItemsBeyondBoardMessage));

            if (isNullItemPresent)
                exceptions.Add(new ArgumentException(NullItemMessage, nameof(items)));

            if (exceptions.Any())
                throw new AggregateException(exceptions);
        }
    }
}
