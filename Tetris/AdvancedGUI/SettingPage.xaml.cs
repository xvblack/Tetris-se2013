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
using Tetris.AdvancedGUI.Styles;

namespace Tetris.AdvancedGUI
{
    /// <summary>
    /// SettingPage.xaml 的交互逻辑
    /// </summary>
    public partial class SettingPage : Page
    {
        NavigationService nav;
        public MainWindow holderWin;
        
        public SettingPage()
        {
            InitializeComponent();

            int colorNum = 6;
            Color[] colors = SquareGenerator.randomColor(colorNum);

            CustomizedButton2 singleModeButton =
                new CustomizedButton2("单人游戏", colors[0]);
            singleModeButton.button.Click += selDualMode_Click;
            singleModeButton.SetValue(Grid.RowProperty, 0);

            CustomizedButton2 dualModeButton =
                new CustomizedButton2("双人游戏", colors[1]);
            dualModeButton.button.Click += selDualMode_Click;
            dualModeButton.SetValue(Grid.RowProperty, 1);
            
            CustomizedButton2 backButton =
                new CustomizedButton2("后  退", colors[2]);
            backButton.button.Click += back_Click;
            backButton.SetValue(Grid.RowProperty, 3);

            ButtonsGrid.Children.Add(singleModeButton);
            ButtonsGrid.Children.Add(dualModeButton);
            ButtonsGrid.Children.Add(backButton);
        }

        // NOT FINISEHD!!!

        // Click to start single mode game
        private void selSingleMode_Click(object sender, RoutedEventArgs e)
        {
            nav = NavigationService.GetNavigationService(this);
            //nav.Navigate(new singleModePage());
        }

        // Click to start dual mode game
        private void selDualMode_Click(object sender, RoutedEventArgs e)
        {
            nav = NavigationService.GetNavigationService(this);
            //nav.Navigate(new dualModePage());
        }
        // Click to go to the last page
        private void back_Click(object sender, RoutedEventArgs e)
        {
            nav = NavigationService.GetNavigationService(this);
            NavigationPage backPage = new NavigationPage();
            backPage.holderWin = holderWin;
            nav.Navigate(backPage);  
        }
    }
}
