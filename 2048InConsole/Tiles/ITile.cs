using System;

namespace _2048InConsole.Tiles
{
    public interface ITile
    {
        int Rank { get; }
        Guid Id { get; }
        Guid? MergedWith { get; }
    }
}