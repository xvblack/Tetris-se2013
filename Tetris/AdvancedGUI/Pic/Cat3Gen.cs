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

namespace Tetris.AdvancedGUI.Pic
{
    /// <summary>
    /// generate a cat
    /// </summary>
    /// 
    public class Cat3Gen : PicGen
    {
        public Cat3Gen() : base()          
        {
            colorIndexMatrix = new int[,]{
                {4,4,4,4,4,4,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,2,2,1,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,2,1,1,1,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,2,1,1,1,1,1,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,2,1,1,1,1,1,2,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,2,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,2,2,2,4,4,4,4,4,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,2,2,2,2,2,4,4,4},
                {4,4,4,2,2,2,4,4,4,4,4,4,4,4,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4},
                {4,4,2,2,2,4,4,3,3,3,3,3,3,4,4,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                {4,4,2,2,4,4,3,3,3,3,3,3,3,4,4,4,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,2,2},
                {4,2,2,4,4,3,3,3,3,3,3,3,3,3,4,4,2,2,2,2,4,4,4,4,4,4,2,2,2,1,1,1,1,2,2,4},
                {4,2,2,4,4,3,3,3,3,3,3,3,3,3,4,4,2,2,2,4,4,4,3,3,4,4,4,2,2,2,1,1,2,2,4,4},
                {2,2,2,4,4,3,3,3,3,3,3,3,3,3,4,4,2,2,4,4,3,3,3,3,3,3,4,4,2,2,1,1,2,4,4,4},
                {2,2,2,4,4,3,3,3,3,3,3,3,3,3,4,4,2,4,4,3,3,3,3,3,3,3,3,4,2,2,2,2,4,4,4,4},
                {2,2,2,4,4,3,3,3,3,3,3,3,3,3,4,4,2,4,3,3,3,3,3,3,3,3,3,4,2,2,2,4,4,4,4,4},
                {2,2,2,4,4,3,3,3,3,3,3,3,3,4,4,2,4,4,3,3,3,3,3,3,3,3,3,4,4,2,4,4,4,4,4,4},
                {2,2,2,4,4,4,3,3,3,3,3,3,3,4,2,2,4,4,3,3,3,3,3,3,3,3,3,4,4,2,4,4,4,4,4,4},
                {2,2,2,2,4,4,4,4,3,3,3,4,4,4,2,2,4,4,3,3,3,3,3,3,3,3,3,4,2,2,4,4,4,4,4,4},
                {4,2,2,2,2,4,4,4,4,4,4,4,2,2,2,2,4,4,3,3,3,3,3,3,3,3,3,4,2,2,4,4,4,4,4,4},
                {4,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,4,3,3,3,3,3,3,3,3,3,4,2,2,4,4,4,4,4,4},
                {4,4,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,4,3,3,3,3,3,3,3,4,4,2,2,4,4,4,4,4,4},
                {4,4,4,2,2,2,2,2,2,2,2,2,1,2,2,2,2,4,4,4,3,3,3,3,4,4,4,2,2,4,4,4,4,4,4,4},
                {4,4,4,4,2,2,2,2,2,2,2,2,1,2,2,2,2,2,4,4,4,4,4,4,4,4,2,2,2,4,4,4,4,4,4,4},
                {4,4,4,4,4,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,2,2,2,2,2,2,2,2,2,2,2,4,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,2,2,2,2,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,2,2,2,2,2,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,2,2,2,2,2,2,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,2,2,2,4,4,4,4,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,2,2,4,4,4,4,4,2,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,2,2,4,4,4,4,4,4,4,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,2,2,4,4,4,4,4,4,4,2,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,2,2,4,4,4,4,4,4,4,2,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,2,2,4,4,4,4,4,4,4,4,4,2,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,2,2,4,4,4,4,4,4,4,4,4,2,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,2,2,4,4,4,4,4,4,4,4,4,2,2,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,2,2,4,4,4,4,4,4,4,4,4,2,2,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,2,2,4,4},
                {4,2,2,4,4,4,4,4,4,4,4,4,2,2,2,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,2,2,2,2,4},
                {4,2,2,4,4,4,4,4,4,4,4,4,2,2,2,2,2,2,2,2,2,2,2,4,4,4,4,4,4,2,2,2,2,2,2,4},
                {4,2,2,4,4,4,4,4,4,4,4,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,4},
                {4,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,4,4,4},
                {4,4,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,4,4,4,4,4},
                {4,4,2,3,3,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
                {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4}};
            setPicMatrix();
        }

        protected override void setColorMap()
        {
            colorNum = 4;
            colorMap = new Color[colorNum];
            colorMap[0] = Color.FromArgb(255, 255, 218, 237); // pink
            colorMap[1] = Color.FromArgb(255, 251, 208, 129); // yellow
            colorMap[2] = Colors.Black;
            colorMap[3] = Colors.Transparent;
        }
    }
}
