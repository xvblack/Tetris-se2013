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
using System.Windows.Navigation;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Tetris.AdvancedGUI.Animation
{
    public class WindowNavigationEffect
    {
        public MainWindow targetWin { set; get; }

        public WindowNavigationEffect() 
        {
            //this.targetWin = targetWin;
        }

        virtual public void startEffect(object sender, NavigatingCancelEventArgs e) { }

        virtual public void endEffect(object sender, EventArgs e) 
        {
            (sender as AnimationClock).Completed -= endEffect;

            targetWin.IsHitTestVisible = true;

            targetWin._allowDirectNavigation = true;
            switch (targetWin._navArgs.NavigationMode)
            {
                case NavigationMode.New:
                    if (targetWin._navArgs.Uri == null)
                    {
                        targetWin.NavigationService.Navigate(targetWin._navArgs.Content);
                    }
                    else
                    {
                        targetWin.NavigationService.Navigate(targetWin._navArgs.Uri);
                    }
                    break;
                case NavigationMode.Back:
                    targetWin.NavigationService.GoBack();
                    break;

                case NavigationMode.Forward:
                    targetWin.NavigationService.GoForward();
                    break;
                case NavigationMode.Refresh:
                    targetWin.NavigationService.Refresh();
                    break;
            }
        }
    }
}
