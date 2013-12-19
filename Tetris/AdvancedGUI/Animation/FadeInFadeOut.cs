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

namespace Tetris.AdvancedGUI.Animation
{
    public class FadeInFadeOut : WindowNavigationEffect
    {
        public FadeInFadeOut() : base() { }

        public override void startEffect(object sender, NavigatingCancelEventArgs e)
        {
            //base.startEffect(sender, e);

            if (targetWin.Content != null && !targetWin._allowDirectNavigation)
            {
                e.Cancel = true;
                targetWin._navArgs = e;
                targetWin.IsHitTestVisible = false;
                DoubleAnimation da = new DoubleAnimation(0.0d, new Duration(TimeSpan.FromMilliseconds(300)));
                da.Completed += endEffect;
                targetWin.BeginAnimation(MainWindow.OpacityProperty, da);
            }
            targetWin._allowDirectNavigation = false;

        }
        public override void endEffect(object sender, EventArgs e)
        {
            base.endEffect(sender, e);

            targetWin.Dispatcher.BeginInvoke(DispatcherPriority.Loaded,
                (ThreadStart)delegate()
            {
                DoubleAnimation da = new DoubleAnimation(1.0d, new Duration(TimeSpan.FromMilliseconds(200)));
                targetWin.BeginAnimation(MainWindow.OpacityProperty, da);
            });
        }
    }
}
