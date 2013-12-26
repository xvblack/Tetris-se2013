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
using System.Windows.Shapes;

namespace Tetris.AdvancedGUI.Pic
{
    /// <summary>
    /// TestPic.xaml 的交互逻辑
    /// </summary>
    public partial class TestPic : Window
    {
        PicGenGrid picGrid;
        PicGen pic;
        int pixelSize;

        public TestPic()
        {
            InitializeComponent();

            pixelSize = 5;

            pic = new Cat5Gen();
            picGrid = new PicGenGrid(pic, pixelSize);

            this.Content = picGrid;

            this.MouseLeftButtonDown += this.mouseOnWhichPixel;

            this.Show();
        }

        private void mouseOnWhichPixel(object sender, MouseEventArgs e)
        {
            Point pt = e.GetPosition(this);
            //double[] gridSize = getPicSize();
            //Console.WriteLine(gridSize[0]);
            //Console.WriteLine(gridSize[1]);

            int xIndex = (int)(pt.X / pixelSize) + 1;
            int yIndex = (int)(pt.Y / pixelSize) + 1;

            Console.Write("xIndex " + xIndex.ToString() + ", yIndex " + yIndex.ToString() + '\n');

        }

        private double[] getPicSize() 
        {
            double[] size = new double[2] { picGrid.ActualHeight, picGrid.ActualWidth };
            return size;           
        }
        
    }
}
