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
using Tetris.AdvancedGUI.Styles;

namespace Tetris.AdvancedGUI
{
    /// <summary>
    /// Single Mode Pages, similar to dual page
    /// </summary>
    public partial class SingleModePage : GameContainerPage
    {
        private Tetrisor t = new Tetrisor();
        private GameGrid gameGrid;
        private Tetris.GameBase.TetrisGame game;
        private Rectangle aRect;

        private ScoreGrid score;

        public SingleModePage()
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
    
            Border border = new Border();

            border.BorderBrush = new SolidColorBrush(Colors.Gray);
            border.BorderThickness = new Thickness(2, 2, 2, 2);

            game = t.NewGame(PlayersName.getName(0));

            int[] gridSize = new int[2] { game.Height, game.Width };

            gameGrid = new GameGrid(gridSize);

            game.AddDisplay(gameGrid);

            border.Child = gameGrid;
            outerGrid.Children.Add(border);
            border.SetValue(Grid.RowProperty, 1);
            border.SetValue(Grid.ColumnProperty, 1);


            // set left crocodile
            PicGen pic = new CrocodileGen();

            PicGenGrid pg1 = new PicGenGrid(pic, SquareGenerator.picSquareSize);
            aCanvas.Children.Add(pg1);
            pg1.SetValue(Canvas.ZIndexProperty, 0);

            Canvas.SetLeft(pg1, (Styles.WindowSizeGenerator.screenWidth
                - gameGrid.getGameGridSize()[1]) / 6);

            Canvas.SetBottom(pg1, (Styles.WindowSizeGenerator.screenHeight -
                gameGrid.getGameGridSize()[0]) / 2 - 0.2 * pg1.getPicSize()[0]);

            // set right crocodile
            pic = new CrocodileGen();
            PicGenGrid pg2 = new PicGenGrid(pic, SquareGenerator.picSquareSize);
            aCanvas.Children.Add(pg2);
            pg2.SetValue(Canvas.ZIndexProperty, 0);

            Canvas.SetRight(pg2, (Styles.WindowSizeGenerator.screenWidth
                - gameGrid.getGameGridSize()[1]) / 4);
            Canvas.SetBottom(pg2, (Styles.WindowSizeGenerator.screenHeight -
                gameGrid.getGameGridSize()[0]) / 2 - 0.2 * pg2.getPicSize()[0]);

            // set little sun
            pic = new SunGen();
            PicGenGrid pg3 = new PicGenGrid(pic, SquareGenerator.picSquareSize / 1.1);
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

            game.AddDisplay(nextBlock);

            aRect = new Rectangle();
            aRect.Fill = new SolidColorBrush(Colors.Transparent);
            aRect.Width = gameGrid.getGameGridSize()[1];
            aRect.Height = gameGrid.getGameGridSize()[0];
            outerGrid.Children.Add(aRect);
            aRect.SetValue(Grid.ColumnProperty, 1);
            aRect.SetValue(Grid.RowProperty, 1);
            game.GameEndEvent += gameEndEffect;


        }

        protected override void Loaded_Event(object sender, RoutedEventArgs e)
        {
            base.Loaded_Event(sender, e);

        }

        protected override void whatHappenWhenAnimationStop(object sender, EventArgs e)
        {
            game.SetController(_controller[0]);
            gameHasStarted = true;
            game.Start();
 

            base.whatHappenWhenAnimationStop(sender, e);
        }

        protected override void keyPressed(object sender, KeyEventArgs e)
        {
            _controller[0].OnKeyDown(e);
            if (e.Key == Key.Enter)
            {
                game.Pause();
                if ((welcomeString1.pauseState == false))
                {
                    welcomeString1.story.Pause(welcomeString1);
                    welcomeString2.story.Pause(welcomeString2);
                    welcomeString1.pauseState = true;
                }
                else
                {
                    welcomeString1.story.Resume(welcomeString1);
                    welcomeString2.story.Resume(welcomeString2);
                    welcomeString1.pauseState = false;
                }
                EscapeDialog win = new EscapeDialog(this, game);
                win.holderWindow = this.holderWin;
                win.ShowDialog();

            }
            base.keyPressed(sender, e);
        }

        public void gameEndEffect(object sender, Tetris.GameBase.TetrisGame.GameEndEventArgs e)
        {
            
            this.Dispatcher.Invoke(
                new Action(
                    delegate
                    {
                        ColorAnimationUsingKeyFrames c = new ColorAnimationUsingKeyFrames();
                        SolidColorBrush b = new SolidColorBrush();

                        double beginTime = 800;
                        double timeDelay = 500;
                        double timeStep = 000;
                        c.KeyFrames.Add(new LinearColorKeyFrame(Colors.Transparent,
                            TimeSpan.FromMilliseconds(beginTime)));
                        c.KeyFrames.Add(new LinearColorKeyFrame(Colors.White,
                            TimeSpan.FromMilliseconds(beginTime + timeDelay)));
                        c.KeyFrames.Add(new LinearColorKeyFrame(Colors.White,
                            TimeSpan.FromMilliseconds(timeStep + beginTime + timeDelay)));;
                        c.Completed += gameEnd;
                        aRect.Fill.BeginAnimation(SolidColorBrush.ColorProperty, c);
                    }
                )
            );
        }
    }

}
