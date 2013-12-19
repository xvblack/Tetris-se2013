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

namespace Tetris.AdvancedGUI
{
    public class PicGen
    {
        public int xNum { set; get; }
        public int yNum { set; get; }

        protected int xSize { set; get; }
        protected int ySize { set; get; }

        protected Color[] colorMap;
        protected int colorNum { set; get; }

        protected SolidColorBrush[,] picMatrix;

        public PicGen() 
        {
            this.setColorMap();
            this.setPicMatrix();
        }

        virtual protected void setPicMatrix() { }
        virtual protected void setColorMap() {
            colorNum = 2;
            colorMap = new Color[colorNum];
            colorMap[0] = Colors.Red;
            colorMap[1] = Colors.Blue;
        }

        public SolidColorBrush[,] getPicMatrix() { return (this.picMatrix); }
    }
}
