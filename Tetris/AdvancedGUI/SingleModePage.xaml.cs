using System;
using System.Collections.Generic;
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
using System.Windows.Media.Animation;
using Tetris.AdvancedGUI.Pic;
using Tetris.GameControl;

namespace Tetris.AdvancedGUI
{
    /// <summary>
    /// SingleModePage.xaml 的交互逻辑
    /// </summary>
    public partial class SingleModePage : GameContainerPage
    {
        private Tetrisor t = new Tetrisor();
        private GameGrid gameGrid;
        private Tetris.GameBase.TetrisGame game;

        private ScoreGrid score;

        public SingleModePage():base()
        {
            InitializeComponent();

            int[] nextBlockSize = new int[] { 7, 5 };

            ColumnDefinition aCol = new ColumnDefinition();
            aCol.Width = new GridLength(50, GridUnitType.Star);
            outerGrid.ColumnDefinitions.Add(aCol);

            aCol = new ColumnDefinition();
            aCol.Width = new GridLength(50, GridUnitType.Auto);
            outerGrid.ColumnDefinitions.Add(aCol);

            aCol = new ColumnDefinition();
            aCol.Width = new GridLength(50, GridUnitType.Star);
            outerGrid.ColumnDefinitions.Add(aCol);
    
            Border border = new Border();

            border.BorderBrush = new SolidColorBrush(Colors.Gray);
            border.BorderThickness = new Thickness(2, 2, 2, 2);

            game = t.NewGame();

            int[] gridSize = new int[2] { game.Height, game.Width };

            gameGrid = new GameGrid(gridSize);

            game.AddDisplay(gameGrid);
            //AIController _aiController = new AIController(game);
            //game.SetController(_aiController);
            game.SetController(_controller[0]);
            

            outerGrid.SetValue(Canvas.LeftProperty, Styles.WindowSizeGenerator.singleModePageLocationLeft);

            border.Child = gameGrid;
            outerGrid.Children.Add(border);
            border.SetValue(Grid.RowProperty, 1);
            border.SetValue(Grid.ColumnProperty, 1);


            // set left crocodile
            PicGen pic = new CrocodileGen();

            PicGenGrid pg1 = new PicGenGrid(pic, Styles.SquareGenerator.picSquareSize);
            aCanvas.Children.Add(pg1);
            pg1.SetValue(Canvas.ZIndexProperty, 0);

            Canvas.SetLeft(pg1, 150);
            Canvas.SetBottom(pg1, 60);

            // set right crocodile
            pic = new CrocodileGen();
            PicGenGrid pg2 = new PicGenGrid(pic, Styles.SquareGenerator.picSquareSize);
            aCanvas.Children.Add(pg2);
            pg2.SetValue(Canvas.ZIndexProperty, 0);

            Canvas.SetLeft(pg2, 530);
            Canvas.SetBottom(pg2, 60);

            // set little sun
            pic = new SunGen();
            PicGenGrid pg3 = new PicGenGrid(pic, Styles.SquareGenerator.picSquareSize / 1.2);
            aCanvas.Children.Add(pg3);
            pg3.SetValue(Canvas.ZIndexProperty, 0);

            Canvas.SetLeft(pg3, -50);
            Canvas.SetTop(pg3, -50);

            // set score board
            double scoreHeight = 150;
            double scoreWidth = 200;
            double scoreRightLoc = 100;
            double scoreTopLoc = 50;
            score = new ScoreGrid(scoreHeight, scoreWidth);

            // set next block board
            double nextRightLoc;
            double nextTopLoc = scoreTopLoc + scoreHeight + 10;
            NextBlockGrid nextGrid = new NextBlockGrid(nextBlockSize);

            Border border2 = new Border();

            border2.BorderBrush = new SolidColorBrush(Colors.Black);
            border2.BorderThickness = new Thickness(3, 3, 3, 3);
            border2.CornerRadius = new CornerRadius(10);
            border2.Padding = new Thickness(3, 3, 3, 3);

            double[] nextSize = nextGrid.getSize();

            border2.Height = nextSize[0] - (nextSize[0] / nextBlockSize[0] - 1) / 2;;
            border2.Width = nextSize[1];

            border2.Child = nextGrid;
            aCanvas.Children.Add(border2);

            nextRightLoc = 0.5 * score.Width - 0.5 * border2.Width + scoreRightLoc;
            border2.SetValue(Canvas.RightProperty, nextRightLoc);
            border2.SetValue(Canvas.TopProperty, 220.0);

            game.AddDisplay(nextGrid);

            aCanvas.Children.Add(score);
            score.SetValue(Canvas.RightProperty, scoreRightLoc);
            score.SetValue(Canvas.TopProperty, scoreTopLoc);

            score.DataContext = game.ScoreSystem;


            

        }

        protected override void Loaded_Event(object sender, RoutedEventArgs e)
        {
            holderWin.Width = Styles.WindowSizeGenerator.singleModePageWidth;
            holderWin.Left = Styles.WindowSizeGenerator.singleModePageLocationLeft;

            outerGrid.Width = holderWin.Width;
            outerGrid.Height = holderWin.Height;

            base.Loaded_Event(sender, e);

        }

        protected override void whatHappenWhenAnimationStop(object sender, System.Timers.ElapsedEventArgs e)
        {
            game.Start();
            base.whatHappenWhenAnimationStop(sender, e);
        }

        protected override void keyPressed(object sender, KeyEventArgs e)
        {
            _controller[0].OnKeyDown(e);
            if (e.Key == Key.Escape)
            {
                game.Pause();
                EscapeDialog win = new EscapeDialog(game);
                win.holderWindow = this.holderWin;
                win.ShowDialog();
            }
        }
    }
}
