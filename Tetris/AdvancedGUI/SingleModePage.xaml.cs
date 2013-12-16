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

namespace Tetris.AdvancedGUI
{
    /// <summary>
    /// SingleModePage.xaml 的交互逻辑
    /// </summary>
    public partial class SingleModePage : Page
    {
        public MainWindow holderWin {get; set;}

        public SingleModePage()
        {
            InitializeComponent();
            int[] gridSize = new int[2] { 10, 10 };
    
            Frame aFrame = new Frame();
            outerGrid.Children.Add(aFrame);
            aFrame.SetValue(Grid.RowProperty, 1);
            aFrame.SetValue(Grid.ColumnProperty, 2);

            GamePage gamePage = new GamePage(gridSize);
            aFrame.Resources.Add(Guid.NewGuid(), gamePage);
            aFrame.Navigate(gamePage);

            StartWelcomeString welcomeString = new StartWelcomeString("READY");
            Canvas.SetLeft(welcomeString, 100);
            Canvas.SetTop(welcomeString, 200);
            aCanvas.Children.Add(welcomeString);

            // add some animation here...

            welcomeString = new StartWelcomeString("GO");
            Canvas.SetLeft(welcomeString, 100);
            Canvas.SetTop(welcomeString, 400);
            aCanvas.Children.Add(welcomeString);


        }

        private void Loaded_ChangeWinSize(object sender, RoutedEventArgs e)
        {
            this.holderWin.Width = Styles.WindowSizeGenerator.singleModePageWidth;
            this.holderWin.Left = Styles.WindowSizeGenerator.singleModePageLocationLeft;

            outerGrid.Width = this.holderWin.Width;
            outerGrid.Height = this.holderWin.Height;

            this.holderWin.PreviewKeyDown += this.keyPressed;


        }

        private void keyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                EscapeDialog win = new EscapeDialog();
                //win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.holderWindow = this.holderWin;
                win.ShowDialog();
            }
        }

        private void Unloaded_Event(object sender, RoutedEventArgs e)
        {
            this.holderWin.PreviewKeyDown -= this.keyPressed;
        }
    }
}
