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
using Tetris.AdvancedGUI.Styles;

namespace Tetris.AdvancedGUI.TestClasses
{
    /// <summary>
    /// TestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestWindow : Window
    {
        public TestWindow()
        {
            InitializeComponent();

            Canvas aCanvas = new Canvas();

            this.Height = WindowSizeGenerator.screenHeight;
            this.Width = WindowSizeGenerator.screenWidth;

            this.Content = aCanvas;

            /*
            StringGrid gameOver = new StringGrid("Game Over", SquareGenerator.squareSize / 2.5);

            aCanvas.Children.Add(gameOver);

            gameOver.startAnimation(1500, 0);

            gameOver.beginAnimation();

            Label l = new Label();

            l.Content = "gds";

            grid.Children.Add(l);
            */

            SwitchLabel l = new SwitchLabel((new string[4]{"label1", "label2", "label3", "label4"}));
            aCanvas.Children.Add(l);

            this.Show();
        }
    }
}
