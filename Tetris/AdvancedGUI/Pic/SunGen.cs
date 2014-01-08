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
    /// a sun
    /// </summary>
    public class SunGen : PicGen
    {
        public SunGen()
            : base() 
        {
            colorIndexMatrix = new int[,]{
                {5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
                {5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,3,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
                {5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,3,3,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
                {5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,3,3,5,5,5,5,3,3,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
                {5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,3,3,5,5,5,5,5,5,5,5,5,5,3,5,5,5,5,5,5,5,5,5,5,5,5},
                {5,5,5,5,5,5,5,5,5,1,4,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,3,5,5,5,5,5,5,5,5,5,5,5,5},
                {5,5,5,5,5,5,5,5,5,1,1,4,5,5,5,5,5,5,3,3,3,3,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
                {5,5,5,5,5,5,5,5,5,1,5,5,5,5,3,3,3,3,3,3,3,3,3,3,3,5,5,5,5,5,4,1,5,5,5,5,5,5,5,5},
                {5,5,5,5,5,5,5,5,5,5,5,5,5,3,3,3,3,3,3,3,3,3,3,3,3,3,3,5,5,5,4,4,5,5,5,5,5,5,5,5},
                {5,5,5,5,5,5,3,5,5,5,5,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,5,5,5,5,5,5,5,5,5,5,5},
                {5,5,5,5,5,5,3,5,5,5,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,5,5,5,3,5,5,5,5,5,5},
                {5,5,5,5,5,5,5,5,5,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,5,5,3,5,5,5,5,5,5},
                {5,5,5,5,5,5,5,5,5,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,5,5,5,5,5,5,5,5,5},
                {5,5,5,4,4,5,5,5,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,5,5,5,5,5,5,5,5},
                {5,5,5,5,4,5,5,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,5,5,4,5,5,5,5,5},
                {5,5,5,5,5,5,5,3,3,3,3,2,2,3,3,3,3,3,3,3,3,3,3,3,3,3,2,2,3,3,3,3,3,5,5,4,4,5,5,5},
                {5,5,5,5,5,5,5,3,3,3,3,2,2,3,3,3,3,3,3,3,3,3,3,3,3,3,2,2,3,3,3,3,3,5,5,5,5,5,5,5},
                {5,5,5,5,5,5,3,3,3,4,4,4,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,5,5,5,5,5,5,5},
                {5,5,3,3,5,5,3,3,4,4,4,4,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,4,4,4,3,3,3,5,5,5,5,5,5},
                {5,5,5,5,5,5,3,3,3,4,4,4,3,3,3,3,2,2,2,2,3,3,3,3,3,3,3,3,4,4,4,3,3,3,5,3,5,5,5,5},
                {5,5,5,5,5,5,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,4,4,3,3,3,5,5,5,5,5,5},
                {5,5,5,3,3,5,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,5,3,5,5,5,5},
                {5,5,3,3,3,5,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,5,5,3,3,5,5,5},
                {5,5,5,5,5,5,5,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,5,5,5,5,5,5,5},
                {5,5,5,5,5,5,5,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,5,5,5,5,5,5,5},
                {5,5,5,5,5,5,5,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,5,3,3,5,5,5,5},
                {5,5,5,5,3,3,5,5,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,5,5,5,5,5,5,5,5},
                {5,5,5,5,5,5,5,5,5,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,5,5,3,5,5,5,5,5,5},
                {5,5,5,5,5,5,5,5,5,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,5,5,5,3,3,5,5,5,5,5},
                {5,5,5,5,5,5,5,5,5,5,5,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,5,5,5,3,3,3,5,5,5,5,5},
                {5,5,5,5,5,5,3,3,5,5,5,5,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,5,5,5,5,5,5,5,3,5,5,5,5},
                {5,5,5,5,5,5,3,5,5,5,5,5,5,5,3,3,3,3,3,3,3,3,3,3,3,5,5,5,5,5,3,5,5,5,5,5,5,5,5,5},
                {5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,3,3,3,3,3,3,3,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
                {5,5,5,5,5,5,5,5,5,5,3,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,4,5,5,5,5,5,5,5,5,5,5,5,5},
                {5,5,5,5,5,5,5,5,5,5,3,5,5,5,5,4,5,5,5,5,5,5,5,5,5,5,4,4,5,5,5,5,5,5,5,5,5,5,5,5},
                {5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,1,1,5,5,5,5,3,5,5,5,5,5,4,5,5,5,5,5,5,5,5,5,5,5,5},
                {5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,1,1,5,5,5,5,5,5,5,5,5,5,5,4,5,5,5,5,5,5,5,5,5,5,5},
                {5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,4,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
                {5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
                {5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5}};
            setPicMatrix();
        }

        protected override void setColorMap()
        {
            //base.setColorMap();
            colorNum = 5;
            colorMap = new Color[colorNum];
            colorMap[0] = Color.FromArgb(255, 242, 0, 58);   // red   
            colorMap[1] = Color.FromArgb(255, 108, 28, 3);   // brown
            colorMap[2] = Color.FromArgb(255, 246, 180, 2);    // orange
            colorMap[3] = Color.FromArgb(255, 248, 72, 23);    // pink
            colorMap[4] = Colors.Transparent;
        }
    }
}
