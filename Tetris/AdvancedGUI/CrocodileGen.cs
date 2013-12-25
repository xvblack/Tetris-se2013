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

namespace Tetris.AdvancedGUI
{
    public class CrocodileGen: PicGen
    {
        public CrocodileGen()
            : base() { }

        protected override void setPicMatrix()
        {
            //base.setPicMatrix();
            int[,] colorIndexMatrix = new int[,]{
                {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,2,6,6,6,2,6,6,6,6,6,6,6,6,6,6,6},
                {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,2,6,6,2,2,2,6,2,2,2,6,2,2,6,6,6,6,6,6,6},
                {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,2,6,6,6,2,2,2,6,3,3,3,3,3,3,3,3,3,3,6,2,2,6,6,6,6},
                {6,6,6,6,6,6,6,6,6,6,6,3,3,6,6,3,3,3,2,2,2,6,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,2,6,6,6,6},
                {6,6,6,6,6,6,6,6,6,6,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,6,6,6,6},
                {6,6,6,6,6,6,6,6,6,6,3,3,5,3,3,3,5,3,3,3,3,3,3,3,3,3,3,1,1,3,3,3,1,1,3,3,3,3,3,3,3,2,6,6},
                {6,6,6,6,6,6,6,6,6,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,1,3,3,3,1,1,3,3,3,3,3,3,3,3,6,6},
                {6,6,6,6,3,6,6,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,6},
                {6,3,6,3,5,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,1,3,3,3,3,3,3,3,3,3,3,3,3,6,3,3,3,3,6},
                {3,5,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,6,3,3,3,1,6,6,3,3,3,6},
                {3,3,3,3,3,3,3,3,3,3,4,4,4,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,6,6,6,1,1,1,6,6,3,3,3,3},
                {3,3,3,3,3,3,3,3,4,4,4,4,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,6,6,3,3,3,6,6,6,6,6,6,6,6,3,3,3,3},
                {3,3,3,3,6,6,3,4,4,4,4,3,3,3,3,3,3,3,3,3,3,3,3,3,6,6,6,6,6,1,1,1,6,6,6,6,6,6,6,6,3,3,3,3},
                {6,6,6,6,6,4,4,4,4,3,3,3,3,3,3,3,3,3,3,3,3,3,3,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,3,3,3,6},
                {6,6,6,6,4,4,4,3,3,3,3,3,3,3,3,3,3,3,3,6,3,3,1,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,3,3,3,6},
                {6,6,3,4,4,4,3,3,3,3,3,3,3,3,3,3,3,3,3,6,1,1,1,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,3,3,3,6,6},
                {6,6,3,3,3,3,3,3,3,3,3,6,6,6,6,3,3,1,3,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,3,3,6,6,6},
                {6,6,6,6,3,3,3,6,6,6,6,6,6,6,6,1,1,1,1,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,3,3,6,6,6,6}};
            xNum = colorIndexMatrix.GetLength(0);
            yNum = colorIndexMatrix.GetLength(1);

            picMatrix = new SolidColorBrush[xNum, yNum];
            for (int i = 0; i < xNum; i++ )
            {
                for (int j = 0; j < yNum; j++)
                {
                    picMatrix[i, j] = 
                        new SolidColorBrush(colorMap[colorIndexMatrix[i, j]-1]);
                }
            }
        }

        protected override void setColorMap()
        {
            //base.setColorMap();
            colorNum = 6;
            colorMap = new Color[colorNum];
            colorMap[0] = Color.FromArgb(255, 240, 236, 40);   // yellow    
            colorMap[1] = Color.FromArgb(255, 240, 139, 31);   // orange
            colorMap[2] = Color.FromArgb(255, 145, 195, 61);   // light green
            colorMap[3] = Color.FromArgb(255, 88, 162, 49);    // dark green
            colorMap[4] = Color.FromArgb(255, 71, 98, 179);    // blue
            colorMap[5] = Colors.Transparent;
        }
    }
}