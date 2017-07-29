using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using _2048InConsole.Boards;
using _2048InConsole.Helpers;
using _2048InConsole.Histories;
using _2048InConsole.Stats;
using _2048InConsole.Tiles;
using _2048InConsole.Timers;

namespace _2048InConsole.Games
{
    [DataContract]
    public class Game : IGame
    {
        private const string NullHistoryMessage = "History can't be null";
        private const string NullTimerMessage = "Timer can't be null";
        private const string NullGlobalStatsMessage = "Global Statistics can't be null";

        [DataMember(IsRequired = true)]
        private readonly IHistory<GameStep> _history;

        [DataMember(IsRequired = true)]
        private readonly ITimer _timer;

        [DataMember(IsRequired = true)]
        public IGlobalStatistics GlobalStatistics { get; private set; }

        public IReadOnlyDictionary<Position, ITile> Tiles => _history.CurrentItem.Board.Items;

        public bool IsMoveAvailable => _history.CurrentItem.Board.IsMoveAvailable;

        public bool IsUndoAvailable => _history.IsUndoAvailable;

        public bool IsRedoAvailable => _history.IsRedoAvailable;

        public IStatistics Statistics => _history.CurrentItem.Statistics;

        internal Game(IHistory<GameStep> history, ITimer timer, IGlobalStatistics globalStatistics)
        {
            _history = history ?? throw new ArgumentNullException(nameof(history), NullHistoryMessage);
            _timer = timer ?? throw new ArgumentNullException(nameof(timer), NullTimerMessage);
            GlobalStatistics = globalStatistics ?? throw new ArgumentNullException(nameof(globalStatistics), NullGlobalStatsMessage);
        }

        public bool Move(Direction direction)
        {
            if (!_timer.IsTicking) _timer.Start();

            if (!_history.CurrentItem.Board.TryMove(direction, out IMovingBoard<ITile> newBoard)) return false;

            double newScore = 0;
            var newElapsedTime = TimeSpan.Zero;
            var bestTileRank = 0;
            Parallel.Invoke(
                () => newScore = Statistics.Score + newBoard.Items.Values.Where(i => i.MergedWith.HasValue).Sum(i => i.Rank),
                () => newElapsedTime = _timer.ElapsedTime,
                () => bestTileRank = newBoard.Items.Values.Max(i => i.Rank));

            var newStatistics = _history.CurrentItem.Statistics.Update(newScore, newElapsedTime, bestTileRank);
            _history.AddItem(new GameStep(newBoard, newStatistics));
            GlobalStatistics.Update(newScore, newElapsedTime, newStatistics.BestTileStatistics);

            return true;
        }

        public bool Undo()
        {
            if (!_history.Undo()) return false;
            _timer.SetElapsedTime(_history.CurrentItem.Statistics.ElapsedTime);
            return true;
        }

        public bool Redo()
        {
            if (!_history.Redo()) return false;
            _timer.SetElapsedTime(_history.CurrentItem.Statistics.ElapsedTime);
            return true;
        }

        public event EventHandler<TimerTickEventArgs> Tick
        {
            add => _timer.Tick += value;
            remove => _timer.Tick -= value;
        }
    }
}