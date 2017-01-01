using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using _2048Unlimited.Model.Abstraction.Histories;

namespace _2048Unlimited.Model.Implementation.Histories
{
    [DataContract]
    internal class History<T> : IHistory<T>
    {
        private const string NullInitialItemsMessage = "Initial items collection can't be null";
        private const string EmptyInitialItemsMessage = "Initial items collection can't be empty";

        [DataMember(IsRequired = true)]
        private readonly Stack<T> _previousItems;

        [DataMember(IsRequired = true)]
        private readonly Stack<T> _nextItems;

        public T CurrentItem => _previousItems.Peek();

        public bool IsUndoAvailable => _previousItems.Count > 1;

        public bool IsRedoAvailable => _nextItems.Any();

        internal History(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items), NullInitialItemsMessage);

            if (!items.Any())
                throw new ArgumentException(EmptyInitialItemsMessage, nameof(items));

            _previousItems = new Stack<T>(items);
            _nextItems = new Stack<T>();
        }

        public void AddItem(T item)
        {
            if (item == null) return;
            _nextItems.Clear();
            _previousItems.Push(item);
        }

        public bool Undo()
        {
            if (!IsUndoAvailable) return false;
            _nextItems.Push(_previousItems.Pop());
            return true;
        }

        public bool Redo()
        {
            if (!IsRedoAvailable) return false;
            _previousItems.Push(_nextItems.Pop());
            return true;
        }
    }
}