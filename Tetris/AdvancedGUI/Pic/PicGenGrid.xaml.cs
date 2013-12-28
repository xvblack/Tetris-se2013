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
using Tetris.AdvancedGUI;

namespace Tetris.AdvancedGUI.Pic
{
    /// <summary>
    /// PicGenGrid.xaml 的交互逻辑
    /// </summary>
    public partial class PicGenGrid : ContentGridGenerator
    {

        public PicGenGrid(PicGen pic, double size)
        {
            initializeGrid(pic.xNum, pic.yNum, size, 0);
            brushesMatrix = pic.getPicMatrix();
            setContent();
            
        }
        private void setContent()
        {
            for (int i = 0; i < gridHeight; i++)
            {
                for (int j = 0; j < gridWidth; j++)
                {
                    squaresMatrix[i, j].Fill = brushesMatrix[i, j];
                }
            }
        }
    }    
}
