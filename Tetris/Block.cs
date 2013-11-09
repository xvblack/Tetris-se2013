using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

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

        // More universal constructor by Hengkai Guo
        public Block(Square[,] style, float l = 0, float r = 0, float vl = 0, int direction = 0)
        {
            _style = style;
            _l = l;
            _r = r;
            _vl = vl;
            _direction = direction;

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
        private readonly TetrisGame _game;
        public TetrisFactory(TetrisGame game, IEnumerable<Square[,]> styles){
            _styles = new List<Square[,]>(styles);
            _random=new Random();
            _game = game;
        }
        public Block GenTetris(){
            float rr;
            int dir = 0;
            int type = _random.Next(0, _styles.Count());
            var temp = _styles[type];
            rr = _game.w / 2 - 1;
            Trace.WriteLine(String.Format("pos = {0}, style = {1}", rr, type));
            var t = new Block(temp, r: rr);
            Trace.WriteLine(String.Format("width = {0}", t.Width));
            return t;
        }
    }
}