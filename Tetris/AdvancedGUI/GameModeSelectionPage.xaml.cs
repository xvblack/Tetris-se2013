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
    /// GameModeSelectionPage.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class GameModeSelectionPage : Page
    {
        NavigationService nav; // Navigate the page
        
        public GameModeSelectionPage()
        {
            InitializeComponent();
        }

        // Click to start single mode game
        private void selSingleMode_Click(object sender, RoutedEventArgs e)
        {
            Window win = new Tetris.MainWindow();
            win.Show();
            
            //nav = NavigationService.GetNavigationService(this);
            //nav.Navigate(new singleModePage());
        }

        // Click to start dual mode game
        private void selDualMode_Click(object sender, RoutedEventArgs e)
        {
            TestClasses.TestWindow win = new TestClasses.TestWindow();
            Frame aFrame = new Frame();
            win.grid.Children.Add(aFrame);
            GamePage page = new GamePage(100, 100);
            aFrame.Resources.Add(Guid.NewGuid(), page);
            aFrame.Navigate(page);
            win.Show();
            //win.grid.Children.Add(page);
            
            //nav = NavigationService.GetNavigationService(this);
            //nav.Navigate(new dualModePage());
        }
        // Click to go to the last page
        private void back_Click(object sender, RoutedEventArgs e)
        {
            nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new NavigationPage());            
        }
    }
}
