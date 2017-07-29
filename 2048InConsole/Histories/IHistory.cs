namespace _2048InConsole.Histories
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
