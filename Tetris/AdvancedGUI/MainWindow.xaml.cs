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
using System.Windows.Media.Animation;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Navigation;

namespace Tetris.AdvancedGUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public bool _allowDirectNavigation = false;
        public NavigatingCancelEventArgs _navArgs = null;
        
        public MainWindow()
        {
            InitializeComponent();
            
            // which window size is used?
            if (Styles.WindowSizeGenerator.windowSize == "Maximum")
            {
                //this.WindowState = WindowState.Maximized;
                this.ResizeMode = System.Windows.ResizeMode.NoResize;
                this.WindowStyle = System.Windows.WindowStyle.None;
                //this.Topmost = true;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }

            this.MinHeight = Styles.WindowSizeGenerator.mainWindowMinHeight;
            this.MinWidth = Styles.WindowSizeGenerator.mainWindowMinWidth;
            this.Height = this.MinHeight;
            this.Width = this.MinWidth;

            NavigationPage firstPage = new NavigationPage();
            firstPage.holderWin = this;
            this.Navigate(firstPage);
        }

        private void NavigationWindow_Navigating(object sender, NavigatingCancelEventArgs e) {
            Tetris.AdvancedGUI.Animation.FadeInFadeOut effect =
                new Tetris.AdvancedGUI.Animation.FadeInFadeOut();
            effect.targetWin = this;
            effect.startEffect(sender, e);
        }
    }
}
