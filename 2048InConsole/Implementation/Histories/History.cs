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

        internal History(T item)
        {
            _previousItems = new Stack<T>();
            _previousItems.Push(item);
            _nextItems = new Stack<T>();
        }

        internal History(IEnumerable<T> items)
        {
            ValidateInput(items);
            _previousItems = new Stack<T>(items);
            _nextItems = new Stack<T>();
        }

        private void ValidateInput(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items), NullInitialItemsMessage);

            if (!items.Any())
                throw new ArgumentException(EmptyInitialItemsMessage, nameof(items));
        }

        public T CurrentItem => _previousItems.Peek();

        public bool IsUndoAvailable => _previousItems.Count > 1;

        public bool IsRedoAvailable => _nextItems.Any();

        public void AddItem(T item)
        {
            if (item == null) return;
            _nextItems.Clear();
            _previousItems.Push(item);
        }

        public void Undo()
        {
            if (!IsUndoAvailable) return;
            _nextItems.Push(_previousItems.Pop());
        }

        public void Redo()
        {
            if (!IsRedoAvailable) return;
            _previousItems.Push(_nextItems.Pop());
        }
    }
}