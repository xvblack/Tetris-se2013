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
using Tetris.AdvancedGUI.Styles;

namespace Tetris.AdvancedGUI
{
    /// <summary>
    /// Single Mode Page
    /// </summary>
    public partial class DualModePage : GameContainerPage
    {
        private Tetrisor t = new Tetrisor();  // the game
        private DuelGame games;

        gameMode p1mode ;  // which playr is playing?
        gameMode p2mode;

        private Rectangle aRect1, aRect2; // used to block game grid when game is over

        /// <summary>
        /// define a mode holder
        /// </summary>
        public class gameMode
        {
            public int player = 0;  // 0 is human; 1 is AI
            public int difficulty = 0; // 0 is low; 1 is medium; 2 is high
            public gameMode(int player, int difficulty)
            {
                this.player = player;
                this.difficulty = difficulty;
            }
        }

        public DualModePage(gameMode[] modeSels) : base()
        {
            // layout definition
            InitializeComponent();

            p1mode = modeSels[0];
            p2mode = modeSels[1];
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

            games = t.NewDuelGame(
                p1mode.player==1?"":PlayersName.getName(0),
                p2mode.player==1?"":PlayersName.getName(1)
                );
            int[] gridSize = new int[2] 
                { games.Item1.Height, games.Item1.Width };

          
            // game grid 
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

            PicGen pic = new Cat3Gen();

            double picSizeRatio = 1.1;

            PicGenGrid pg1 = new PicGenGrid(pic, SquareGenerator.picSquareSize / picSizeRatio);
            aCanvas.Children.Add(pg1);
            pg1.SetValue(Canvas.ZIndexProperty, 0);

            Canvas.SetLeft(pg1, (Styles.WindowSizeGenerator.screenWidth -
                pg1.getPicSize()[1] * SquareGenerator.picSquareSize / picSizeRatio) / 2);
            Canvas.SetBottom(pg1, Styles.WindowSizeGenerator.gameModuleTop + 
                ( -1 * pg1.getPicSize()[0] *
                SquareGenerator.picSquareSize / picSizeRatio) / 2.2);

            aRect1 = new Rectangle();
            aRect1.Fill = new SolidColorBrush(Colors.Transparent);
            aRect1.Width = gameGrid1.getGameGridSize()[1];
            aRect1.Height = gameGrid1.getGameGridSize()[0];
            outerGrid.Children.Add(aRect1);
            aRect1.SetValue(Grid.ColumnProperty, 1);
            aRect1.SetValue(Grid.RowProperty, 1);
            //game.GameEndEvent += gameEnd;
            games.DuelGameEndEvent += gameEndEffect;

            // set the mask rect (used when game is over)
            aRect2 = new Rectangle();
            aRect2.Fill = new SolidColorBrush(Colors.Transparent);
            aRect2.Width = gameGrid1.getGameGridSize()[1];
            aRect2.Height = gameGrid1.getGameGridSize()[0];
            outerGrid.Children.Add(aRect2);
            aRect2.SetValue(Grid.ColumnProperty, 3);
            aRect2.SetValue(Grid.RowProperty, 1);

        }

        protected override void Loaded_Event(object sender, RoutedEventArgs e)
        {
            base.Loaded_Event(sender, e);
        }

        // when animation stop, start the game
        protected override void whatHappenWhenAnimationStop(object sender, EventArgs e)
        {
            if (p1mode.player == 1) // player1 is AI
            {
                AIController.AIType aiType;
                if (p1mode.difficulty == 0)
                    aiType = AIController.AIType.Low;
                else if (p1mode.difficulty == 1)
                    aiType = AIController.AIType.Middle;
                else
                    aiType = AIController.AIType.High;
                AIController _aiController1 = new AIController(games.Item1, aiType);
                games.Item1.SetController(_aiController1);
            }
            else  // playrer1 is human
            {
                games.Item1.SetController(_controller[0]);
            }
            if (p2mode.player == 1) // player2 is AI
            {
                AIController.AIType aiType;
                if (p2mode.difficulty == 0)
                    aiType = AIController.AIType.Low;
                else if (p2mode.difficulty == 1)
                    aiType = AIController.AIType.Middle;
                else
                    aiType = AIController.AIType.High;
                AIController _aiController1 = new AIController(games.Item1, aiType);

                AIController _aiController2 = new AIController(games.Item2, aiType);
                games.Item2.SetController(_aiController2);
            }
            else  // playrer2 is human
            {
                games.Item2.SetController(_controller[1]);
            }

            games.Item1.Start();
            games.Item2.Start();
            gameHasStarted = true;
            
            base.whatHappenWhenAnimationStop(sender, e);
        }

        // correspone the key pressed events
        protected override void keyPressed(object sender, KeyEventArgs e)
        {
            //_controller[0].OnKeyDown(e);
            //_controller[1].OnKeyDown(e);
            // pause the game and show a exit log
            if (e.Key == Key.Escape)
            {
                games.Item1.Pause();
                games.Item2.Pause();
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

                EscapeDialog win = new EscapeDialog(this, games.Item1, games.Item2);
                win.holderWindow = this.holderWin;
                win.ShowDialog();

            }
            base.keyPressed(sender, e);

        }

        // show which one is the winner when game is over, and show the mask rect to hide the game grids
        public void gameEndEffect(object sender, int winner)
        {

            this.Dispatcher.Invoke(
                new Action(
                    delegate
                    {
                        // show the winner
                        Label l = new Label();
                        l.Content = "WINNER!";
                        l.FontSize = WindowSizeGenerator.fontSizeLarge * 1.5;
                        this.aCanvas.Children.Add(l);
                        l.SetValue(Canvas.TopProperty, 0.25 * WindowSizeGenerator.screenHeight);
                        if (winner == 0)
                            l.SetValue(Canvas.LeftProperty, 0.08 * WindowSizeGenerator.screenWidth);
                        else
                            l.SetValue(Canvas.LeftProperty, 0.70 * WindowSizeGenerator.screenWidth);
                        l.SetValue(Canvas.ZIndexProperty, 100);

                        // begin to hide the game grids
                        ColorAnimationUsingKeyFrames c1 = new ColorAnimationUsingKeyFrames();

                        double beginTime = 800;
                        double timeDelay = 500;
                        double timeStep = 000;
                        c1.KeyFrames.Add(new LinearColorKeyFrame(Colors.Transparent,
                            TimeSpan.FromMilliseconds(beginTime)));
                        c1.KeyFrames.Add(new LinearColorKeyFrame(Colors.White,
                            TimeSpan.FromMilliseconds(beginTime + timeDelay)));
                        c1.KeyFrames.Add(new LinearColorKeyFrame(Colors.White,
                            TimeSpan.FromMilliseconds(timeStep + beginTime + timeDelay))); ;
                        c1.Completed += gameEnd;
                        aRect1.Fill.BeginAnimation(SolidColorBrush.ColorProperty, c1);

                        ColorAnimationUsingKeyFrames c2 = new ColorAnimationUsingKeyFrames();
                        c2.KeyFrames.Add(new LinearColorKeyFrame(Colors.Transparent,
                            TimeSpan.FromMilliseconds(beginTime)));
                        c2.KeyFrames.Add(new LinearColorKeyFrame(Colors.White,
                            TimeSpan.FromMilliseconds(beginTime + timeDelay)));
                        c2.KeyFrames.Add(new LinearColorKeyFrame(Colors.White,
                            TimeSpan.FromMilliseconds(timeStep + beginTime + timeDelay))); ;
                        c2.Completed += gameEnd;
                        aRect2.Fill.BeginAnimation(SolidColorBrush.ColorProperty, c2);
                    }
                )
            );
        }
    }
}
