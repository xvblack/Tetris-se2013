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
using System.Windows.Shapes;

namespace Tetris.AdvancedGUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.MinHeight = Styles.WindowSizeGenerator.mainWindowMinHeight;
            this.MinWidth = Styles.WindowSizeGenerator.mainWindowMinWidth;
            this.Height = this.MinHeight;
            this.Width = this.MinWidth;
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            NavigationPage firstPage = new NavigationPage();
            firstPage.holderWin = this;
            contentFrame.Resources.Add(Guid.NewGuid(), firstPage);
            contentFrame.Navigate(firstPage);  
        }

        private void contentFrame_Navigated_1(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            //String[] addMainWinRefList = { "NavigationPage", "s"};
            /*
            String currentPage = e.Content.ToString();
            if (currentPage.Contains("NavigationPage"))
            {
                contentFrame.Content
            }**/
        }
    }
}
