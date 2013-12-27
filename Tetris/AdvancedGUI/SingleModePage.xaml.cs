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

            int[] nextBlockSize = new int[] { 4, 3 };

            ColumnDefinition aCol = new ColumnDefinition();
            aCol.Width = new GridLength(50, GridUnitType.Star);
            outerGrid.ColumnDefinitions.Add(aCol);

            aCol = new ColumnDefinition();
            aCol.Width = new GridLength(50, GridUnitType.Auto);
            outerGrid.ColumnDefinitions.Add(aCol);

            aCol = new ColumnDefinition();
            aCol.Width = new GridLength(50, GridUnitType.Star);
            outerGrid.ColumnDefinitions.Add(aCol);

            outerGrid.Width = Styles.WindowSizeGenerator.screenWidth;
            outerGrid.Height = Styles.WindowSizeGenerator.screenHeight;

            //outerGrid.SetValue(Canvas.BottomProperty,
                //Styles.WindowSizeGenerator.gameModuleTop);
            
    
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

            Styles.SquareGenerator squareGen = new Styles.SquareGenerator();

            //outerGrid.SetValue(Canvas.LeftProperty, Styles.WindowSizeGenerator.singleModePageLocationLeft);

            border.Child = gameGrid;
            outerGrid.Children.Add(border);
            border.SetValue(Grid.RowProperty, 1);
            border.SetValue(Grid.ColumnProperty, 1);


            // set left crocodile
            PicGen pic = new CrocodileGen();

            PicGenGrid pg1 = new PicGenGrid(pic, squareGen.picSquareSize());
            aCanvas.Children.Add(pg1);
            pg1.SetValue(Canvas.ZIndexProperty, 0);

            Canvas.SetLeft(pg1, (Styles.WindowSizeGenerator.screenWidth
                - gameGrid.getGameGridSize()[1]) / 6);

            Canvas.SetBottom(pg1, (Styles.WindowSizeGenerator.screenHeight -
                gameGrid.getGameGridSize()[0]) / 2 - 0.2 * pg1.getPicSize()[0]);

            // set right crocodile
            pic = new CrocodileGen();
            PicGenGrid pg2 = new PicGenGrid(pic, squareGen.picSquareSize());
            aCanvas.Children.Add(pg2);
            pg2.SetValue(Canvas.ZIndexProperty, 0);

            Canvas.SetRight(pg2, (Styles.WindowSizeGenerator.screenWidth
                - gameGrid.getGameGridSize()[1]) / 4);
            Canvas.SetBottom(pg2, (Styles.WindowSizeGenerator.screenHeight -
                gameGrid.getGameGridSize()[0]) / 2 - 0.2 * pg2.getPicSize()[0]);

            // set little sun
            pic = new SunGen();
            PicGenGrid pg3 = new PicGenGrid(pic, squareGen.picSquareSize() / 1.1);
            aCanvas.Children.Add(pg3);
            pg3.SetValue(Canvas.ZIndexProperty, 0);

            Canvas.SetLeft(pg3, -40);
            Canvas.SetTop(pg3, -40);

            // set score board
            double scoreHeight = Styles.WindowSizeGenerator.scoreBoardHeight;
            double scoreWidth = Styles.WindowSizeGenerator.scoreBoardWidth;
            double scoreLeftLoc = Styles.WindowSizeGenerator.scoreBoardLeft
                + (Styles.WindowSizeGenerator.screenWidth + gameGrid.getGameGridSize()[1]) / 2;
            double scoreBottomLoc = gameGrid.getGameGridSize()[0] + 
                Styles.WindowSizeGenerator.gameModuleTop - scoreHeight;
            score = new ScoreGrid(scoreHeight, scoreWidth);

            Grid scoreNextBlockGrid = new Grid();
            aCol = new ColumnDefinition();
            aCol.Width = new GridLength(0.5 * scoreWidth, GridUnitType.Pixel);
            scoreNextBlockGrid.ColumnDefinitions.Add(aCol);
            aCol = new ColumnDefinition();
            //aCol.Width = new GridLength(50, GridUnitType.Star);
            scoreNextBlockGrid.ColumnDefinitions.Add(aCol);

            RowDefinition aRow = new RowDefinition();
            aRow.Height = new GridLength(1, GridUnitType.Auto);
            scoreNextBlockGrid.RowDefinitions.Add(aRow);

            aRow = new RowDefinition();
            aRow.Height = new GridLength(0.2 * scoreHeight, GridUnitType.Pixel);
            scoreNextBlockGrid.RowDefinitions.Add(aRow);

            aRow = new RowDefinition();
            aRow.Height = new GridLength(1, GridUnitType.Star);
            scoreNextBlockGrid.RowDefinitions.Add(aRow);

            scoreNextBlockGrid.Children.Add(score);
            score.SetValue(Grid.ColumnProperty, 1);
            score.SetValue(Grid.RowProperty, 0);
            score.SetValue(Grid.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            score.SetValue(Grid.VerticalAlignmentProperty, VerticalAlignment.Top);

            outerGrid.Children.Add(scoreNextBlockGrid);
            scoreNextBlockGrid.SetValue(Grid.ColumnProperty, 2);
            scoreNextBlockGrid.SetValue(Grid.RowProperty, 1);
            

        
            //aCanvas.Children.Add(score);
            //score.SetValue(Canvas.LeftProperty, scoreLeftLoc);
            //score.SetValue(Canvas.BottomProperty, scoreBottomLoc);

            score.DataContext = game.ScoreSystem;

            // set next block board
            double nextBlockHeight = Styles.WindowSizeGenerator.nextBoardHeight;
            double nextBlockWidth = Styles.WindowSizeGenerator.nextBoardWidth;
            double nextLeftLoc = scoreLeftLoc;
            double nextBottomLoc = scoreBottomLoc + nextBlockHeight + 10;
            NextBlock nextBlock = new NextBlock(nextBlockHeight, nextBlockWidth);

            Console.WriteLine(nextBlockHeight + " block h");

            double[] nextSize = nextBlock.getSize();

            scoreNextBlockGrid.Children.Add(nextBlock);
            nextBlock.SetValue(Grid.RowProperty, 2);
            nextBlock.SetValue(Grid.ColumnProperty, 1);
            nextBlock.SetValue(Grid.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            nextBlock.SetValue(Grid.VerticalAlignmentProperty, VerticalAlignment.Top);

            //nextBlock.SetValue(Canvas.LeftProperty, nextLeftLoc);
            //nextBlock.SetValue(Canvas.TopProperty, 220.0);

            game.AddDisplay(nextBlock);

       

        }

        protected override void Loaded_Event(object sender, RoutedEventArgs e)
        {
            /*
            holderWin.Width = Styles.WindowSizeGenerator.singleModePageWidth;
            holderWin.Left = Styles.WindowSizeGenerator.singleModePageLocationLeft;

            outerGrid.Width = holderWin.Width;
            outerGrid.Height = holderWin.Height;
            */
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
            if (e.Key == Key.Enter)
            {
                game.Pause();
                EscapeDialog win = new EscapeDialog(game);
                win.holderWindow = this.holderWin;
                win.ShowDialog();
            }
        }
    }
}
