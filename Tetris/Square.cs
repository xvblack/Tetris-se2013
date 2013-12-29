using System.Collections.Generic;
using System;

namespace Tetris.GameBase
{
    public class Square // 小方块
    {
        public Square(int color)
        {
            NewSquare = true;
            Color = color;
        }

        public int Color // 方块颜色
        {
            get; set;
        }

        public int SubId = 0; // 多方块子Id
        static public void MarkMulti(Block block)
        {
            for (var i = 0; i < block.Height; i++)
                for (var j = 0; j < block.Width; j++)
                    {
                        if (block.SquareAt(i, j) != null)
                            block.SquareAt(i, j).SubId = block.Width * i + j;
                    }
                }
        public bool NewSquare { get; private set; } // 是否是下落中或刚刚落下的方块

        public void Devoid() // 标记为旧方块
        {
            NewSquare = false;
        }

        private static SquareArray Style(int[,] style) // 根据传进的整数矩阵生成SquareArray类
        {
            var result = new SquareArray(style.GetUpperBound(0) + 1, style.GetUpperBound(1) + 1);
            for (int i = 0; i < style.GetUpperBound(0) + 1; i++)
                for (int j = 0; j < style.GetUpperBound(1) + 1; j++)
                    result[style.GetUpperBound(0) - i, j] = style[i, j] != 0 ? new Square(style[i, j]) : null;
            return result;
        }

        public static List<SquareArray> Styles(int[][,] styles) // 根据传进的整数矩阵数组生成SquareArray的List
        {
            var result = new SquareArray[styles.Length];
            for (int i = 0; i < styles.Length; i++)
            {
                result[i] = Style(styles[i]);
            }
            return new List<SquareArray>(result);
        }

        public virtual Square Clone() // 复制Square
        {
            return new Square(Color);
        }

        public static SquareArray BuildStyle(int m, int n, int color) // 生成m*n的方型SquareArray
        {
            int[,] result=new int[m,n];
            for (var i=0;i<m;i++)
                for (var j = 0; j < n; j++)
                    result[i, j] = color;
            return Style(result);
        }
}


    public class SquareArray // Square矩阵，以左下角为原点
    {
        private int _m = 0;
        private int _n = 0;

        public Square[,] Storage // 内部存储数组
        {
            get; private set;
        }

        public SquareArray(int m, int n) // 生成m*n大小的空的SquareArray
        {
            Storage=new Square[m,n];
            _m = m - 1;
            _n = n - 1;
        }

        public SquareArray(Square[,] _squares) // 生成以传入数组为Storage的SquareArray
        {
            Storage = _squares;
            _m = Storage.GetUpperBound(0);
            _n = Storage.GetUpperBound(1);
        }

        public SquareArray Clone() // 浅复制SquareArray
        {
            return new SquareArray(Storage.Clone() as Square[,]);
        }

        public int GetUpperBound(int i) // 实现与Square[,]相同的获得一个方向上最大index的函数
        {
            //return Storage.GetUpperBound(i);
            if (i == 0)
                return _m;
            return _n;
        }

        public Square this[int i, int j] // 获得以左下角为原点的i行j列的方块
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
