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

            // game grid definition
            outerGrid.Width = Styles.WindowSizeGenerator.screenWidth;
            outerGrid.Height = Styles.WindowSizeGenerator.screenHeight;

            ColumnDefinition aCol = new ColumnDefinition();
            aCol.Width = new GridLength(Styles.WindowSizeGenerator.dualGameModuleRight, 
                GridUnitType.Pixel);
            outerGrid.ColumnDefinitions.Add(aCol);

            aCol = new ColumnDefinition();
            aCol.Width = new GridLength(1, GridUnitType.Auto);
            outerGrid.ColumnDefinitions.Add(aCol);

            aCol = new ColumnDefinition();
            aCol.Width = new GridLength(Styles.WindowSizeGenerator.dualAdditionModuleWidth,
                GridUnitType.Star);
            outerGrid.ColumnDefinitions.Add(aCol);

            aCol = new ColumnDefinition();
            aCol.Width = new GridLength(1, GridUnitType.Auto);
            outerGrid.ColumnDefinitions.Add(aCol);

            aCol = new ColumnDefinition();
            aCol.Width = new GridLength(Styles.WindowSizeGenerator.dualGameModuleRight, 
                GridUnitType.Pixel);
            outerGrid.ColumnDefinitions.Add(aCol);

            Border border1 = new Border();

            border1.BorderBrush = new SolidColorBrush(Colors.Gray);
            border1.BorderThickness = new Thickness(2, 2, 2, 2);

            Border border2 = new Border();

            border2.BorderBrush = new SolidColorBrush(Colors.Gray);
            border2.BorderThickness = new Thickness(2, 2, 2, 2);

            games = t.NewDuelGame();
            int[] gridSize = new int[2] 
                { games.Item1.Height, games.Item1.Width };

          
            
            GameGrid gameGrid1 = new GameGrid(gridSize);
            border1.Child = gameGrid1;
            outerGrid.Children.Add(border1);
            border1.SetValue(Grid.RowProperty, 1);
            border1.SetValue(Grid.ColumnProperty, 1);

            GameGrid gameGrid2 = new GameGrid(gridSize);

            border2.Child = gameGrid2;
            outerGrid.Children.Add(border2);
            border2.SetValue(Grid.RowProperty, 1);
            border2.SetValue(Grid.ColumnProperty, 3);

            games.Item1.AddDisplay(gameGrid1);
            games.Item2.AddDisplay(gameGrid2);

            AIController _aiController1 = new AIController(games.Item1, AIController.AIType.Middle);
            AIController _aiController2 = new AIController(games.Item2, AIController.AIType.Low);
            games.Item1.SetController(_aiController1);
            games.Item2.SetController(_aiController2);
            //games.Item1.SetController(_controller[0]);
            //games.Item2.SetController(_controller[1]);

            

            // set score board and next block board
            Grid scoreNextBlockGrid = new Grid();
            aCol = new ColumnDefinition();
            aCol.Width = new GridLength(1, GridUnitType.Star);
            scoreNextBlockGrid.ColumnDefinitions.Add(aCol);

            aCol = new ColumnDefinition();
            aCol.Width = new GridLength(1, GridUnitType.Auto);
            scoreNextBlockGrid.ColumnDefinitions.Add(aCol);

            aCol = new ColumnDefinition();
            aCol.Width = new GridLength(1, GridUnitType.Star);
            scoreNextBlockGrid.ColumnDefinitions.Add(aCol);

            aCol = new ColumnDefinition();
            aCol.Width = new GridLength(1, GridUnitType.Auto);
            scoreNextBlockGrid.ColumnDefinitions.Add(aCol);

            aCol = new ColumnDefinition();
            aCol.Width = new GridLength(1, GridUnitType.Star);
            scoreNextBlockGrid.ColumnDefinitions.Add(aCol);

            RowDefinition aRow = new RowDefinition();
            aRow.Height = new GridLength(1, GridUnitType.Auto);
            scoreNextBlockGrid.RowDefinitions.Add(aRow);

            aRow = new RowDefinition();
            aRow.Height = new GridLength(Styles.WindowSizeGenerator.screenHeight / 30,
                GridUnitType.Pixel);
            scoreNextBlockGrid.RowDefinitions.Add(aRow);

            aRow = new RowDefinition();
            aRow.Height = new GridLength(1, GridUnitType.Auto);
            scoreNextBlockGrid.RowDefinitions.Add(aRow);

            outerGrid.Children.Add(scoreNextBlockGrid);

            scoreNextBlockGrid.SetValue(Grid.ColumnProperty, 2);
            scoreNextBlockGrid.SetValue(Grid.RowProperty, 1);

            double scoreHeight = Styles.WindowSizeGenerator.scoreBoardHeight;
            double scoreWidth = Styles.WindowSizeGenerator.scoreBoardWidth ;

            ScoreGrid score1 = new ScoreGrid(scoreHeight, scoreWidth);
            ScoreGrid score2 = new ScoreGrid(scoreHeight, scoreWidth);

            // set next block board
            double nextBlockHeight = Styles.WindowSizeGenerator.nextBoardHeight;
            double nextBlockWidth = Styles.WindowSizeGenerator.nextBoardWidth;
            NextBlock nextBlock1 = new NextBlock(nextBlockHeight, nextBlockWidth);
            NextBlock nextBlock2 = new NextBlock(nextBlockHeight, nextBlockWidth);

            scoreNextBlockGrid.Children.Add(nextBlock1);
            nextBlock1.SetValue(Grid.ColumnProperty, 1);
            nextBlock1.SetValue(Grid.RowProperty, 2);
            nextBlock1.SetValue(Grid.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            nextBlock1.SetValue(Grid.VerticalAlignmentProperty, VerticalAlignment.Top);

            scoreNextBlockGrid.Children.Add(nextBlock2);
            nextBlock2.SetValue(Grid.ColumnProperty, 3);
            nextBlock2.SetValue(Grid.RowProperty, 2);
            nextBlock2.SetValue(Grid.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            nextBlock2.SetValue(Grid.VerticalAlignmentProperty, VerticalAlignment.Top);
            
            games.Item1.AddDisplay(nextBlock1);
            games.Item2.AddDisplay(nextBlock2);

            scoreNextBlockGrid.Children.Add(score1);
            scoreNextBlockGrid.Children.Add(score2);

            score1.SetValue(Grid.ColumnProperty, 1);
            score1.SetValue(Grid.RowProperty, 0);
            score1.SetValue(Grid.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            score1.SetValue(Grid.VerticalAlignmentProperty, VerticalAlignment.Top);

            score2.SetValue(Grid.ColumnProperty, 3);
            score2.SetValue(Grid.RowProperty, 0);
            score2.SetValue(Grid.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            score2.SetValue(Grid.VerticalAlignmentProperty, VerticalAlignment.Top);

            score1.DataContext = games.Item1.ScoreSystem;
            score2.DataContext = games.Item2.ScoreSystem;

            // set background pictures.

            Styles.SquareGenerator squareGen = new Styles.SquareGenerator();

            PicGen pic = new Cat3Gen();

            double picSizeRatio = 1.1;

            PicGenGrid pg1 = new PicGenGrid(pic, squareGen.picSquareSize() / picSizeRatio);
            aCanvas.Children.Add(pg1);
            pg1.SetValue(Canvas.ZIndexProperty, 0);

            Canvas.SetLeft(pg1, (Styles.WindowSizeGenerator.screenWidth -
                pg1.getPicSize()[1] * squareGen.picSquareSize() / picSizeRatio) / 2);
            Canvas.SetBottom(pg1, Styles.WindowSizeGenerator.gameModuleTop + 
                ( -1 * pg1.getPicSize()[0] *
                squareGen.picSquareSize() / picSizeRatio ) / 2.2 );

        }

        protected override void Loaded_Event(object sender, RoutedEventArgs e)
        {
            /*
            holderWin.Width = Styles.WindowSizeGenerator.dualModePageWidth;
            holderWin.Left = Styles.WindowSizeGenerator.dualModePageLocationLeft;

            outerGrid.Width = holderWin.Width;
            outerGrid.Height = holderWin.Height;
            */
            base.Loaded_Event(sender, e);
        }

        protected override void whatHappenWhenAnimationStop(object sender, System.Timers.ElapsedEventArgs e)
        {
            games.Item1.Start();
            games.Item2.Start();
            
            base.whatHappenWhenAnimationStop(sender, e);
        }
        protected override void keyPressed(object sender, KeyEventArgs e)
        {
            _controller[0].OnKeyDown(e);
            _controller[1].OnKeyDown(e);
            if (e.Key == Key.Enter)
            {
                games.Item1.Pause();
                games.Item2.Pause();
                EscapeDialog win = new EscapeDialog(games.Item1, games.Item2);
                win.holderWindow = this.holderWin;
                win.ShowDialog();
            }
        }
    }
}
