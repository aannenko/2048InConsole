namespace _2048InConsole.Boards
{
    internal interface IBoardItemsCollider<T>
    {
        bool TryCollide(T sourceItem, T destinationItem, out T resultingItem);
    }
}
