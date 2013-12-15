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
        private bool _allowDirectNavigation = false;
        private NavigatingCancelEventArgs _navArgs = null;
        
        public MainWindow()
        {
            InitializeComponent();
            this.MinHeight = Styles.WindowSizeGenerator.mainWindowMinHeight;
            this.MinWidth = Styles.WindowSizeGenerator.mainWindowMinWidth;
            this.Height = this.MinHeight;
            this.Width = this.MinWidth;

            NavigationPage firstPage = new NavigationPage();
            firstPage.holderWin = this;
            this.Navigate(firstPage);
        }

        private void NavigationWindow_Navigating(object sender, NavigatingCancelEventArgs e) {
            if (Content != null && !_allowDirectNavigation) {
                e.Cancel = true;
                _navArgs = e;
                this.IsHitTestVisible = false;
                DoubleAnimation da = new DoubleAnimation(0.1d, new Duration(TimeSpan.FromMilliseconds(300)));
                da.Completed += FadeOutCompleted;
                this.BeginAnimation(OpacityProperty, da);
            }
            _allowDirectNavigation = false;
        }

        private void FadeOutCompleted(object sender, EventArgs e) {
            (sender as AnimationClock).Completed -= FadeOutCompleted;

            this.IsHitTestVisible = true;

            _allowDirectNavigation = true;
            switch (_navArgs.NavigationMode) {
                case NavigationMode.New:
                    if (_navArgs.Uri == null) {
                        NavigationService.Navigate(_navArgs.Content);
                    } else {
                        NavigationService.Navigate(_navArgs.Uri);
                    }
                    break;
                case NavigationMode.Back:
                    NavigationService.GoBack();
                    break;

                case NavigationMode.Forward:
                    NavigationService.GoForward();
                    break;
                case NavigationMode.Refresh:
                    NavigationService.Refresh();
                    break;
            }

            Dispatcher.BeginInvoke(DispatcherPriority.Loaded,
                (ThreadStart)delegate() {
                DoubleAnimation da = new DoubleAnimation(1.0d, new Duration(TimeSpan.FromMilliseconds(200)));
                this.BeginAnimation(OpacityProperty, da);
            });
        }
    }
}
