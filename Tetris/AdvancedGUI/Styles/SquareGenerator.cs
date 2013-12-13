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

namespace Tetris.AdvancedGUI.Styles
{

    // generate squares color map
    class SquareGenerator
    {
        const int _colorNum = 9;
        static int _squareContainerSize = 30;
        static int _squareSize = 40;
        Color[] colorMap;

        public SquareGenerator() { 
            colorMap = new Color[_colorNum];
        }


        // get squares' colors
        public Color[] getColorMap() {
            
            // colors definition
            colorMap[0] = Colors.Transparent;   // Transparent, no color
            colorMap[1] = Color.FromArgb(255, 255, 68, 68);     // red
            colorMap[2] = Color.FromArgb(255, 153, 204, 0);     // green
            colorMap[3] = Color.FromArgb(255, 255, 187, 51);    // yellow
            colorMap[4] = Color.FromArgb(255, 51, 181, 229);    // blue
            colorMap[5] = Color.FromArgb(255, 170, 102, 204);   // purple
            colorMap[6] = Color.FromArgb(255, 255, 136, 51);    // orange ? // We should drop out some colors
            colorMap[7] = Colors.WhiteSmoke;
            colorMap[8] = Color.FromArgb(100, 0, 0, 0);   // black for dao ju

            return colorMap;
        }
        public static int squareSize() { return _squareSize; }
        public static int squareContainerSize() { return _squareContainerSize;  }
    }
}
