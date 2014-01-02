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
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Tetris.AdvancedGUI.Styles;

namespace Tetris.AdvancedGUI
{

    public class SwitchLabel : Canvas
    {
        int labelNum = 0;
        Label[] labelArray;
        int whichLabelShow = 0;

        DoubleAnimation ap = new DoubleAnimation();
        DoubleAnimation bp = new DoubleAnimation();
        DoubleAnimation an = new DoubleAnimation();
        DoubleAnimation bn = new DoubleAnimation();

        const double height = 100;
        const double width = 200;
        const double buttonWidth = 50;

        public SwitchButton p = new SwitchButton(height, 1);
        public SwitchButton n = new SwitchButton(height, 0);

        public SwitchLabel(String[] contents)
        {
            labelNum = contents.Length;
            labelArray = new Label[labelNum];

            this.Height = height;

            this.Children.Add(n);
            this.Children.Add(p);

            Rectangle pRect = setMaskRect(0);
            Rectangle nRect = setMaskRect(1);

            n.SetValue(Canvas.LeftProperty, 0.0);
            p.SetValue(Canvas.RightProperty, 0.0);

            n.SetValue(Canvas.ZIndexProperty, 10);
            p.SetValue(Canvas.ZIndexProperty, 10);

            this.Width = 2 * buttonWidth + width;

            this.ClipToBounds = true;

            for (int i = 0; i < labelNum; i++)
            {
                labelArray[i] = new Label();
                labelArray[i].FontSize = WindowSizeGenerator.fontSizeMedium;
                labelArray[i].Height = height;
                labelArray[i].Width = width;
                labelArray[i].Content = contents[i];
                labelArray[i].HorizontalContentAlignment =
                    System.Windows.HorizontalAlignment.Center;
                labelArray[i].VerticalContentAlignment =
                    System.Windows.VerticalAlignment.Center;
            }

            double dur = 0.1;

            setAnimation(ap, buttonWidth, buttonWidth - width, dur, 1);
            setAnimation(bp, buttonWidth + width, buttonWidth, dur, 0);

            setAnimation(an, buttonWidth, buttonWidth + width, dur, -1);
            setAnimation(bn, buttonWidth - width, buttonWidth, dur, 0);

            p.Click += pPressed;
            n.Click += nPressed;

            // startup content
            this.Children.Add(labelArray[0]);
            labelArray[0].SetValue(Canvas.LeftProperty, buttonWidth);
            whichLabelShow = 0;
        }

        private Rectangle setMaskRect(int pos)
        {
            Rectangle rect = new Rectangle();
            rect.Width = buttonWidth;
            rect.Height = height;
            rect.Fill = new SolidColorBrush(Colors.White);
            if (pos <= 0)
                rect.SetValue(Canvas.LeftProperty, 0.0);
            else
                rect.SetValue(Canvas.RightProperty, 0.0);
            rect.SetValue(Canvas.ZIndexProperty, 2);

            this.Children.Add(rect);

            return (rect);
        }

        private void pPressed(object sender, RoutedEventArgs e)
        {
            if (whichLabelShow < labelNum - 1)
            {
                n.Click -= nPressed;
                p.Click -= pPressed;
                Label next = labelArray[whichLabelShow + 1];
                Label present = labelArray[whichLabelShow];

                this.Children.Add(next);

                present.BeginAnimation(Canvas.LeftProperty, ap);
                next.BeginAnimation(Canvas.LeftProperty, bp);
            }
        }
        private void pAnimationDone(object sender, EventArgs e)
        {
            this.Children.Remove(labelArray[whichLabelShow]);
            whichLabelShow++;
            n.Click += nPressed;
            p.Click += pPressed;
        }

        private void nAnimationDone(object sender, EventArgs e)
        {
            this.Children.Remove(labelArray[whichLabelShow]);
            whichLabelShow--;
            p.Click += pPressed;
            n.Click += nPressed;
        }

        private void nPressed(object sender, RoutedEventArgs e)
        {
            if (whichLabelShow > 0)
            {
                n.Click -= nPressed;
                p.Click -= pPressed;
                Label next = labelArray[whichLabelShow - 1];
                Label present = labelArray[whichLabelShow];

                this.Children.Add(next);

                present.BeginAnimation(Canvas.LeftProperty, an);
                next.BeginAnimation(Canvas.LeftProperty, bn);
            }
        }
        private void setAnimation(DoubleAnimation a, double from, double to, double dur, int dir)
        {
            a.From = from;
            a.To = to;
            a.Duration = new Duration(TimeSpan.FromSeconds(dur));
            if (dir > 0) a.Completed += pAnimationDone;
            else if (dir < 0) a.Completed += nAnimationDone;
        }

        public int getLabelIndex() { return whichLabelShow; }
    }
}
