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

namespace Tetris.AdvancedGUI
{
    /// <summary>
    /// GameModeSelectionPage.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class GameModeSelectionPage : Page
    {
        NavigationService nav; // Navigate the page
        public MainWindow holderWin;     // to change the size of main window
        
        public GameModeSelectionPage()
        {
            InitializeComponent();

            ButtonsGrid.Width = 300;

            double buttonHeight = 50;
            double buttonWidth = 200;

            int colorNum = 6;

            int[] colorIndex = (new Styles.SquareGenerator()).randomColorIndex(colorNum);

            CustomizedButton2 singleModeButton =
                new CustomizedButton2("单人游戏", buttonHeight, buttonWidth, colorIndex[0]);
            singleModeButton.button.Click += selDualMode_Click;
            singleModeButton.SetValue(Grid.RowProperty, 0);

            CustomizedButton2 dualModeButton =
                new CustomizedButton2("双人游戏", buttonHeight, buttonWidth, colorIndex[1]);
            dualModeButton.button.Click += selDualMode_Click;
            dualModeButton.SetValue(Grid.RowProperty, 1);
            
            CustomizedButton2 backButton =
                new CustomizedButton2("后  退", buttonHeight, buttonWidth, colorIndex[2]);
            backButton.button.Click += back_Click;
            backButton.SetValue(Grid.RowProperty, 3);

            ButtonsGrid.Children.Add(singleModeButton);
            ButtonsGrid.Children.Add(dualModeButton);
            ButtonsGrid.Children.Add(backButton);
        }

        // Click to start single mode game
        private void selSingleMode_Click(object sender, RoutedEventArgs e)
        {
            nav = NavigationService.GetNavigationService(this);
            SingleModePage nextPage = new SingleModePage();
            nextPage.holderWin = holderWin;
            nav.Navigate(nextPage);
        }

        // Click to start dual mode game
        private void selDualMode_Click(object sender, RoutedEventArgs e)
        { 
            nav = NavigationService.GetNavigationService(this);
            DualModePage nextPage = new DualModePage();
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
