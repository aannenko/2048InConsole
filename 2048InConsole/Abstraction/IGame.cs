using System;
using System.Collections.Generic;
using _2048Unlimited.Model.Abstraction.Stats;
using _2048Unlimited.Model.Abstraction.Tiles;
using _2048Unlimited.Model.Helpers;

namespace _2048Unlimited.Model.Abstraction
{
    public interface IGame
    {
        IReadOnlyDictionary<Position, ITile> Tiles { get; }

        bool IsMoveAvailable { get; }

        bool IsUndoAvailable { get; }

        bool IsRedoAvailable { get; }

        IStatistics Statistics { get; }

        bool Move(Direction direction);

        bool Undo();

        bool Redo();

        event EventHandler<TimerTickEventArgs> Tick;
    }
}
