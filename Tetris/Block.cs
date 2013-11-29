using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Tetris.GameBase
{
    public class Block:CSprite
    {
        private static int _id=0;
        public int Width
        {
            get{return _style.GetUpperBound(1-_direction%2)+1;}
        }
        public int Height
        {
            get { return _style.GetUpperBound(_direction%2)+1; }
        }
        public Square SquareAt(int i,int j)
        {
            switch (_direction)
            {
                case 0:
                    return _style[i, j];
                case 1:
                    return _style[j, _style.GetUpperBound(1) - i];
                case 2:
                    return _style[_style.GetUpperBound(0) - i, _style.GetUpperBound(1) - j];
                case 3:
                    return _style[_style.GetUpperBound(0) - j, i];
            }
            return null;
        }

        private readonly SquareArray _style;
        public int Id { get; private set; }
        private float _l;
        private float _r;
        private float _vl;
        private int _direction;
        public int LPos { get { return (int)_l; } set{ _l=value; } }
        public int RPos { get { return (int)_r; } set{ _r = value; } }
        public int FallSpeed { get { return (int)_vl;} set{ _vl=value; } }

        // More universal constructor by Hengkai Guo
        public const int TempId = -2;
        public Block(SquareArray style, int blockId=-1)
        {
            if (blockId==-1)
                Id = _id++;
            else
                Id = blockId;
            _style = style;
        }

        public void Rotate()
        {
            _direction = (_direction + 1)%4;
        }
        public Block Rotate(int x)
        {
            _direction = (_direction + x) % 4;
            return this;
        }
        public void CounterRotate()
        {
            _direction = (_direction - 1) % 4;
        }

        public Block Clone()
        {
            return new Block(_style,blockId:TempId) { _direction = this._direction, _vl = this._vl, _l = this._l, _r = this._r };
        }

        public Block Fall()
        {
            LPos--;
            return this;
        }
    }

    public class TetrisFactory
    {
        readonly List<SquareArray> _styles;
        readonly Random _random;
        public TetrisGame Game;
        public TetrisFactory(IEnumerable<SquareArray> styles){
            _styles = new List<SquareArray>(styles);
            _random=new Random();
        }
        public Block GenTetris(){
            int rr;
            int type = _random.Next(0, _styles.Count());
            var temp = _styles[type];
            rr = Game.Width / 2 - 1;
            //Trace.WriteLine(String.Format("pos = {0}, style = {1}", rr, type));
            var t = new Block(temp){RPos = rr};
            return t;
        }
    }
}