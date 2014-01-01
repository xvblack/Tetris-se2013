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
    /// CustomizedButton2.xaml 的交互逻辑
    /// </summary>
    public partial class CustomizedGrid : Grid
    {

        public Rectangle mark = new Rectangle();

        public CustomizedGrid(Color aColor)
        {
            ColumnDefinition aCol = new ColumnDefinition();
            aCol.Width = new GridLength(30, GridUnitType.Pixel);
            this.ColumnDefinitions.Add(aCol);

            aCol = new ColumnDefinition();
            aCol.Width = new GridLength(1, GridUnitType.Star);
            this.ColumnDefinitions.Add(aCol);

            this.Children.Add(mark);

            mark.SetValue(Grid.ColumnProperty, 0);

            mark.Width = 20;
            mark.Height = 20;
            mark.Margin = new Thickness(0, 5, 0, 5);

            mark.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            mark.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            mark.Visibility = System.Windows.Visibility.Hidden;

            mark.Fill = new SolidColorBrush(aColor);

            
        }

        
        public void showMark(object sender, RoutedEventArgs e)
        {
            this.mark.Visibility = System.Windows.Visibility.Visible;
        }

        public void hideMark(object sender, RoutedEventArgs e)
        {
            this.mark.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
