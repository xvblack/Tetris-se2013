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

namespace Tetris.AdvancedGUI
{
    /// <summary>
    /// CustomizedButton.xaml 的交互逻辑
    /// </summary>
    public partial class CustomizedButton : Grid
    {
        bool ifMouseDown = false;
        public RoutedEvent ClickEvent;

        Rectangle mark;

        public CustomizedButton(double height, double width, String content, int color) : base()
        {
            InitializeComponent();

            this.Height = height;
            this.Width = width;

            double markSize = height / 4;
            double labelSize = height - markSize;

            l.Height = 10000;
            l.Width = 10000;

            mark = new Rectangle();

            mark.Width = 20;
            mark.Height = mark.Width;
            Random num = new Random();

            Console.WriteLine(mark.Width);
            Console.WriteLine(this.ActualHeight);
            //mark.Fill = new SolidColorBrush(((new Styles.SquareGenerator()).getColorMap())[color]);

            mark.Fill = new SolidColorBrush(Colors.Black);

            this.Children.Add(mark);

            mark.SetValue(Grid.ColumnProperty, 0);
            mark.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            mark.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            this.ClickEvent = EventManager.RegisterRoutedEvent(
                "Click", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(CustomizedButton));

            mark.Visibility = System.Windows.Visibility.Hidden;

            this.Click += test;
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            mark.Visibility = System.Windows.Visibility.Visible;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            ifMouseDown = false;
            mark.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ifMouseDown = true;
        }

        private void Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ifMouseDown)
            {
                RoutedEventArgs ee = new RoutedEventArgs(ClickEvent, this);
                base.RaiseEvent(ee);
            }
            ifMouseDown = false;
        }

        public event RoutedEventHandler Click
        {
            add 
            {
                base.AddHandler(ClickEvent, value);
            }
            remove
            {
                base.RemoveHandler(ClickEvent, value);
            }
        }
        private void test(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("??/");

        }
    }
}
