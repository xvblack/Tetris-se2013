using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media;

namespace Tetris
{
    public class TetrisGame
    {
        public enum GameAction { Rotate, Left, Right, Down };

        private IEngine _engine;
        private readonly Square[,] _underLying;
        private readonly List<IDisplay> _displays;
        private Block _block;
        private readonly TetrisFactory _factory;
        private IController _controller;
        private int _tick;
        private readonly int _w, _h;
        private const int Round = 6;
        private readonly int _gameSpeed;
        private int _state;

        private delegate void UpdateCallback();

        public TetrisGame(IEnumerable<Square[,]> styles,IEngine engine,int w=10,int h=15, int gameSpeed=1)
        {
            _w = w;
            _h = h;
            _gameSpeed = gameSpeed;
            _engine = engine;
            _engine.TickEvent += new TickHandler(Update);
            _underLying = new Square[_h, _w];
            _displays=new List<IDisplay>();
            // TODO
            _factory=new TetrisFactory(styles);
            _tick = 0;
            _state = 0;
            _engine.Enabled = true;
        }

        public void Start()
        {
            if (_block==null) GenTetris();
            _state = 1;
        }
        public void End()
        {
            Trace.WriteLine(String.Format("Fail"));
            _state = 0;
            Console.Out.Write("end");
        }

        public void AddDisplay(IDisplay display)
        {
            _displays.Add(display);
            display.SetGame(this);
        }
        public void SetController(IController controller)
        {
            _controller = controller;
        }
        public Square[,] Image
        {
            get
            {
                var result = new Square[_h, _w];
                Array.Copy(_underLying, result, _h * _w);
                if (_block!=null)
                    for (int i=0;i<_block.Height;i++)
                        for (int j = 0; j < _block.Width; j++)
                        {
                            if (_block.SquareAt(i, j)!=null) result[_block.LPos + i, _block.RPos + j] = _block.SquareAt(i, j);
                        }
                return result;
            }
        }


        public bool Try(Block block)
        {
            var result = true;
            var tempBlock = _block;
            _block = block;
            if (Intersect()) result = false;
            _block = tempBlock;
            return result;
        }
        private void PerRound(int times, UpdateCallback cb)
        {
            if (_tick%(Round/times) == 0)
            {
                cb();
            }
        }

        private bool Valid(int i, int j )
        {
            return (i >= 0) && (i < _h) && (j >= 0) && (j < _w);
        }
        private bool Intersect()
        {
            for (int i = 0; i < _block.Height; i++)
                for (int j = 0; j < _block.Width; j++)
                {
                    if ((_block.SquareAt(i, j) != null) && (Valid(_block.LPos + i, _block.RPos + j)) && (_underLying[_block.LPos + i, _block.RPos + j] != null))
                    {
                        return true;
                    }
                    if ((_block.SquareAt(i, j) != null) && (!Valid(_block.LPos + i, _block.RPos + j)))
                        return true;
                }
            return false;
        }

        private void Update(object sender,int tick)
        {
            if (_state == 0) return;
            if (_block == null) GenTetris();
            _tick++;
            //Trace.WriteLine(_tick);
            _block.FallSpeed = FallingSpeed;
            PerRound(3,HandleAction);
            PerRound(1 * _block.FallSpeed, delegate()
            {
                HandleFalling();
                ClearBar();
            });
            
            foreach (var display in _displays)
            {
                display.OnStateChange();
            }
        }

        private void ClearBar()
        {
            for (int i = 0; i < _h; i++)
            {
                bool clear = true;
                for (int j = 0; j < _w; j++)
                {
                    if (_underLying[i, j] == null) clear = false;
                }
                if (clear)
                {
                    for(var s=i-1;s>0;s--)
                    for (var j = 0; j < _w; j++)
                    {
                        _underLying[s+1, j] = _underLying[s,j];
                    }
                    for (int j = 0; j < _w; j++)
                    {
                        _underLying[0, j] = null;
                    }
                }
            }
        }
        private int FallingSpeed
        {
            get
            {
                if (_controller.Act(GameAction.Down))
                {
                    return 2*_gameSpeed;
                }
                return 1*_gameSpeed;
            }
        }
        private void GenTetris()
        {
            if (_block!=null) throw new Exception("multiple sprite generating");
            var b = _factory.GenTetris();
            if (Try(b))
            {
                _block = b;
                Trace.WriteLine(String.Format("OK"));
            }
            else
            {
                End();
            }
        }

        private void HandleAction()
        {
            if (_controller.Act(GameAction.Rotate))
            {
                _block.Rotate();
                if (Intersect())
                {
                    _block.CounterRotate();
                }
            }

            if (_controller.Act(GameAction.Left))
            {
                _block.RPos--;
                if (Intersect())
                {
                    _block.RPos++;
                }
            }
            if (_controller.Act(GameAction.Right))
            {
                _block.RPos++;
                if (Intersect())
                {
                    _block.RPos--;
                }
            }
        }
        private void HandleFalling()
        {
            if (Try(_block.Clone().Fall()))
            {
                _block.Fall();
                Trace.WriteLine(String.Format("Block pos = ({0},{1})", _block.LPos, _block.RPos));
            }
            else
            {
                AddToUnderlying();
            }
        }
        private void AddToUnderlying()
        {
            for (int i = 0; i < _block.Height; i++)
                for(int j=0;j<_block.Width;j++)
                {
                    if (_block.SquareAt(i, j) != null)
                    {
                        _underLying[_block.LPos + i,_block.RPos + j] = _block.SquareAt(i, j);
                    }
                }
            _block = null;
        }
    }
}
