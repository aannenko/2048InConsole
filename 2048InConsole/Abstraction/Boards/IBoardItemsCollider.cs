namespace _2048Unlimited.Model.Abstraction.Boards
{
    internal interface IBoardItemsCollider<T>
    {
        bool TryCollide(T sourceItem, T destinationItem, out T resultingItem);
    }
}
