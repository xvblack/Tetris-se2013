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

        const int _brushLen = 20;
        static readonly public Brush[] brushesMap = new Brush[] {
                               new SolidColorBrush(colorMap[0]),  // 1-8: normal blocks
                               new SolidColorBrush(colorMap[1]),
                               new SolidColorBrush(colorMap[2]),
                               new SolidColorBrush(colorMap[3]),
                               new SolidColorBrush(colorMap[4]),
                               new SolidColorBrush(colorMap[5]),
                               new SolidColorBrush(colorMap[6]),
                               new SolidColorBrush(colorMap[7]), // 8
                               new RadialGradientBrush(colorMap[1], colorMap[2]), // special items // 9
                               new RadialGradientBrush(colorMap[2], colorMap[3]), // 10
                               new RadialGradientBrush(colorMap[3], colorMap[4]), // 11
                               new RadialGradientBrush(colorMap[4], colorMap[5]), // 12
                               new RadialGradientBrush(colorMap[5], colorMap[6]), // 13
                               new RadialGradientBrush(colorMap[6], colorMap[7]), // 14
                               // 15E:\我的文档\GitHub\Tetris-se2013\Tetris\AdvancedGUI\Styles\Pic\TonPic\1.jpg
                               new ImageBrush((new BitmapImage(new Uri(@"Pic\TonPic\1.jpg", UriKind.RelativeOrAbsolute)))), // 15 - 23
                               new ImageBrush((new BitmapImage(new Uri(@"Pic\TonPic\2.jpg", UriKind.Relative)))),
                               new ImageBrush((new BitmapImage(new Uri(@"Pic\TonPic\3.jpg", UriKind.Relative)))),
                               new ImageBrush((new BitmapImage(new Uri(@"Pic\TonPic\4.jpg", UriKind.Relative)))),
                               new ImageBrush((new BitmapImage(new Uri(@"Pic\TonPic\5.jpg", UriKind.Relative)))),
                               new ImageBrush((new BitmapImage(new Uri(@"Pic\TonPic\6.jpg", UriKind.Relative)))),
                               new ImageBrush((new BitmapImage(new Uri(@"Pic\TonPic\7.jpg", UriKind.Relative)))),
                               new ImageBrush((new BitmapImage(new Uri(@"Pic\TonPic\8.jpg", UriKind.Relative)))),
                               new ImageBrush((new BitmapImage(new Uri(@"Pic\TonPic\9.jpg", UriKind.Relative)))),
                               //23
                               new RadialGradientBrush(colorMap[1], colorMap[3]), // 24
                               new RadialGradientBrush(colorMap[1], colorMap[4]), // 25
                               // 26
                               new RadialGradientBrush(colorMap[4], colorMap[7]),  // 26    
                               new RadialGradientBrush(colorMap[4], colorMap[7]),
                               new RadialGradientBrush(colorMap[4], colorMap[7]),
                               new RadialGradientBrush(colorMap[4], colorMap[7]),
                               new RadialGradientBrush(colorMap[4], colorMap[7]),
                               new RadialGradientBrush(colorMap[4], colorMap[7]),
                               new RadialGradientBrush(colorMap[4], colorMap[7]),
                               new RadialGradientBrush(colorMap[4], colorMap[7]),
                               new RadialGradientBrush(colorMap[4], colorMap[7])
                               // 35
            
        };

        
        static public Brush brushClone(Brush aBrush) 
        {
            if (aBrush.GetType() == typeof(SolidColorBrush))
            {
                return (((SolidColorBrush)aBrush).Clone());
            }
            else if (aBrush.GetType() == typeof(RadialGradientBrush))
            {
                return (((RadialGradientBrush)aBrush).Clone());
            }
            else
            { //return(new SolidColorBrush(Colors.Black));
                return (((ImageBrush)aBrush).Clone());
            }

        }

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
