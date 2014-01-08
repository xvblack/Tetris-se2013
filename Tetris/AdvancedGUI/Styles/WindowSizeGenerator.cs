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
    /// <summary>
    /// use to handle differnet 分辨率 resolution
    /// </summary>
    class WindowSizeGenerator
    {
        static readonly public double screenWidth = 
            System.Windows.SystemParameters.PrimaryScreenWidth;
        static readonly public double screenHeight = 
            System.Windows.SystemParameters.PrimaryScreenHeight;

        static readonly public int gameWidth = 10;
        static readonly public int gameHeight = 17;

        // for game height
        static private double gameModuleTopBottomRatio = 0.05;
        static readonly public double gameModuleHeight = 
            (1 - gameModuleTopBottomRatio) * screenHeight;
        static readonly public double gameModuleTop =
            gameModuleTopBottomRatio * screenHeight / 2;

        // for dual game width
        static private double dualAdditionModuleRatio = 0.35;
        static private double dualGameModuleLeftRightRatio = 0.05;
        static readonly public double dualAdditionModuleWidth =
            (dualAdditionModuleRatio) * screenWidth;
        static readonly public double dualGameModuleWidth =
            (1 - dualAdditionModuleRatio - dualGameModuleLeftRightRatio) * screenWidth / 2;
        static readonly public double dualGameModuleRight =
            dualGameModuleLeftRightRatio * screenWidth / 2;

        // for single game height    
        // use dual game size to guide single game size

        // score board size 
        static private double scoreBoardWidthRatio = 0.40;
        static private double scoreBoardLeftRatio = (1 - 2 * scoreBoardWidthRatio) / 3;
        static readonly public double scoreBoardWidth = dualAdditionModuleWidth 
            * scoreBoardWidthRatio;
        static readonly public double scoreBoardLeft = scoreBoardLeftRatio
            * dualAdditionModuleWidth;
        static readonly public double scoreBoardHeight = scoreBoardWidth * 0.7;

        // nextBlock board size
        static readonly public double nextBoardWidth = scoreBoardWidth;
        static readonly public double nextBoardHeight = scoreBoardWidth * 1.1;
        static readonly public double nextBoardLeft = scoreBoardLeft; 

        // additional font size
        static readonly public double fontSizeLarge = screenWidth / 30;
        static readonly public double fontSizeSmall = screenWidth / 80;
        static readonly public double fontSizeMedium = screenWidth / 50;
    }
}
