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
    public partial class CustomizedButton2 : Grid
    {
        //public Button button = new Button();

        public CustomizedButton2(String content, Color aColor)
        {
            InitializeComponent();

            button.FontSize = Styles.WindowSizeGenerator.fontSizeMedium;

            mark.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            mark.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            mark.Visibility = System.Windows.Visibility.Hidden;

            mark.Fill = new SolidColorBrush(aColor);

            button.MouseEnter += showMark;
            button.MouseLeave += hideMark;

            button.Content = content;
        }

        private void showMark(object sender, MouseEventArgs e)
        {
            this.mark.Visibility = System.Windows.Visibility.Visible;
        }

        private void hideMark(object sender, MouseEventArgs e)
        {
            this.mark.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
