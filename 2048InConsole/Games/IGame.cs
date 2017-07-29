using System;
using System.Collections.Generic;
using _2048InConsole.Helpers;
using _2048InConsole.Stats;
using _2048InConsole.Tiles;

namespace _2048InConsole.Games
{
    public interface IGame
    {
        IReadOnlyDictionary<Position, ITile> Tiles { get; }

        bool IsMoveAvailable { get; }

        bool IsUndoAvailable { get; }

        bool IsRedoAvailable { get; }

        IStatistics Statistics { get; }

        IGlobalStatistics GlobalStatistics { get; }

        bool Move(Direction direction);

        bool Undo();

        bool Redo();

        event EventHandler<TimerTickEventArgs> Tick;
    }
}
