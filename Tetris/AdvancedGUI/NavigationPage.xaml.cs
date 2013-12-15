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
    /// NavigationPage.xaml 的交互逻辑
    /// </summary>
    public partial class NavigationPage : Page
    {
        NavigationService nav;
        public MainWindow holderWin {get; set;}
       
        public NavigationPage() {
            InitializeComponent();
        }

        // Click the button to go to Achievement
        private void gotoAchievement_Click(object sender, RoutedEventArgs e)
        {
            nav = NavigationService.GetNavigationService(this);
            AchievementPage nextPage = new AchievementPage();
            nextPage.holderWin = holderWin;
            nav.Navigate(nextPage);
        }
        // Click the button to go to GameModeSelection Page
        private void gotoGameModeSel_Click(object sender, RoutedEventArgs e)
        {
            GameModeSelectionPage nextPage = new GameModeSelectionPage();
            nav = NavigationService.GetNavigationService(this);
            nextPage.holderWin = holderWin;
            nav.Navigate(nextPage);
        }
        // Click the button to go to Setting Page
        private void gotoSetting_Click(object sender, RoutedEventArgs e)
        {
            SettingPage nextPage = new SettingPage();
            nav = NavigationService.GetNavigationService(this);
            nextPage.holderWin = holderWin;
            nav.Navigate(nextPage);
        }
        // Click the button to exit the game
        private void exitGame_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            this.holderWin.Width = Styles.WindowSizeGenerator.mainWindowMinWidth;
            this.holderWin.Left = Styles.WindowSizeGenerator.mainWindowLocationLeft;
        }
    }
}
