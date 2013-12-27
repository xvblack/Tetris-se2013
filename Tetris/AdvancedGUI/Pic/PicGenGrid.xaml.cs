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

namespace Tetris.AdvancedGUI.Pic
{
    /// <summary>
    /// PicGenGrid.xaml 的交互逻辑
    /// </summary>
    public partial class PicGenGrid : Grid
    {
        private int rowNum;
        private int colNum;

        public PicGenGrid(PicGen pic, double size)
        {
            InitializeComponent();
            SolidColorBrush[,] picColorMatrix = pic.getPicMatrix();

            rowNum = pic.xNum;
            colNum = pic.yNum;

            int i = 0;
            int j = 0;

            for (i = 0; i < rowNum; i++) {
                RowDefinition aRow = new RowDefinition();
                aRow.Height = new GridLength(50, GridUnitType.Auto);
                this.RowDefinitions.Add(aRow);
            }

            for (j = 0; j < colNum; j++) {
                ColumnDefinition aCol = new ColumnDefinition();
                aCol.Width = new GridLength(50, GridUnitType.Auto);
                this.ColumnDefinitions.Add(aCol);
            }

            Rectangle[,] rect = new Rectangle[rowNum, colNum];

            for (i = 0; i < rowNum; i++) {
                for (j = 0; j < colNum; j++) {
                    rect[i, j] = new Rectangle();
                    rect[i, j].Fill = picColorMatrix[i, j];
                    rect[i, j].SetValue(Grid.RowProperty, i);
                    rect[i, j].SetValue(Grid.ColumnProperty, j);
                    rect[i, j].Height = size;
                    rect[i, j].Width = size;
                    this.Children.Add(rect[i, j]);
                }
            }
        }

        public int[] getPicSize()
        { 
            return(new int[2] {rowNum, colNum});
        }
    }
}
