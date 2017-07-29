using _2048InConsole.Boards;
using _2048InConsole.Games;
using _2048InConsole.Helpers;
using _2048InConsole.Histories;
using _2048InConsole.Saves;
using _2048InConsole.Stats;
using _2048InConsole.Tiles;
using _2048InConsole.Timers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;

namespace _2048InConsole
{
    internal class Program
    {
        private static readonly string SaveFilePath = Path.Combine(
            Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath),
            "2048ultimate.sav");

        private static readonly Type[] KnownTypes =
        {
            typeof(Game),
            typeof(History<GameStep>),
            typeof(GameStep),
            typeof(TilesBoard),
            typeof(Board<Tile>),
            typeof(Tile),
            typeof(TilesMover),
            typeof(TilesCollider),
            typeof(Statistics),
            typeof(GlobalStatistics),
            typeof(LocalStatistics),
            typeof(TileStatistics),
            typeof(ReadOnlyDictionary<int, ITileStatistics>),
            typeof(ReadOnlyDictionary<Position, ITile>),
            typeof(ElapsingTimer)
        };

        private static readonly GameSettings Settings = new GameSettings();
        private static readonly ISavesManager<IGame> GameSaver = new SavesManager<IGame>();

        private static readonly object Locker = new object();
        private static readonly ConsoleDrawer Drawer = new ConsoleDrawer(Settings.Columns*Settings.ColumnSize);

        private static IGame _game;

        private static void Main()
        {
            _game = GetGame();
            if (Settings.DynamicElapsedTime) _game.Tick += Game_Tick;

            bool isMoveMade = true;
            while (true)
            {
                if (isMoveMade) DrawGame(_game.Statistics.ElapsedTime);

                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        isMoveMade = _game.Move(Direction.Left);
                        break;
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        isMoveMade = _game.Move(Direction.Up);
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        isMoveMade = _game.Move(Direction.Right);
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        isMoveMade = _game.Move(Direction.Down);
                        break;
                    case ConsoleKey.Q:
                        isMoveMade = _game.Undo();
                        break;
                    case ConsoleKey.E:
                        isMoveMade = _game.Redo();
                        break;
                    case ConsoleKey.N:
                    case ConsoleKey.Spacebar:
                        if (Settings.DynamicElapsedTime) _game.Tick -= Game_Tick;
                        _game = GetGame();
                        if (Settings.DynamicElapsedTime) _game.Tick += Game_Tick;
                        isMoveMade = true;
                        break;
                    case ConsoleKey.Z:
                        SaveGame();
                        return;
                }
            }
        }

        private static IGame GetGame()
        {
            GameSaver.TryLoad(SaveFilePath, KnownTypes, out IGame game);

            return game ?? new Game(
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
                new GlobalStatistics()
            );
        }

        private static void SaveGame()
        {
            GameSaver.TrySave(SaveFilePath, _game, KnownTypes);
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
                DrawStatistics(_game.GlobalStatistics);
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
