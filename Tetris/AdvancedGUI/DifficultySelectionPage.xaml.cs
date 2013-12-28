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
    /// AchievementPage.xaml 的交互逻辑
    /// </summary>
    public partial class difficultySelectionPage : Page
    {
        NavigationService nav;
        public MainWindow holderWin;

        public difficultySelectionPage()
        {
            InitializeComponent();

            int colorNum = 6;
            Color[] colors = SquareGenerator.randomColor(colorNum);

            CustomizedButton2 dplayersModeButton =
                new CustomizedButton2("玩家 VS 玩家", colors[0]);
            dplayersModeButton.button.Click += selDualMode_Click;
            dplayersModeButton.SetValue(Grid.RowProperty, 0);

            CustomizedButton2 lowAiModeButton =
                new CustomizedButton2("玩家 VS 低级电脑", colors[1]);
            lowAiModeButton.button.Click += selDualMode_Click;
            lowAiModeButton.SetValue(Grid.RowProperty, 1);

            CustomizedButton2 mAiModeButton =
                new CustomizedButton2("玩家 VS 中级电脑", colors[2]);
            mAiModeButton.button.Click += selDualMode_Click;
            mAiModeButton.SetValue(Grid.RowProperty, 2);

            CustomizedButton2 hAiModeButton =
                new CustomizedButton2("玩家 VS 高级电脑", colors[3]);
            hAiModeButton.button.Click += selDualMode_Click;
            hAiModeButton.SetValue(Grid.RowProperty, 3);

            CustomizedButton2 dAiModeButton =
                new CustomizedButton2("电脑 VS 电脑", colors[4]);
            dAiModeButton.button.Click += selDualMode_Click;
            dAiModeButton.SetValue(Grid.RowProperty, 4);

            CustomizedButton2 backButton =
                new CustomizedButton2("后  退", colors[5]);
            backButton.button.Click += back_Click;
            backButton.SetValue(Grid.RowProperty, 6);

            ButtonsGrid.Children.Add(dplayersModeButton);
            ButtonsGrid.Children.Add(dAiModeButton);
            ButtonsGrid.Children.Add(hAiModeButton);
            ButtonsGrid.Children.Add(lowAiModeButton);
            ButtonsGrid.Children.Add(mAiModeButton);
            ButtonsGrid.Children.Add(backButton);

        }

        // NOT FINISHED!!!

        // Click to start single mode game
        private void selSingleMode_Click(object sender, RoutedEventArgs e)
        {
            nav = NavigationService.GetNavigationService(this);
            //nav.Navigate(new singleModePage());
        }

        // Click to start dual mode game
        private void selDualMode_Click(object sender, RoutedEventArgs e)
        {
            nav = NavigationService.GetNavigationService(this);;
            DualModePage nextPage = new DualModePage(0);
            nextPage.holderWin = holderWin;
            nav.Navigate(nextPage);
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
