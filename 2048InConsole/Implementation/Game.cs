using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2048Unlimited.Model.Abstraction;
using _2048Unlimited.Model.Abstraction.Boards;
using _2048Unlimited.Model.Abstraction.Histories;
using _2048Unlimited.Model.Abstraction.Stats;
using _2048Unlimited.Model.Abstraction.Tiles;
using _2048Unlimited.Model.Abstraction.Timers;
using _2048Unlimited.Model.Helpers;

namespace _2048Unlimited.Model.Implementation
{
    public class Game : IGame
    {
        private const string NullHistoryMessage = "History can't be null";
        private const string NullTimerMessage = "Timer can't be null";
        private const string NullGlobalStatsMessage = "Global Statistics can't be null";

        private readonly IHistory<GameStep> _history;
        private readonly ITimer _timer;
        private readonly IGlobalStatistics _globalStatistics;

        public IReadOnlyDictionary<Position, ITile> Tiles => _history.CurrentItem.Board.Items;

        public bool IsMoveAvailable => _history.CurrentItem.Board.IsMoveAvailable;

        public bool IsUndoAvailable => _history.IsUndoAvailable;

        public bool IsRedoAvailable => _history.IsRedoAvailable;

        public IStatistics Statistics => _history.CurrentItem.Statistics;

        internal Game(IHistory<GameStep> history, ITimer timer, IGlobalStatistics globalStatistics)
        {
            if (history == null)
                throw new ArgumentNullException(nameof(history), NullHistoryMessage);

            if (timer == null)
                throw new ArgumentNullException(nameof(timer), NullTimerMessage);

            if (globalStatistics == null)
                throw new ArgumentNullException(nameof(globalStatistics), NullGlobalStatsMessage);

            _history = history;
            _timer = timer;
            _globalStatistics = globalStatistics;
        }

        public bool Move(Direction direction)
        {
            IMovingBoard<ITile> newBoard;
            if (!_history.CurrentItem.Board.TryMove(direction, out newBoard)) return false;

            double newScore = 0;
            var newElapsedTime = TimeSpan.Zero;
            var bestTileRank = 0;
            Parallel.Invoke(
                () => newScore = Statistics.Score + newBoard.Items.Values.Where(i => i.MergedWith.HasValue).Sum(i => i.Rank),
                () => newElapsedTime = _timer.ElapsedTime,
                () => bestTileRank = newBoard.Items.Values.Max(i => i.Rank));

            var newStatistics = _history.CurrentItem.Statistics.Update(newScore, newElapsedTime, bestTileRank);
            _history.AddItem(new GameStep(newBoard, newStatistics));
            _globalStatistics.Update(newScore, newElapsedTime, newStatistics.BestTileStatistics);

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
            add { _timer.Tick += value; }
            remove { _timer.Tick -= value; }
        }
    }
}