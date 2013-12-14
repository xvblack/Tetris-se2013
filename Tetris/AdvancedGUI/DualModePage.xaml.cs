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
    /// dualModePage.xaml 的交互逻辑
    /// </summary>
    public partial class DualModePage : Page
    {
        public MainWindow holderWin { get; set; }
        
        public DualModePage()
        {
            InitializeComponent();

            int[] gridSize = new int[2] {10, 10};

            Frame leftFrame = new Frame();
            outerGrid.Children.Add(leftFrame);
            leftFrame.SetValue(Grid.RowProperty, 1);
            leftFrame.SetValue(Grid.ColumnProperty, 1);

            GamePage leftGamePage = new GamePage(gridSize);
            leftFrame.Resources.Add(Guid.NewGuid(), leftGamePage);
            leftFrame.Navigate(leftGamePage);

            Frame rightFrame = new Frame();
            outerGrid.Children.Add(rightFrame);
            rightFrame.SetValue(Grid.RowProperty, 1);
            rightFrame.SetValue(Grid.ColumnProperty, 3);

            GamePage rightGamePage = new GamePage(gridSize);
            rightFrame.Resources.Add(Guid.NewGuid(), rightGamePage);
            rightFrame.Navigate(rightGamePage);
            
        }

        private void Loaded_ChangeWinSize(object sender, RoutedEventArgs e)
        {
            //this.holderWin.Width = 1200;
            Console.WriteLine(this.holderWin);
            this.holderWin.Width = Styles.WindowSizeGenerator.dualModePageWidth;
            this.holderWin.Left = Styles.WindowSizeGenerator.dualModePageLocationLeft;

            this.holderWin.PreviewKeyDown += this.keyPressed;
        }

        private void keyPressed(object sender, KeyEventArgs e) {
            if (e.Key == Key.Escape) {
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
