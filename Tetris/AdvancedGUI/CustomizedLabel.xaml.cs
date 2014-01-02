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
    /// CustomizedPlayerSel.xaml 的交互逻辑
    /// </summary>
    public partial class CustomizedLabel : CustomizedGrid
    {
        public CustomizedLabel(Color aColor, String content):base(aColor)
        {
            ColumnDefinition aCol = new ColumnDefinition();
            aCol.Width = new GridLength(1, GridUnitType.Auto);
            this.ColumnDefinitions.Add(aCol);

            Label l = new Label();
            l.Content = content;
            l.Margin = new Thickness(20, 5, 0, 5);
            l.FontSize = WindowSizeGenerator.fontSizeMedium;
            l.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            l.SetValue(Grid.ColumnProperty, 1);
            this.Children.Add(l);
        }
    }
}
