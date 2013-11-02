using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Tetris
{
    public class Square
    {
        int _color;

        private Square(int color)
        {
            _color = color;
        }

        public int Color
        {
            get
            {
                return _color;
            }
        }

        private static Square[,] Style(int[,] style)
        {
            var result=new Square[style.GetUpperBound(0)+1,style.GetUpperBound(1)+1];
            for (int i = 0; i < style.GetUpperBound(0)+1; i++)
                for (int j = 0; j < style.GetUpperBound(1)+1; j++)
                    result[i, j] = new Square(style[i, j]);
            return result;
        }

        public static Square[][,] Styles(int[][,] styles)
        {
            var result = new Square[styles.Length][,];
            for (int i = 0; i < styles.Length; i++)
            {
                result[i] = Style(styles[i]);
            }
            return result;
        }
    }
}
