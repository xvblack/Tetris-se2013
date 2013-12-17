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
    /// AchievementPage.xaml 的交互逻辑
    /// </summary>
    public partial class AchievementPage : Page
    {
        NavigationService nav;
        public MainWindow holderWin;
        
        public AchievementPage()
        {
            InitializeComponent();
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