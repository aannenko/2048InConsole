namespace _2048Unlimited.Model.Abstraction.Histories
{
    internal interface IHistory<T>
    {
        T CurrentItem { get; }

        bool IsUndoAvailable { get; }

        bool IsRedoAvailable { get; }

        void AddItem(T item);

        bool Undo();

        bool Redo();
    }
}
