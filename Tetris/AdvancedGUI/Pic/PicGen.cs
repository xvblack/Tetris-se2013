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
    /// used to generate a picture
    /// </summary>
    public class PicGen
    {
        public int xNum { set; get; } // x direction pixel num
        public int yNum { set; get; } // y direction pixel num

        protected int xSize { set; get; } // width
        protected int ySize { set; get; } // height

        protected Color[] colorMap;   // colors used to show the pic
        protected int colorNum { set; get; }  // how many colors used

        protected int[,] colorIndexMatrix;    // matrix holding the colors indexes of each pixel

        protected SolidColorBrush[,] picMatrix; // finaly pic

        public PicGen() 
        {
            this.setColorMap();
            this.setPicMatrix();
        }

        // set up the pic
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

        // set the color map
        virtual protected void setColorMap() {
            colorNum = 2;
            colorMap = new Color[colorNum];
            colorMap[0] = Colors.Red;
            colorMap[1] = Colors.Blue;
        }

        // return the pic generated
        public SolidColorBrush[,] getPicMatrix() { return (this.picMatrix); }
    }
}
