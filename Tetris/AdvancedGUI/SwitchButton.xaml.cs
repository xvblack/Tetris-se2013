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
    /// Switching Button
    /// </summary>
    public partial class SwitchButton : Button
    {
        public SwitchButton(double height, int dir)
        {
            InitializeComponent();
            this.Height = height;
            this.Content = (dir > 0 ? ">" : "<");
            this.FontSize = WindowSizeGenerator.fontSizeLarge;
           
        }
    }
}
