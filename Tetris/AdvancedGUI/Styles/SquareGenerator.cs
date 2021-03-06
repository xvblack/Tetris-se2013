﻿using System;
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
    /// <summary>
    /// generate squares color map
    /// </summary>
    /// 
    class SquareGenerator
    {
        const int _colorNum = 9;
        const string path = "./AdvancedGUI/Styles/Pic/";  // where the pics of items are stored

        // return the size of a square
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

        // return the size of a square of background pics
        public static double picSquareSize
        {
            get { return (squareSize / 3); }
        }

        // the colors used by the squares
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

        // the brushes to show the square 
        const int _brushLen = 20;
        static readonly public Brush[] brushesMap = new Brush[] {
                               new SolidColorBrush(colorMap[0]),  
                               new SolidColorBrush(colorMap[1]), // 1-8: normal blocks
                               new SolidColorBrush(colorMap[2]),
                               new SolidColorBrush(colorMap[3]),
                               new SolidColorBrush(colorMap[4]),
                               new SolidColorBrush(colorMap[5]),
                               new SolidColorBrush(colorMap[6]),
                               new SolidColorBrush(colorMap[7]),
                               new SolidColorBrush(colorMap[8]), // 8
                               // special triggering items 
                               new ImageBrush((new BitmapImage(new Uri(path + "g.jpg", UriKind.Relative)))), // 9
                               new ImageBrush((new BitmapImage(new Uri(path + "ig.jpg", UriKind.Relative)))), // 10
                               new ImageBrush((new BitmapImage(new Uri(path + "t.jpg", UriKind.Relative)))), // 11
                               new ImageBrush((new BitmapImage(new Uri(path + "i.jpg", UriKind.Relative)))), // 12

                               // gun and the inverse
                               new ImageBrush((new BitmapImage(new Uri(path + "GunPic/1.jpg", UriKind.Relative)))), // 13
                               new ImageBrush((new BitmapImage(new Uri(path + "GunPic/2.jpg", UriKind.Relative)))), // 14
                               // ton
                               new ImageBrush((new BitmapImage(new Uri(path + "TonPic/7.jpg", UriKind.RelativeOrAbsolute)))), // 15 - 23
                               new ImageBrush((new BitmapImage(new Uri(path + "TonPic/8.jpg", UriKind.Relative)))),
                               new ImageBrush((new BitmapImage(new Uri(path + "TonPic/9.jpg", UriKind.Relative)))),
                               new ImageBrush((new BitmapImage(new Uri(path + "TonPic/4.jpg", UriKind.Relative)))),
                               new ImageBrush((new BitmapImage(new Uri(path + "TonPic/5.jpg", UriKind.Relative)))),
                               new ImageBrush((new BitmapImage(new Uri(path + "TonPic/6.jpg", UriKind.Relative)))),
                               new ImageBrush((new BitmapImage(new Uri(path + "TonPic/1.jpg", UriKind.Relative)))),
                               new ImageBrush((new BitmapImage(new Uri(path + "TonPic/2.jpg", UriKind.Relative)))),
                               new ImageBrush((new BitmapImage(new Uri(path + "TonPic/3.jpg", UriKind.Relative)))),
                               // shooted squares
                               new SolidColorBrush(Colors.Black), // 24
                               // single block
                               new SolidColorBrush(colorMap[3]), // 25
                               // tripple block
                               new SolidColorBrush(colorMap[1]),  // 26    
                               new SolidColorBrush(colorMap[1]),
                               new SolidColorBrush(colorMap[1]),
                               new SolidColorBrush(colorMap[1]),
                               new SolidColorBrush(colorMap[1]),
                               new SolidColorBrush(colorMap[1]),
                               new SolidColorBrush(colorMap[1]),
                               new SolidColorBrush(colorMap[1]),
                               new SolidColorBrush(colorMap[1])
                               // 35
            
        };

        // used to generate a new brush
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
            { 
                return (((ImageBrush)aBrush).Clone());
            }

        }
        
        // generate a random colors list
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
