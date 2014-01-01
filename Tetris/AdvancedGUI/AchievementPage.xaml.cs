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
    public partial class AchievementPage : Page
    {
        NavigationService nav;
        public MainWindow holderWin;
        
        public AchievementPage()
        {
            InitializeComponent(); 
            this.FontSize = WindowSizeGenerator.fontSizeMedium;
            onePersonLabel.FontSize = WindowSizeGenerator.fontSizeLarge;
            foreach (FrameworkElement i in this.onePerson.Children)
            {              
                i.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            }
            foreach (FrameworkElement i in this.AllPeople.Children)
            {
                i.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            }
            CustomizedButton2 backButton = new CustomizedButton2("后　退", 
                SquareGenerator.colorMap[2]);
            contentGrid.Children.Add(backButton);
            backButton.SetValue(Grid.ColumnProperty, 2);
            backButton.SetValue(Grid.RowProperty, 3);
            backButton.button.Click += back_Click;
            backButton.Width = 200;
            backButton.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;

            name.Focus();
            name.Text = PlayersName.getName(0);

            okay.Click += okay_Click;

            areYouSomebody.FontSize = WindowSizeGenerator.fontSizeLarge;
        }

        // Load Highest Records
        private void highest_Loaded(object sender, RoutedEventArgs e)
        { 
            // read the achievements from AchievementSystem
            HighestScore.Content = "";
            MostTotalClearBar.Content = "";
            HighestScoreName.Content = "";
            MostTotalClearBarName.Content = "";
        }

        // Click to display the achievements of this one person
        private void okay_Click(object sender, RoutedEventArgs e)
        {
            // read the achievements from AchievementSystem
            // if this person exists
            if (false)
            {
                areYouSomebody.Content = "";
                // display the achievments
                HighScore.Content = "0";
                TotalClearBar.Content = "0";
                SeqClear.Content = ("" == "true" ? "完成" : "还没有");
                HardSurvive.Content = ("" == "true" ? "完成" : "还没有");
            }
            else
            {
                areYouSomebody.Content = "你谁啊？";
                HighScore.Content = "0";
                TotalClearBar.Content = "0";
                SeqClear.Content = "还没有";
                HardSurvive.Content = "还没有";
            }
        }
        // Click to go to the last page
        private void back_Click(object sender, RoutedEventArgs e)
        {
            nav = NavigationService.GetNavigationService(this);
            NavigationPage backPage = new NavigationPage();
            backPage.holderWin = holderWin;
            nav.Navigate(backPage);  
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            okay_Click(null, null);
            highest_Loaded(sender, e);
        }
    }
}
