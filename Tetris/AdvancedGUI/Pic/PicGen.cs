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
    public class PicGen
    {
        public int xNum { set; get; }
        public int yNum { set; get; }

        protected int xSize { set; get; }
        protected int ySize { set; get; }

        protected Color[] colorMap;
        protected int colorNum { set; get; }

        protected int[,] colorIndexMatrix;

        protected SolidColorBrush[,] picMatrix;

        public PicGen() 
        {
            this.setColorMap();
            this.setPicMatrix();
        }

        protected void setPicMatrix() {

            if (colorIndexMatrix != null) 
            {
                xNum = colorIndexMatrix.GetLength(0);
                yNum = colorIndexMatrix.GetLength(1);

                picMatrix = new SolidColorBrush[xNum, yNum];
                for (int i = 0; i < xNum; i++)
                {
                    for (int j = 0; j < yNum; j++)
                    {
                        picMatrix[i, j] = 
                            new SolidColorBrush(colorMap[colorIndexMatrix[i, j] - 1]);
                    }
                }
            }
        }
        virtual protected void setColorMap() {
            colorNum = 2;
            colorMap = new Color[colorNum];
            colorMap[0] = Colors.Red;
            colorMap[1] = Colors.Blue;
        }

        public SolidColorBrush[,] getPicMatrix() { return (this.picMatrix); }
    }
}
