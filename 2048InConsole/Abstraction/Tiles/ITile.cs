using System;

namespace _2048Unlimited.Model.Abstraction.Tiles
{
    public interface ITile
    {
        int Rank { get; }
        Guid Id { get; }
        Guid? MergedWith { get; }
    }
}