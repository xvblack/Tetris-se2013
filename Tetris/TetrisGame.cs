using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Tetris
{
    public partial class TetrisGame
    {

        public enum GameAction { Rotate, Left, Right, Down };

        #region Event Args and Callback Declare
        public class ClearBarEventArgs
        {
            public int Line { get; private set; }
            public Square[] Squares { get; private set; }
            public int Tick { get; private set; }

            public ClearBarEventArgs(Square[,] underlying, int line, int tick)
            {
                Line = line;
                Tick = tick;
                Squares = new Square[underlying.GetUpperBound(1) + 1];
                for (var j = 0; j < underlying.GetUpperBound(1) + 1; j++)
                {
                    Squares[j] = underlying[line, j];
                }
            }
        }
        public class UpdateEndEventArgs
        {
            public int Tick { get; private set; }

            public UpdateEndEventArgs(int tick)
            {
                Tick = tick;
            }
        }
        public class DrawEventArgs
        {
            public int Tick { get; private set; }

            public DrawEventArgs(int tick)
            {
                Tick = tick;
            }
        }

        public delegate void ClearBarCallback(object sender, ClearBarEventArgs e);
        public delegate void UpdateEndCallback(object sender, UpdateEndEventArgs e);
        public delegate void DrawCallback(TetrisGame sender, DrawEventArgs e);
        #endregion

        private delegate void UpdateCallback();

        private readonly Square[,] _underLying;
        private Block _block;
        private int _tick;
        private readonly int _w, _h;
        private const int Round = 6;
        private readonly int _gameSpeed;
        private int _state;         // 0 for game ending, 1 for looping, 2 for pause
        private readonly object _updating=new object();

        #region Reference to External Objects
        private readonly IEngine _engine;
        private readonly TetrisFactory _factory;
        private IController _controller;
        public ScoreSystem ScoreSystem { get; private set; }
        #endregion

        #region Events
        public event ClearBarCallback ClearBarEvent;
        public event UpdateEndCallback UpdateEndEvent;
        public event DrawCallback DrawEvent;
        #endregion

        public TetrisGame(IEnumerable<Square[,]> styles,IEngine engine,int w=10,int h=15, int gameSpeed=1)
        {
            _w = w;
            _h = h;
            _gameSpeed = gameSpeed;
            _engine = engine;
            _engine.TickEvent += UpdateDispatch;
            _underLying = new Square[_h, _w];
            _factory=new TetrisFactory(this,styles);

            #region Intializeing ScoreSystem
            ScoreSystem =new ScoreSystem();
            ClearBarEvent += ScoreSystem.OnClearBar;
            UpdateEndEvent += ScoreSystem.OnUpdateEnd;
            #endregion

            _tick = 0;
            _state = 0;
        }

        public void Start()
        {
            _engine.Enabled = true;
            if (_block==null) GenTetris();
            _state = 1;
        }
        public void Pause()
        {
            Debug.Assert(_state==1,"Can Only Pause a Ongoing Game");
            _state = 2;
        }
        public void Continue()
        {
            _state = 1;
        }
        public void End()
        {
            Trace.WriteLine("ending");
            _state = 0;
            _engine.Enabled = false;
            Console.Out.Write("end");
        }

        public void AddDisplay(IDisplay display)
        {
            DrawEvent += display.OnDrawing;
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
        public Square[,] UnderLying
        {
            get
            {
                var result = new Square[_h, _w];
                Array.Copy(_underLying, result, _h * _w);
                return result;
            }
        }
        public Block block
        {
            get
            {
                return _block;
            }
        }

        // Get Width
        public int w
        {
            get
            {
                return _w;
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

        private void UpdateDispatch(object sender,int tick)
        {
            lock (_updating)
            {
                if (_block == null) GenTetris();
                if (_state == 0) return;    // Check ending caused by cannot generate new block
                if (_state == 2) return;    // Check for pause
                _tick++;                    // internal tick add
                Debug.Assert(_block != null, "loop continue when _block is null");
                _block.FallSpeed = FallingSpeed;    // Use the Game FallingSpeed as the block fall speed
                PerRound(3, HandleAction);
                PerRound(1*_block.FallSpeed, delegate()
                {
                    HandleFalling();
                    ClearBar();
                });
                UpdateEndEvent.Invoke(this,new UpdateEndEventArgs(_tick));
                DrawEvent.Invoke(this,new DrawEventArgs(_tick));
            }
        }

        private void ClearBar()
        {
            for (var i = 0; i < _h; i++)
            {
                bool clear = true;
                for (int j = 0; j < _w; j++)
                {
                    if (_underLying[i, j] == null) clear = false;
                }
                if (clear)
                {
                    ClearBarEvent.Invoke(this,new ClearBarEventArgs(_underLying,i,_tick));
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
