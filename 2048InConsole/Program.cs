using System;
using System.Text;
using _2048Unlimited.Model.Abstraction;
using _2048Unlimited.Model.Abstraction.Stats;
using _2048Unlimited.Model.Abstraction.Tiles;
using _2048Unlimited.Model.Helpers;
using _2048Unlimited.Model.Implementation;
using _2048Unlimited.Model.Implementation.Boards;
using _2048Unlimited.Model.Implementation.Histories;
using _2048Unlimited.Model.Implementation.Stats;
using _2048Unlimited.Model.Implementation.Timers;

namespace _2048InConsole
{
    internal class Program
    {
        private const byte ColumnSize = 8;
        private const byte Columns = 4;
        private const byte Rows = 4;

        private static readonly int LineSize = Columns*ColumnSize;
        private static readonly object Locker = new object();
        private static readonly IStatisticsInternal GlobalStats = new GlobalStatistics();

        private static IGame _game;

        private static void Main()
        {
            _game = GetNewGame();
            _game.Tick += Game_Tick;

            while (true)
            {
                DrawGame(_game.Statistics.ElapsedTime);
                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        _game.Move(Direction.Left);
                        break;
                    case ConsoleKey.UpArrow:
                        _game.Move(Direction.Up);
                        break;
                    case ConsoleKey.RightArrow:
                        _game.Move(Direction.Right);
                        break;
                    case ConsoleKey.DownArrow:
                        _game.Move(Direction.Down);
                        break;
                    case ConsoleKey.Q:
                        _game.Undo();
                        break;
                    case ConsoleKey.E:
                        _game.Redo();
                        break;
                    case ConsoleKey.N:
                        _game.Tick -= Game_Tick;
                        _game = GetNewGame();
                        _game.Tick += Game_Tick;
                        break;
                }
            }
        }

        private static void Game_Tick(object sender, TimerTickEventArgs e)
        {
            DrawGame(e.ElapsedTime);
        }

        private static IGame GetNewGame()
        {
            return new Game(
                new History<GameStep>(
                    new GameStep(
                        new TilesBoard(
                            Columns,
                            Rows,
                            new TilesMover(
                                new TilesCollider()
                            )
                        ),
                        new LocalStatistics()
                    )
                ),
                new ElapsingTimer(),
                GlobalStats
            );
        }

        private static void DrawGame(TimeSpan elapsedTime)
        {
            lock (Locker)
            {
                Console.Clear();
                Console.WriteLine();
                FillLineWithBlocks("2048");
                FillLineWithBlocks(" ");
                FillLineWithBlocks("Score", "Best");
                FillLineWithBlocks(_game.Statistics.Score.ToString(), GlobalStats.Score.ToString());
                Console.WriteLine();
                FillLineWith("========");
                for (int i = 0; i < Rows; i++)
                {
                    FillLineWith("|      |");
                    for (int j = 0; j < Columns; j++)
                    {
                        var position = new Position(j, i);
                        ITile tile;
                        Console.Write(_game.Tiles.TryGetValue(position, out tile) ? GetStringBlock(tile.Rank.ToString(), ColumnSize) : "|      |");
                    }

                    Console.WriteLine();
                    FillLineWith("|      |");
                    FillLineWith("========");
                }

                Console.WriteLine();
                FillLineWithBlocks($"{_game.Statistics.Moves} moves", elapsedTime.ToString("g"));
                Console.WriteLine();
                FillLineWithBlocks("Local statistics");
                DrawStatistics(_game.Statistics);
                Console.WriteLine();
                FillLineWithBlocks("Global statistics");
                DrawStatistics(GlobalStats);
            }
        }

        private static void DrawStatistics(IStatistics stats)
        {
            FillLineWithBlocks(" ");
            FillLineWithBlocks("Score", stats.Score.ToString());
            FillLineWithBlocks("Moves", stats.Moves.ToString());
            FillLineWithBlocks("Elapsed", stats.ElapsedTime.ToString());

            FillLineWithBlocks(" ");
            FillLineWithBlocks("Tile", "Moves");
            foreach (var pair in stats.MovesToTileDictionary)
                FillLineWithBlocks(pair.Key.ToString(), pair.Value.ToString());

            FillLineWithBlocks(" ");
            FillLineWithBlocks("Tile", "Time");
            foreach (var pair in stats.TimeToTileDictionary)
                FillLineWithBlocks(pair.Key.ToString(), pair.Value.ToString());
        }

        private static void FillLineWith(string content)
        {
            var builder = new StringBuilder(content);
            while (builder.Length < LineSize)
                builder.Append(content);

            if (builder.Length > LineSize)
                builder.Remove(LineSize, builder.Length - LineSize);

            Console.Write(builder);
            Console.WriteLine();
        }

        private static void FillLineWithBlocks(params string[] content)
        {
            var blockLenght = LineSize/content.Length;
            var remainder = LineSize%content.Length;
            var builder = new StringBuilder();
            foreach (var block in content)
            {
                builder.Append(GetStringBlock(block, blockLenght));
                if (remainder < 1) continue;
                builder.Append(" ");
                remainder--;
            }

            Console.Write(builder);
            Console.WriteLine();
        }

        private static string GetStringBlock(string content, int blockLenght)
        {
            if (content.Length > blockLenght - 2)
                content = content.Remove(content.Length - 2);

            var builder = new StringBuilder("|");
            var full = blockLenght - 2 - content.Length;
            var secondPart = full / 2;
            var firstPart = full - secondPart;

            builder.Append(new string(' ', firstPart));
            builder.Append(content);
            builder.Append(new string(' ', secondPart));
            builder.Append("|");

            return builder.ToString();
        }
    }
}
