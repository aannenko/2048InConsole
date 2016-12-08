using System;
using System.Runtime.Serialization;
using _2048Unlimited.Model.Abstraction.Boards;
using _2048Unlimited.Model.Abstraction.Stats;
using _2048Unlimited.Model.Abstraction.Tiles;

namespace _2048Unlimited.Model.Implementation
{
    [DataContract]
    internal class GameStep
    {
        private const string NullBoardMessage = "Game Step cannot contain a null Board";
        private const string NullStatsMessage = "Game Step cannot contain a null Statistics";

        [DataMember(IsRequired = true)]
        public IMovingBoard<ITile> Board { get; }

        [DataMember(IsRequired = true)]
        public IStatisticsInternal Statistics { get; }

        internal GameStep(IMovingBoard<ITile> board, IStatisticsInternal statistics)
        {
            ValidateParameters(board, statistics);
            Board = board;
            Statistics = statistics;
        }

        private void ValidateParameters(IMovingBoard<ITile> board, IStatisticsInternal statistics)
        {
            if (board == null)
                throw new ArgumentNullException(nameof(board), NullBoardMessage);

            if (statistics == null)
                throw new ArgumentNullException(nameof(statistics), NullStatsMessage);
        }
    }
}