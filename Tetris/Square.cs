using System.Collections.Generic;
using System;

namespace Tetris.GameBase
{
    public class Square
    {
        private Square(int color)
        {
            Color = color;
        }

        public int Color
        {
            get; set;
        }

        private static SquareArray Style(int[,] style)
        {
            var result=new SquareArray(style.GetUpperBound(0)+1,style.GetUpperBound(1)+1);
            for (int i = 0; i < style.GetUpperBound(0)+1; i++)
                for (int j = 0; j < style.GetUpperBound(1) + 1; j++)
                    result[style.GetUpperBound(0)-i, j] = style[i, j] != 0 ? new Square(style[i, j]) : null;
            return result;
        }

        public static IEnumerable<SquareArray> Styles(int[][,] styles)
        {
            var result = new SquareArray[styles.Length];
            for (int i = 0; i < styles.Length; i++)
            {
                result[i] = Style(styles[i]);
            }
            return result;
        }
    }


    public class SquareArray
    {
        private int _m = 0;
        private int _n = 0;

        public Square[,] Storage
        {
            get; private set;
        }

        public SquareArray(int m, int n)
        {
            Storage=new Square[m,n];
            _m = m - 1;
            _n = n - 1;
        }

        public SquareArray(Square[,] _squares)
        {
            Storage = _squares;
            _m = Storage.GetUpperBound(0);
            _n = Storage.GetUpperBound(1);
        }

        public int GetUpperBound(int i)
        {
            //return Storage.GetUpperBound(i);
            if (i == 0)
                return _m;
            return _n;
        }

        public Square this[int i, int j]
        {
            get
            {
               if (i > _m)
               {
                   return null;
               }
               return Storage[_m - i, j];                
            }
            set { if (i <= _m) Storage[_m - i, j] = value; }
        }
    }
}
