using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Block:CSprite
    {
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
                    return _style[_style.GetUpperBound(0) - j, i];
                case 2:
                    return _style[_style.GetUpperBound(0) - i, _style.GetUpperBound(1) - j];
                case 3:
                    return _style[j, _style.GetUpperBound(1) - i];
            }
            return null;
        }

        public int LPosAt(int i, int j)
        {
            return LPos + i;
        }

        public int RPosAt(int i, int j)
        {
            return RPos + j;
        }
        private readonly Square[,] _style;
        private float _l, _r;
        private float _vl;
        private int _direction;
        public int LPos { get { return (int)_l; } set{ _l=value; } }
        public int RPos { get { return (int)_r; } set{ _r = value; } }
        public int FallSpeed { get { return (int)_vl;} set{ _vl=value; } }

        public Block(Square[,] style)
        {
            _style = style;
            _l = _r = _vl = 0;
            _direction = 0;
        }

        public void Rotate()
        {
            _direction = (_direction + 1)%4;
        }
        public void CounterRotate()
        {
            _direction = (_direction - 1) % 4;
        }

        public Block Clone()
        {
            return new Block(_style) { _direction = this._direction, _vl = this._vl, _l = this._l, _r = this._r };
        }

        public Block Fall()
        {
            LPos++;
            return this;
        }

    }
    class TetrisFactory
    {
        readonly List<Square[,]> _styles;
        readonly Random _random;
        public TetrisFactory(IEnumerable<Square[,]> styles){
            _styles = new List<Square[,]>(styles);
            _random=new Random();
        }
        public Block GenTetris(){
            var t = new Block(_styles[_random.Next(0, _styles.Count())]);
            return t;
        }
    }
}