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

        private double _squareSize;
        private double _picSquareSize;
        private Color[] _colorMap;

        public SquareGenerator() {

            double tmpHeight = WindowSizeGenerator.gameModuleHeight 
                / WindowSizeGenerator.gameHeight;

            double tmpWidth = WindowSizeGenerator.dualGameModuleWidth
                / WindowSizeGenerator.gameWidth;

            if (tmpHeight < tmpWidth)
                _squareSize = tmpHeight;
            else
                _squareSize = tmpWidth;

            _picSquareSize = _squareSize / 3;
            _colorMap = new Color[_colorNum];
            setColorMap();
        }

        public double squareSize()
        {
            return _squareSize;
        }

        public double picSquareSize()
        {
            return _picSquareSize;
        }

        public Color[] colorMap() 
        {
            return _colorMap;
        }
        // get squares' colors
        private void setColorMap() {
            
            // colors definition
            _colorMap[0] = Colors.Transparent;   // Transparent, no color
            _colorMap[1] = Color.FromArgb(255, 255, 68, 68);     // red
            _colorMap[2] = Color.FromArgb(255, 153, 204, 0);     // green
            _colorMap[3] = Color.FromArgb(255, 255, 187, 51);    // yellow
            _colorMap[4] = Color.FromArgb(255, 51, 181, 229);    // blue
            _colorMap[5] = Color.FromArgb(255, 170, 102, 204);   // purple
            _colorMap[6] = Color.FromArgb(255, 255, 136, 51);    // orange ? // We should drop out some colors
            _colorMap[7] = Colors.Gray;
            _colorMap[8] = Color.FromArgb(100, 0, 0, 0);   // black for dao ju
        }

        public int[] randomColorIndex(int colorNum)
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
            return colorIndex;
        }
    }
}
