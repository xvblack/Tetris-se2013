using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tetris.GameBase;
using Tetris.GameGUI;

namespace Tetris
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, IDisplay
    {
        private Square[,] _image;
        private readonly Controller _controller;
        private readonly AIController _aiController;
        private readonly AIController _aiController2;
        private bool dual = true; // is dual?
        private ColumnDefinition[] _oriGridCol;
        private Grid child;

        public MainWindow()
        {
            InitializeComponent();
            var t = new Tetrisor();
            _oriGridCol = this.Grid.ColumnDefinitions.ToArray<ColumnDefinition>();
            Trace.WriteLine(_oriGridCol.Length);
            child = this.grid_count2;

            if (dual)
            {
                var games = t.NewDuelGame();
                this.Grid.ColumnDefinitions.Clear();
                for (int i = 0; i < _oriGridCol.Length; i++ )
                    this.Grid.ColumnDefinitions.Add(_oriGridCol[i]);
                if (!this.Grid.Children.Contains(child))
                    this.Grid.Children.Add(child);
                Trace.WriteLine(this.Grid.ColumnDefinitions.Count);
                GameGrid gameGrid1 = new GameGrid(games.Item1.Height, games.Item1.Width);
                this.Grid.Children.Add(gameGrid1);
                Grid.SetRow(gameGrid1, 1);
                Grid.SetColumn(gameGrid1, 1);
                GameGrid gameGrid2 = new GameGrid(games.Item2.Height, games.Item2.Width);
                this.Grid.Children.Add(gameGrid2);
                Grid.SetRow(gameGrid2, 1);
                Grid.SetColumn(gameGrid2, 5);

                games.DuelGameEndEvent += delegate(DuelGame game, int winner)
                {
                    if (winner == 0)
                    {
                        WinnerBlock.Dispatcher.Invoke(new Action(delegate()
                        {
                            WinnerBlock.Text = "Left";
                        }));
                    }
                    else
                    {
                        WinnerBlock.Dispatcher.Invoke(new Action(delegate()
                        {
                            WinnerBlock.Text = "Right";
                        }));
                    }
                };

                this.Height = 50 + gameGrid1.Height + 30 + 10 + 20;
                this.Width = 200 + gameGrid1.Width * 2 + 10 * 2 + 150 + 5;

                grid_count.DataContext = games.Item1.ScoreSystem;
                grid_count2.DataContext = games.Item2.ScoreSystem;

                games.Item1.AddDisplay(gameGrid1);
                games.Item2.AddDisplay(gameGrid2);

                _aiController2 = new AIController(games.Item1, 100);
                games.Item1.SetController(_aiController2);
                //_controller = new Controller();
                //games.Item1.SetController(_controller);
                _aiController = new AIController(games.Item2, 100);
                games.Item2.SetController(_aiController);

                games.Item1.Start();
                games.Item2.Start();
            }
            else
            {
                this.Grid.ColumnDefinitions.RemoveRange(4, 3);
                this.Grid.Children.Remove(child);
                var game = t.NewGame();
                //game.AddDisplay(this);

                GameGrid gameGrid = new GameGrid(game.Height, game.Width);
                this.Grid.Children.Add(gameGrid);               
                Grid.SetRow(gameGrid, 1);
                Grid.SetColumn(gameGrid, 1);

                this.Height = 50 + gameGrid.Height + 30 + 10 + 20;
                this.Width = 200 + gameGrid.Width + 10 * 2;

                grid_count.DataContext = game.ScoreSystem;
                game.AddDisplay(gameGrid);

                _aiController = new AIController(game, 100);
                game.SetController(_aiController);

                game.Start();
            }
        }
        
        private readonly Key[] Keys = new Key[]
        {
            Key.Left,
            Key.Right,
            Key.Up,
            Key.Down
        };

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (Keys.Contains(e.Key))
            {
                _controller.KeyState[e.Key] = true;
                if (e.Key == Key.Left) _controller.KeyState[Key.Right] = false;
                if (e.Key == Key.Right) _controller.KeyState[Key.Left] = false;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (Keys.Contains(e.Key))
            {
               // _controller.KeyState[e.Key] = false;
            }
        }

        public void OnDrawing(TetrisGame game, TetrisGame.DrawEventArgs e)
        {
            _image = game.Image;
            Console.Clear();
            Console.WriteLine(game.ScoreSystem.Score);
            Console.WriteLine("============");
            for (int i = 0; i < 15; i++)
            {
                Console.Write("|");
                for (int j = 0; j < 10; j++)
                    Console.Out.Write(_image[i, j] == null ? " " : (_image[i,j].Color>=5? "I" : "#"));
                Console.Out.WriteLine("|");
            }
            Console.WriteLine("============");
        }
        
        static readonly Dictionary<TetrisGame.GameAction,Key> Ht=new Dictionary<TetrisGame.GameAction, Key>()
        {
            {TetrisGame.GameAction.Left,Key.Left},
            {TetrisGame.GameAction.Right,Key.Right},
            {TetrisGame.GameAction.Down,Key.Down},
            {TetrisGame.GameAction.Rotate,Key.Up}
        };

        class Controller : IController
        {
            private readonly Dictionary<Key,bool> _keyState=new Dictionary<Key, bool>()
            {
                {Key.Left,false},
                {Key.Right,false},
                {Key.Up,false},
                {Key.Down,false}
            };

            public Dictionary<Key, bool> KeyState
            {
                get { return _keyState; }
            }
            public bool Act(TetrisGame.GameAction action)
            {
                var result= KeyState[Ht[action]];
                KeyState[Ht[action]] = false;
                return result;
            }
        }
    }
}
