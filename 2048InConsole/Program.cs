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
        private static readonly GameSettings Settings = new GameSettings();

        private static readonly object Locker = new object();
        private static readonly ConsoleDrawer Drawer = new ConsoleDrawer(Settings.Columns*Settings.ColumnSize);
        private static readonly IGlobalStatistics GlobalStats = new GlobalStatistics();

        private static IGame _game;

        private static void Main()
        {
            _game = GetNewGame();
            if (Settings.DynamicElapsedTime) _game.Tick += Game_Tick;

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
                        if (Settings.DynamicElapsedTime) _game.Tick -= Game_Tick;
                        _game = GetNewGame();
                        if (Settings.DynamicElapsedTime) _game.Tick += Game_Tick;
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
                                Settings.Columns,
                                Settings.Rows,
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
            for (int i = 0; i < Settings.Rows; i++)
            {
                Drawer.FillLine(Drawer.GetStringBlock(" ", Settings.ColumnSize));
                for (int j = 0; j < Settings.Columns; j++)
                {
                    var position = new Position(j, i);
                    ITile tile;
                    Console.Write(_game.Tiles.TryGetValue(position, out tile)
                        ? Drawer.GetStringBlock(tile.Rank.ToString(), Settings.ColumnSize)
                        : Drawer.GetStringBlock(" ", Settings.ColumnSize));
                }

                Console.WriteLine();
                Drawer.FillLine(Drawer.GetStringBlock(" ", Settings.ColumnSize));
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
