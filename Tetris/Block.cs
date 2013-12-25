using System.Diagnostics;

namespace Tetris.GameBase
{
    public partial class Block:ISprite // 下落的方块
    {
        private static int _id=0;
        public int Width // 宽度，包括旋转
        {
            get{return Style.GetUpperBound(1-Direction%2)+1;}
        }
        public int Height // 高度，包括旋转
        {
            get { return Style.GetUpperBound(Direction%2)+1; }
        }
        public Square SquareAt(int i,int j) // 获得以左下角为原点的i行j列的方块，包括旋转
        {
            Debug.Assert(Direction>=0&&Direction<=3);
            switch (Direction)
            {
                case 0:
                    return Style[i, j];
                case 1:
                    return Style[j, Style.GetUpperBound(1) - i];
                case 2:
                    return Style[Style.GetUpperBound(0) - i, Style.GetUpperBound(1) - j];
                case 3:
                    return Style[Style.GetUpperBound(0) - j, i];
            }
            return null;
        }

        public SquareArray Style; // 方块样式
        public int Id { get; private set; } // 方块Id
        public int LPos { get; set; } // 方块左下角所在行数
        public int RPos { get; set; } // 方块左下角所在列数
        protected int Direction { get; set; }  // 方块方向
        public int FallSpeed { get; set; } // 下落速度

        public const int TempId = -2; // 临时方块Id
        public Block(SquareArray style, int blockId=-1)
        {
            if (blockId==-1) // 如果没有指定Id，生成新Id
                Id = _id++;
            else
                Id = blockId; // 否则直接使用给定的Id
            Style = style;
        }

        public void Rotate() // 顺时针旋转
        {
            Direction = (Direction + 1)%4;
        }
        public Block Rotate(int x) //顺时针旋转指定次数
        {
            Direction = (Direction + x) % 4;
            return this;
        }
        public void CounterRotate() // 逆时针旋转
        {
            Direction = (Direction - 1) % 4;
        }

        public Block Clone() // 浅复制方块
        {
            return new Block(Style,blockId:TempId) { Direction = this.Direction, FallSpeed = this.FallSpeed, LPos = this.LPos, RPos = this.RPos };
        }

        public Block Fall() // 方块下落
        {
            LPos--;
            return this;
        }
    }
}