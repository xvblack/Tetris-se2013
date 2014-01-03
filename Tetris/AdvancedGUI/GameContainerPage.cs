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
using Tetris.GameBase;
using System.Timers;
using Tetris.GameControl;
using Tetris.AdvancedGUI.Styles;


namespace Tetris.AdvancedGUI
{
    /// <summary>
    /// Game Container Page (the base of single mode and dual mode pages)
    /// </summary>
    public partial class GameContainerPage : Page
    {
        public MainWindow holderWin { get; set; }
        // used to set the parameters of the main window
        public StringGrid welcomeString1, welcomeString2;
        // show a greeting message

        public StringGrid gameOver;

        protected Grid outerGrid = new Grid();
        protected Canvas aCanvas = new Canvas();

        public bool gameHasStarted = false;

        public PlayerController[] _controller = new PlayerController[2];

        public GameContainerPage() 
        {

            // layout generation
            this.Loaded += Loaded_Event;
            this.Unloaded += Unloaded_Event;

            this.Content = aCanvas;

            RowDefinition aRow = new RowDefinition();
            aRow.Height = new GridLength(2, GridUnitType.Star);
            outerGrid.RowDefinitions.Add(aRow);

            aRow = new RowDefinition();
            aRow.Height = new GridLength(50, GridUnitType.Auto);
            outerGrid.RowDefinitions.Add(aRow);

            aRow = new RowDefinition();
            aRow.Height = new GridLength(1, GridUnitType.Star);
            outerGrid.RowDefinitions.Add(aRow);

            aCanvas.Children.Add(outerGrid);
            outerGrid.SetValue(Canvas.ZIndexProperty, 1);

            // controller setup

            String player1Path = Properties.Settings.Default.Player1Path;
            String player2Path = Properties.Settings.Default.Player2Path;

            _controller[0] = new PlayerController();
            _controller[0].SetConfig(new ControllerConfig(player1Path));

            _controller[1] = new PlayerController();
            _controller[1].SetConfig(new ControllerConfig(player2Path));
        }

        // the starting animation(ready go)
        protected void startAnimation()
        {
            int timeStart = 10;
            int timeStep = 1000;
            int timeDelay = timeStep + 100;


            welcomeString1 = new StringGrid("READY", SquareGenerator.squareSize / 1.8);

            Canvas.SetLeft(welcomeString1,
                (Styles.WindowSizeGenerator.screenWidth - welcomeString1.getWidth()) / 2);
            Canvas.SetTop(welcomeString1,
                (outerGrid.Height - welcomeString1.getHeight()) / 2);
            Canvas.SetZIndex(welcomeString1, 10);

            aCanvas.Children.Add(welcomeString1);
            welcomeString1.startAnimation(timeStep, timeStart);

            Console.WriteLine(outerGrid.Width);
            Console.WriteLine(welcomeString1.getWidth());
            Console.WriteLine((holderWin.Width - welcomeString1.getWidth()) / 2);

            welcomeString2 = new StringGrid("GO", SquareGenerator.squareSize);
            Canvas.SetLeft(welcomeString2,
                (Styles.WindowSizeGenerator.screenWidth - welcomeString2.getWidth()) / 2);
            Canvas.SetTop(welcomeString2,
                (holderWin.Height - welcomeString2.getHeight()) / 2);
            Canvas.SetZIndex(welcomeString2, 10);

            aCanvas.Children.Add(welcomeString2);
            welcomeString2.startAnimation(timeStep, timeDelay + timeStart);

            welcomeString2.story.Completed += whatHappenWhenAnimationStop;


            welcomeString1.beginAnimation();
            welcomeString2.beginAnimation();

            gameOver = new StringGrid("Game Over", SquareGenerator.squareSize / 2.5);

            aCanvas.Children.Add(gameOver);

            Canvas.SetLeft(gameOver,
                (Styles.WindowSizeGenerator.screenWidth - gameOver.getWidth()) / 2);
            Canvas.SetTop(gameOver,
                (outerGrid.Height - gameOver.getHeight()) / 2);
            Canvas.SetZIndex(gameOver, 10);

            gameOver.startAnimation(1500, 0);
            
        }

        // show the animation when the game is over
        protected void gameEnd(object sender, EventArgs e)
        {
            gameOver.story.Completed += showExitLog;
            gameOver.beginAnimation();
        }

        protected void showExitLog(object sender, EventArgs e)
        {
            NavigationPage backPage = new NavigationPage();
            backPage.holderWin = this.holderWin;
            this.holderWin.Navigate(backPage);
        }

        virtual protected void Loaded_Event(object sender, RoutedEventArgs e)
        {           
            this.holderWin.PreviewKeyDown += this.keyPressed;
            this.startAnimation();
        }

        // the starting animation is stoped
        virtual protected void whatHappenWhenAnimationStop(object sender,
            EventArgs e) { }

        virtual protected void keyPressed(object sender, KeyEventArgs e)
        {
            _controller[0].OnKeyDown(e);
            _controller[1].OnKeyDown(e);

        }

        protected void Unloaded_Event(object sender, RoutedEventArgs e)
        {
            this.holderWin.PreviewKeyDown -= this.keyPressed;
        }
    }
}
