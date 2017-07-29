using System;
using System.Runtime.Serialization;
using _2048InConsole.Boards;
using _2048InConsole.Stats;
using _2048InConsole.Tiles;

namespace _2048InConsole.Games
{
    [DataContract]
    internal class GameStep
    {
        private const string NullBoardMessage = "Game Step cannot contain a null Board";
        private const string NullStatsMessage = "Game Step cannot contain a null Statistics";

        [DataMember(IsRequired = true)]
        public IMovingBoard<ITile> Board { get; private set; }

        [DataMember(IsRequired = true)]
        public ILocalStatistics Statistics { get; private set; }

        internal GameStep(IMovingBoard<ITile> board, ILocalStatistics statistics)
        {
            Board = board ?? throw new ArgumentNullException(nameof(board), NullBoardMessage);
            Statistics = statistics ?? throw new ArgumentNullException(nameof(statistics), NullStatsMessage);
        }
    }
}