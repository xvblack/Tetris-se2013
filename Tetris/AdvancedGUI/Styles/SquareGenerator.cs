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

        static public double squareSize {
            get
            {
                return (
                (WindowSizeGenerator.gameModuleHeight / WindowSizeGenerator.gameHeight)
                    > (WindowSizeGenerator.dualGameModuleWidth / WindowSizeGenerator.gameWidth)
                    ? (WindowSizeGenerator.dualGameModuleWidth / WindowSizeGenerator.gameWidth)
                    : (WindowSizeGenerator.gameModuleHeight / WindowSizeGenerator.gameHeight)
                 );
            }
        }
        public static double picSquareSize
        {
            get { return (squareSize / 3); }
        }

        static readonly public Color[] colorMap = new Color[] {
                                Colors.Transparent,  // Transparent, no color
                                Color.FromArgb(255, 255, 68, 68),     // red
                                Color.FromArgb(255, 153, 204, 0),     // green
                                Color.FromArgb(255, 255, 187, 51),    // yellow
                                Color.FromArgb(255, 51, 181, 229),    // blue
                                Color.FromArgb(255, 170, 102, 204),   // purple
                                Color.FromArgb(255, 255, 136, 51),    // orange 
                                Colors.Gray,
                                Color.FromArgb(100, 0, 0, 0)  // black for dao ju
        };

        public static Color[] randomColor(int colorNum)
        {
            int[] colorIndex = new int[colorNum];
            Random ran = new Random();
            int i = 0;
            for (i = 0; i < colorNum; i++)
            {
                colorIndex[i] = i + 1;
            }
            int tmp = 0;
            int num = 0;
            for (i = 0; i < colorNum; i++)
            {
                num = ran.Next(colorNum - 1);
                tmp = colorIndex[num];
                colorIndex[num] = colorIndex[colorNum - 1 - num];
                colorIndex[colorNum - 1 - num] = tmp;
            }

            Color[] randomColorMap = new Color[colorNum];
            for (i = 0; i < colorNum; i++)
            {
                randomColorMap[i] = colorMap[colorIndex[i]];
            }
                
            return randomColorMap;
        }
    }
}
