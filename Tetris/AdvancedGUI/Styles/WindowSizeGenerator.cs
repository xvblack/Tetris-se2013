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
    static class WindowSizeGenerator
    {
        static readonly public int mainWindowMinWidth = 800;
        static readonly public int mainWindowMinHeight = 700;
        static readonly public int dualModePageWidth = 1400;
        static readonly public int singleModePageWidth = 1000;
        static readonly public String windowSize = "Maximum";
        //static readonly public String windowSize = "";

        static readonly public double screenSize = 
            System.Windows.SystemParameters.FullPrimaryScreenWidth;

        

        static readonly public double mainWindowLocationLeft = 
            (screenSize - mainWindowMinWidth) / 2;
        static readonly public double dualModePageLocationLeft = 
            (screenSize - dualModePageWidth) / 2;
        static readonly public double singleModePageLocationLeft =
            (screenSize - singleModePageWidth) / 2;
    }
}
