﻿using System;
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
    /// Navigation Pages
    /// </summary>
    public partial class NavigationPage : Page
    {
        NavigationService nav;
        public MainWindow holderWin {get; set;}
       
        public NavigationPage() {
            InitializeComponent();

            int colorNum = 6;
            
            Color[] colors = SquareGenerator.randomColor(colorNum);

            CustomizedButton2 exitButton = 
                new CustomizedButton2("退  出", colors[0]);
            exitButton.button.Click += exitGame_Click;
            exitButton.SetValue(Grid.RowProperty, 3);

            CustomizedButton2 startGameButton =
                new CustomizedButton2("开始游戏", colors[1]);
            startGameButton.button.Click += gotoGameModeSel_Click;
            startGameButton.SetValue(Grid.RowProperty, 0);

            CustomizedButton2 achievementButton =
                new CustomizedButton2("成就系统", colors[2]);
            achievementButton.button.Click += gotoAchievement_Click;
            achievementButton.SetValue(Grid.RowProperty, 1);


            CustomizedButton2 settingsButton =
                new CustomizedButton2("设  置", colors[3]); 
            settingsButton.button.Click += gotoSetting_Click;
            settingsButton.SetValue(Grid.RowProperty, 2);
 
            ButtonsGrid.Children.Add(startGameButton);
            ButtonsGrid.Children.Add(achievementButton);
            ButtonsGrid.Children.Add(settingsButton);
            ButtonsGrid.Children.Add(exitButton);


            StringGrid title = new StringGrid("Tetris", SquareGenerator.squareSize / 1.1);
            
            outerGrid.Children.Add(title);
            
            title.SetValue(Grid.RowProperty, 1);
            
            title.SetValue(Grid.ColumnProperty, 1);
            
            title.noAnimation();
            
            Pic.Cat2Gen pic = new Pic.Cat2Gen();
            Pic.PicGenGrid pg = new Pic.PicGenGrid(pic, SquareGenerator.picSquareSize / 1.2);
            aCanvas.Children.Add(pg);
            pg.SetValue(Canvas.ZIndexProperty, 0);

            Canvas.SetRight(pg, 2);
            Canvas.SetBottom(pg, 2);
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
        }
    }
}
