using System;
using System.Collections.Generic;
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

        private static readonly object Locker = new object();
        private static readonly ConsoleDrawer Drawer = new ConsoleDrawer(Columns*ColumnSize);
        private static readonly IGlobalStatistics GlobalStats = new GlobalStatistics();

        private static IGame _game;

        private static void Main()
        {
            _game = GetNewGame();
            //_game.Tick += Game_Tick; // uncomment if you want a real elapsed time; flickering may appear

            while (true)
            {
                DrawGame(_game.Statistics.ElapsedTime);
                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        _game.Move(Direction.Left);
                        break;
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        _game.Move(Direction.Up);
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        _game.Move(Direction.Right);
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        _game.Move(Direction.Down);
                        break;
                    case ConsoleKey.Q:
                        _game.Undo();
                        break;
                    case ConsoleKey.E:
                        _game.Redo();
                        break;
                    case ConsoleKey.N:
                    case ConsoleKey.Spacebar:
                        //_game.Tick -= Game_Tick; // uncomment if you want a real elapsed time
                        _game = GetNewGame();
                        //_game.Tick += Game_Tick; // uncomment if you want a real elapsed time
                        break;
                }
            }
        }

        private static IGame GetNewGame()
        {
            return new Game(
                new History<GameStep>(
                    new List<GameStep> { 
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
                    }
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
                DrawHeader();
                DrawGameField();
                DrawTime(elapsedTime);
                Console.WriteLine();
                Drawer.FillLineWithBlocks("Local statistics");
                DrawStatistics(_game.Statistics);
                Console.WriteLine();
                Drawer.FillLineWithBlocks("Global statistics");
                DrawStatistics(GlobalStats);
            }
        }

        private static void DrawHeader()
        {
            Console.WriteLine();
            Drawer.FillLineWithBlocks("2048");
        }

        private static void DrawGameField()
        {
            Console.WriteLine();
            Drawer.FillLine("========");
            for (int i = 0; i < Rows; i++)
            {
                Drawer.FillLine(Drawer.GetStringBlock(" ", ColumnSize));
                for (int j = 0; j < Columns; j++)
                {
                    var position = new Position(j, i);
                    ITile tile;
                    Console.Write(_game.Tiles.TryGetValue(position, out tile)
                        ? Drawer.GetStringBlock(tile.Rank.ToString(), ColumnSize)
                        : Drawer.GetStringBlock(" ", ColumnSize));
                }

                Console.WriteLine();
                Drawer.FillLine(Drawer.GetStringBlock(" ", ColumnSize));
                Drawer.FillLine("========");
            }
        }

        private static void DrawTime(TimeSpan elapsedTime)
        {
            Console.WriteLine();
            Drawer.FillLineWithBlocks(elapsedTime.ToString("g"));
        }

        private static void DrawStatistics(IStatistics stats)
        {
            Drawer.FillLineWithBlocks(" ");
            Drawer.FillLineWithBlocks("Score", stats.Score.ToString());
            Drawer.FillLineWithBlocks("Moves", stats.Moves.ToString());
            Drawer.FillLineWithBlocks("Elapsed", stats.ElapsedTime.ToString());

            Drawer.FillLineWithBlocks(" ");
            Drawer.FillLineWithBlocks("Tile", "Moves", "Time");
            foreach (var pair in stats.TilesStatistics)
                Drawer.FillLineWithBlocks(pair.Key.ToString(), pair.Value.Moves.ToString(), pair.Value.Time.ToString());
        }

        private static void Game_Tick(object sender, TimerTickEventArgs e)
        {
            DrawGame(e.ElapsedTime);
        }
    }
}
