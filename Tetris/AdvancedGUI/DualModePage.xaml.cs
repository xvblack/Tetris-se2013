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

namespace Tetris.AdvancedGUI
{
    /// <summary>
    /// SingleModePage.xaml 的交互逻辑
    /// </summary>
    public partial class DualModePage : GameContainerPage
    {
        private Tetrisor t = new Tetrisor();
        private Tuple<Tetris.GameBase.TetrisGame,
            Tetris.GameBase.TetrisGame> games;

        public DualModePage() : base()
        {
            InitializeComponent();

            ColumnDefinition aCol = new ColumnDefinition();
            aCol.Width = new GridLength(1, GridUnitType.Star);
            outerGrid.ColumnDefinitions.Add(aCol);

            aCol = new ColumnDefinition();
            aCol.Width = new GridLength(1, GridUnitType.Auto);
            outerGrid.ColumnDefinitions.Add(aCol);

            aCol = new ColumnDefinition();
            aCol.Width = new GridLength(480, GridUnitType.Pixel);
            outerGrid.ColumnDefinitions.Add(aCol);

            aCol = new ColumnDefinition();
            aCol.Width = new GridLength(10, GridUnitType.Auto);
            outerGrid.ColumnDefinitions.Add(aCol);

            aCol = new ColumnDefinition();
            aCol.Width = new GridLength(10, GridUnitType.Star);
            outerGrid.ColumnDefinitions.Add(aCol);

            //outerGrid.Width = Styles.WindowSizeGenerator.dualModePageWidth;
            Border border1 = new Border();

            border1.BorderBrush = new SolidColorBrush(Colors.Gray);
            border1.BorderThickness = new Thickness(2, 2, 2, 2);

            Border border2 = new Border();

            border2.BorderBrush = new SolidColorBrush(Colors.Gray);
            border2.BorderThickness = new Thickness(2, 2, 2, 2);

            games = t.NewDuelGame();
            int[] gridSize = new int[2] 
                { games.Item1.Height, games.Item1.Width };

            outerGrid.SetValue(Canvas.LeftProperty, 10.0);

            GameGrid gameGrid1 = new GameGrid(gridSize);
            border1.Child = gameGrid1;
            outerGrid.Children.Add(border1);
            border1.SetValue(Grid.RowProperty, 1);
            border1.SetValue(Grid.ColumnProperty, 1);

            GameGrid gameGrid2 = new GameGrid(gridSize);
            //NextBlockGrid gameGrid2 = new NextBlockGrid(new int[2]{7, 7});
            border2.Child = gameGrid2;
            outerGrid.Children.Add(border2);
            border2.SetValue(Grid.RowProperty, 1);
            border2.SetValue(Grid.ColumnProperty, 3);

            games.Item1.AddDisplay(gameGrid1);
            games.Item2.AddDisplay(gameGrid2);
            //Add Display of NextBlockGrid

            games.Item1.SetController(_controller);
            //AIController _aiController1 = new AIController(games.Item1, 100);
            AIController _aiController2 = new AIController(games.Item2, 100);
            //games.Item1.SetController(_aiController1);
            games.Item2.SetController(_aiController2);

            // set left crocodile
            PicGen pic = new Cat3Gen();

            PicGenGrid pg1 = new PicGenGrid(pic, Styles.SquareGenerator.picSquareSize);
            aCanvas.Children.Add(pg1);
            pg1.SetValue(Canvas.ZIndexProperty, 0);

            Canvas.SetLeft(pg1, 480);
            Canvas.SetTop(pg1, 440);

            /*
            pic = new CatGen();

            PicGenGrid pg2 = new PicGenGrid(pic, Styles.SquareGenerator.picSquareSize / 2.5);
            aCanvas.Children.Add(pg2);
            pg1.SetValue(Canvas.ZIndexProperty, 0);

            Canvas.SetRight(pg2, 450);
            Canvas.SetBottom(pg2, 60);
            */

            // set score board
            double scoreHeight = 130;
            double scoreWidth = 200;
            double scoreRightLoc = 100;
            double scoreTopLoc = 67;
            ScoreGrid score1 = new ScoreGrid(scoreHeight, scoreWidth);
            ScoreGrid score2 = new ScoreGrid(scoreHeight, scoreWidth);

            // set next block board
            int[] nextBlockSize = new int[] { 7, 5 };
            double nextRightLoc;
            double nextTopLoc = scoreTopLoc + scoreHeight + 10;
            NextBlockGrid nextGrid1 = new NextBlockGrid(nextBlockSize);
            NextBlockGrid nextGrid2 = new NextBlockGrid(nextBlockSize);

            Border border2_1 = new Border();

            border2_1.BorderBrush = new SolidColorBrush(Colors.Black);
            border2_1.BorderThickness = new Thickness(3, 3, 3, 3);
            border2_1.CornerRadius = new CornerRadius(10);
            border2_1.Padding = new Thickness(3, 3, 3, 3);

            Border border2_2 = new Border();

            border2_2.BorderBrush = new SolidColorBrush(Colors.Black);
            border2_2.BorderThickness = new Thickness(3, 3, 3, 3);
            border2_2.CornerRadius = new CornerRadius(10);
            border2_2.Padding = new Thickness(3, 3, 3, 3);

            double[] nextSize = nextGrid1.getSize();

            border2_1.Height = nextSize[0] - (nextSize[0] / nextBlockSize[0] - 1) / 2; ;
            border2_1.Width = nextSize[1];

            border2_2.Height = nextSize[0] - (nextSize[0] / nextBlockSize[0] - 1) / 2; ;
            border2_2.Width = nextSize[1];

            border2_1.Child = nextGrid1;
            border2_2.Child = nextGrid2;
            aCanvas.Children.Add(border2_1);
            aCanvas.Children.Add(border2_2);

            nextRightLoc = 0.5 * score1.Width - 0.5 * border2.Width + scoreRightLoc;
            border2_1.SetValue(Canvas.LeftProperty, 450.0);
            border2_1.SetValue(Canvas.TopProperty, 210.0);

            border2_2.SetValue(Canvas.RightProperty,455.0);
            border2_2.SetValue(Canvas.TopProperty, 210.0);

            games.Item1.AddDisplay(nextGrid1);
            games.Item2.AddDisplay(nextGrid2);

            aCanvas.Children.Add(score1);
            score1.SetValue(Canvas.LeftProperty, 450.0);
            score1.SetValue(Canvas.TopProperty, scoreTopLoc);

            aCanvas.Children.Add(score2);
            score2.SetValue(Canvas.RightProperty, 455.0);
            score2.SetValue(Canvas.TopProperty, scoreTopLoc);

            score1.DataContext = games.Item1.ScoreSystem;
            score2.DataContext = games.Item2.ScoreSystem;

        }

        protected override void Loaded_Event(object sender, RoutedEventArgs e)
        {
            holderWin.Width = Styles.WindowSizeGenerator.dualModePageWidth;
            holderWin.Left = Styles.WindowSizeGenerator.dualModePageLocationLeft;

            outerGrid.Width = holderWin.Width;
            outerGrid.Height = holderWin.Height;

            base.Loaded_Event(sender, e);
        }

        protected override void whatHappenWhenAnimationStop(object sender, System.Timers.ElapsedEventArgs e)
        {
            games.Item1.Start();
            games.Item2.Start();
            
            base.whatHappenWhenAnimationStop(sender, e);
        }
    }
}
