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

namespace Tetris.AdvancedGUI
{
    /// <summary>
    /// GameContainerPage.xaml 的交互逻辑
    /// </summary>
    public partial class GameContainerPage : Page
    {
        public MainWindow holderWin { get; set; }
        // used to set the parameters of the main window
        protected StartWelcomeString welcomeString;
        // show a greeting message

        protected Grid outerGrid = new Grid();
        protected Canvas aCanvas = new Canvas();

        public PlayerController[] _controller = new PlayerController[2];

        Timer whenGameBegin = new Timer();

        public GameContainerPage() 
        {
            this.Loaded += Loaded_Event;
            this.Unloaded += Unloaded_Event;

            this.Content = aCanvas;

            RowDefinition aRow = new RowDefinition();
            aRow.Height = new GridLength(50, GridUnitType.Star);
            outerGrid.RowDefinitions.Add(aRow);

            aRow = new RowDefinition();
            aRow.Height = new GridLength(50, GridUnitType.Auto);
            outerGrid.RowDefinitions.Add(aRow);

            aRow = new RowDefinition();
            aRow.Height = new GridLength(50, GridUnitType.Star);
            outerGrid.RowDefinitions.Add(aRow);

            aCanvas.Children.Add(outerGrid);
            outerGrid.SetValue(Canvas.ZIndexProperty, 1);

            _controller[0] = new PlayerController();
            _controller[0].SetConfig(new ControllerConfig(ControllerConfig.ConfigType.Player1));

            _controller[1] = new PlayerController();
            _controller[1].SetConfig(new ControllerConfig(ControllerConfig.ConfigType.Player2));
        }

        protected void startAnimation()
        {
            int timeStart = 500;
            int timeStep = 1200;
            int timeDelay = timeStep + 500;
            

            welcomeString = new StartWelcomeString("READY");

            if (double.IsNaN(outerGrid.Width))
            {
                outerGrid.Width = Styles.WindowSizeGenerator.mainWindowMinWidth;
            }

            if (double.IsNaN(outerGrid.Height))
            {
                outerGrid.Height = Styles.WindowSizeGenerator.mainWindowMinHeight;
            }

            Canvas.SetLeft(welcomeString,
                (Styles.WindowSizeGenerator.screenSize - welcomeString.getWidth()) / 2);
            Canvas.SetTop(welcomeString,
                (outerGrid.Height - welcomeString.getHeight()) / 2);
            Canvas.SetZIndex(welcomeString, 10);

            aCanvas.Children.Add(welcomeString);
            welcomeString.startAnimation(timeStep, timeStart);

            Console.WriteLine(outerGrid.Width);
            Console.WriteLine(welcomeString.getWidth());
            Console.WriteLine((holderWin.Width - welcomeString.getWidth()) / 2);

            welcomeString = new StartWelcomeString("GO");
            Canvas.SetLeft(welcomeString,
                (Styles.WindowSizeGenerator.screenSize - welcomeString.getWidth()) / 2);
            Canvas.SetTop(welcomeString,
                (holderWin.Height - welcomeString.getHeight()) / 2);
            Canvas.SetZIndex(welcomeString, 10);

            aCanvas.Children.Add(welcomeString);
            welcomeString.startAnimation(timeStep, timeDelay + timeStart);

            whenGameBegin.Interval = timeStep + timeStep + timeDelay;
            whenGameBegin.Elapsed += whatHappenWhenAnimationStop;

            whenGameBegin.Start();
        }

        virtual protected void Loaded_Event(object sender, RoutedEventArgs e)
        {           
            this.holderWin.PreviewKeyDown += this.keyPressed;
            this.startAnimation();
        }

        virtual protected void whatHappenWhenAnimationStop(object sender,
            ElapsedEventArgs e) 
        {
            Console.WriteLine("stop");
            whenGameBegin.Stop();
        }

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
