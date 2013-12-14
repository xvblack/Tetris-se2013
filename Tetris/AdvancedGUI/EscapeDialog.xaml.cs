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
    /// EscapeDialog.xaml 的交互逻辑
    /// </summary>
    public partial class EscapeDialog : Window
    {
        public MainWindow holderWindow;
        
        public EscapeDialog()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        private void okayClicked(object sender, RoutedEventArgs e) {
            Console.WriteLine("okayclikced");
            NavigationPage backPage = new NavigationPage();
            this.holderWindow.contentFrame.Resources.Add(Guid.NewGuid(), backPage);
            this.holderWindow.contentFrame.Navigate(backPage);
            backPage.holderWin = this.holderWindow;
            this.holderWindow.Width = Styles.WindowSizeGenerator.mainWindowMinWidth;
            this.holderWindow.Left = Styles.WindowSizeGenerator.mainWindowLocationLeft;
            this.Close();
        }
        private void cancelClicked(object sender, RoutedEventArgs e) { this.Close(); }
    }
}
