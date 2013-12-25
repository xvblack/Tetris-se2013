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
using Tetris.GameControl;
using Tetris.GameSystem;

namespace Tetris
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Square[,] _image;
        private readonly PlayerController _controller;
        private readonly AIController _aiController;
        private readonly AIController _aiController2;
        private bool dual = true; // is dual?
        private ColumnDefinition[] _oriGridCol;
        private Grid child;
        private Tetrisor t;

        public MainWindow()
        {
            InitializeComponent();
            t = new Tetrisor();
            _oriGridCol = this.Grid.ColumnDefinitions.ToArray<ColumnDefinition>();
            child = this.grid_count2;
            AchievementSystem.Load();

            if (dual)
            {
                this.Grid.ColumnDefinitions.Clear();
                for (int i = 0; i < _oriGridCol.Length; i++ )
                    this.Grid.ColumnDefinitions.Add(_oriGridCol[i]);
                if (!this.Grid.Children.Contains(child))
                    this.Grid.Children.Add(child);

                var games = t.NewDuelGame();
                GameGrid gameGrid1 = new GameGrid(games.Item1.Height, games.Item1.Width);
                // test code
                Console.WriteLine(games.Item1.Height);
                Console.WriteLine(gameGrid1.Height);
                // end
                this.Grid.Children.Add(gameGrid1);
                Grid.SetRow(gameGrid1, 1);
                Grid.SetColumn(gameGrid1, 1);
                GameGrid gameGrid2 = new GameGrid(games.Item2.Height, games.Item2.Width);
                this.Grid.Children.Add(gameGrid2);
                Grid.SetRow(gameGrid2, 1);
                Grid.SetColumn(gameGrid2, 5);

                this.Height = 50 + gameGrid1.Height + 30 + 10 + 20;
                this.Width = 200 + gameGrid1.Width * 2 + 10 * 2 + 150 + 5;

                grid_count.DataContext = games.Item1.ScoreSystem;
                grid_count2.DataContext = games.Item2.ScoreSystem;

                games.Item1.AddDisplay(gameGrid1);
                games.Item2.AddDisplay(gameGrid2);

                games.Item2.AddDisplay(new ConsoleDisplay());

                // speed: 0~15 (速度，0为完全不按加速)
                // error: 0~100 （每次犯错的概率）
                // errorCount: >=0 （每几次才可能犯错一次，0为不犯错，1为每次都可能犯错）
                // 根据这三个参数可以调节AI的难度，最后选三个作为三种难度就行 by 郭亨凯
                //_aiController2 = new AIController(games.Item1, 15);
                //games.Item1.SetController(_aiController2);
                _controller = new PlayerController();
                games.Item1.SetController(_controller);
                _aiController = new AIController(games.Item2, 15);
                games.Item2.SetController(_aiController);

                games.Item1.Start();
                games.Item2.Start();
            }
            else
            {
                this.Grid.ColumnDefinitions.Clear();
                for (int i = 0; i < _oriGridCol.Length; i++)
                    this.Grid.ColumnDefinitions.Add(_oriGridCol[i]);
                if (this.Grid.Children.Contains(child))
                    this.Grid.Children.Remove(child);
                this.Grid.ColumnDefinitions.RemoveRange(4, 3);

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

        protected override void OnClosed(EventArgs e)
        {
            AchievementSystem.Save();
            t.StopEngine();
            base.OnClosed(e);
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
            _controller.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            _controller.OnKeyUp(e);
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

            public void InverseControl()
            {
            }
        }
    }
}
