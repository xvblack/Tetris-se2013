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
    public partial class CustomizedButton2 : CustomizedGrid
    {
        //public Button button = new Button();

        public CustomizedButton2(String content, Color aColor):base(aColor)
        {
            InitializeComponent();

            button.FontSize = Styles.WindowSizeGenerator.fontSizeMedium;

            button.Content = content;

            this.MouseEnter += showMark;
            this.MouseLeave += hideMark;
        }
    }
}
