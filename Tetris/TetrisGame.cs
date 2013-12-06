using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Tetris.GameSystem;

namespace Tetris.GameBase
{
    public partial class TetrisGame
    {

        public enum GameAction { Rotate, Left, Right, Down };

        #region Event Args and Callback Declare

        public class TickEventArgs
        {
            public int Tick { get; protected set; }

            public TickEventArgs(int tick)
            {
                Tick = tick;
            }
        }

        public class UpdateBeginEventArgs : TickEventArgs
        {
            public UpdateBeginEventArgs(int tick) : base(tick)
            {
            }
        }
        public class ClearBarEventArgs : TickEventArgs
        {
            public int Line { get; private set; }
            public Square[] Squares { get; private set; }

            public ClearBarEventArgs(SquareArray underlying, int line, int tick) : base(tick)
            {
                Line = line;
                Squares = new Square[underlying.GetUpperBound(1) + 1];
                for (var j = 0; j < underlying.GetUpperBound(1) + 1; j++)
                {
                    Squares[j] = underlying[line, j];
                }
            }
        }
        public class UpdateEndEventArgs : TickEventArgs
        {

            public UpdateEndEventArgs(int tick) : base(tick)
            {
            }
        }
        public class DrawEventArgs : TickEventArgs
        {

            public DrawEventArgs(int tick):base(tick)
            {
            }
        }

        public class GameEndEventArgs
        {
            public int Score { get; private set; }

            public GameEndEventArgs(int score)
            {
                Score = score;
            }
        }

        public class AddToUnderlyingEventArgs : TickEventArgs
        {
            public Block block { get; private set; }
            public AddToUnderlyingEventArgs(int tick, Block addingBlock):base(tick)
            {
                block = addingBlock;
            }
        }

        public delegate void UpdateBeginCallback(TetrisGame game, UpdateBeginEventArgs e);
        public delegate void ClearBarCallback(TetrisGame sender, ClearBarEventArgs e);
        public delegate void UpdateEndCallback(TetrisGame game, UpdateEndEventArgs e);
        public delegate void DrawCallback(TetrisGame sender, DrawEventArgs e);
        public delegate void GameEndCallback(object sender, GameEndEventArgs e);
        public delegate void AddToUnderlyingCallback(TetrisGame game, AddToUnderlyingEventArgs e);

        #endregion

        private delegate void UpdateCallback();
        
        public int Id { get; private set; }
        private readonly SquareArray _underLying;
        public Block Block { get; private set; }
        private int _tick;
        private readonly int _w, _h;
        private const int RoundTicks = 24;   // round tick numbers
        private readonly int _gameSpeed;
        private volatile int _state;         // 0 for game ending, 1 for looping, 2 for pause
        private Stack<Square> _newSquares = new Stack<Square>();

        public TetrisFactory Factory{get { return _factory; }}
        public SquareArray UnderLying
        {
            get { return _underLying; }
        }

        public int Height
        {
            get { return _h; }
        }

        public int Width
        {
            get { return _w; }
        }

        #region Reference to External Objects
        private readonly TetrisFactory _factory;
        private IController _controller;
        #endregion

        #region Events
        public event UpdateBeginCallback UpdateBeginEvent;
        public event ClearBarCallback ClearBarEvent;
        public event ClearBarCallback BeforeClearBarEvent;
        public event UpdateEndCallback UpdateEndEvent;
        public event DrawCallback DrawEvent;
        public event GameEndCallback GameEndEvent;
        public event AddToUnderlyingCallback AddToUnderlyingEvent;
        #endregion

        public TetrisGame(int id, IEnumerable<SquareArray> styles, IEngine engine, TetrisFactory factory, int w, int h, int gameSpeed)
        {
            _w = w;
            _h = h;
            _gameSpeed = gameSpeed;
            engine.TickEvent += UpdateDispatch;
            _underLying = new SquareArray(h,w);
            _factory=factory;
            _factory.Game = this;
            InitTickAPI();
            Id = id;
            _tick = 0;
            _state = 0;
        }

        public void Start()
        {
            if (Block==null) GenTetris();
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
            _state = 0;
            GameEndEvent.Invoke(this,new GameEndEventArgs(this.ScoreSystem.Score));
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
                var result = new SquareArray(_h,_w);
                Array.Copy(_underLying.Storage, result.Storage, _h * _w);
                if (Block!=null)
                    for (var i=0;i<Block.Height;i++)
                        for (var j = 0; j < Block.Width; j++)
                        {
                            if (Block.SquareAt(i, j) != null && Valid(Block.LPos + i, Block.RPos + j)) result[Block.LPos + i, Block.RPos + j] = Block.SquareAt(i, j);
                        }
                return result.Storage;
            }
        }

        public bool Try(Block block)
        {
            var result = true;
            var tempBlock = Block;
            Block = block;
            if (Intersect()) result = false;
            Block = tempBlock;
            return result;
        }
        private void PerRound(int times, UpdateCallback cb)
        {
            if (_tick%(RoundTicks/times) == 0)
            {
                cb();
            }
        }

        private bool Valid(int i, int j ,bool UpperBound=true)
        {
            if (UpperBound)
            {
                return (i >= 0) && (i < _h) && (j >= 0) && (j < _w);
            }
            else
            {
                return (i >= 0) && (j >= 0) && (j < _w);
            }
        }
        private bool Intersect()
        {
            for (int i = 0; i < Block.Height; i++)
                for (int j = 0; j < Block.Width; j++)
                {
                    if ((Block.SquareAt(i, j) != null) && (Valid(Block.LPos + i, Block.RPos + j)) && (_underLying[Block.LPos + i, Block.RPos + j] != null))
                    {
                        return true;
                    }
                    if ((Block.SquareAt(i, j) != null) && (!Valid(Block.LPos + i, Block.RPos + j,false)))
                        return true;
                }
            return false;
        }

        public void ClearBlock()
        {
            Block = null;
        }

        private void UpdateDispatch(object sender,int tick)
        {
            DrawEvent.Invoke(this, new DrawEventArgs(_tick));
            lock (this)
            {
                if (Block == null) GenTetris();
                if (_state == 0) return;
                if (_state == 2) return;
                if (Block == null) GenTetris();
                if (_state == 0) 
                    return;    // Check ending caused by cannot generate new block
                if (_state == 2) return;    // Check for pause
                foreach (Square newSquare in _newSquares)
                {
                    newSquare.Devoid();
                }
                _newSquares.Clear();
                _tick++;                    // internal tick add
                UpdateBeginEvent.Invoke(this, new UpdateBeginEventArgs(_tick));
                Debug.Assert(Block != null, "loop continue when Block is null");
                Debug.Assert(Block.Id!=Block.TempId,"loop using a temp block");
                Block.FallSpeed = FallingSpeed;    // Use the Game FallingSpeed as the block fall speed
                PerRound(8, HandleAction);
                PerRound(1*Block.FallSpeed, HandleFalling);
                ClearBar();
                UpdateEndEvent.Invoke(this,new UpdateEndEventArgs(_tick));
                if (_state == 0) return;
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
                    BeforeClearBarEvent.Invoke(this, new ClearBarEventArgs(_underLying,i,_tick));
                    for(var s=i;s<_h-1;s++)
                        for (var j = 0; j < _w; j++)
                        {
                            _underLying[s, j] = _underLying[s+1,j];
                        }
                    for (int j = 0; j < _w; j++)
                    {
                        _underLying[_h-1, j] = null;
                    }
                    ClearBarEvent.Invoke(this, new ClearBarEventArgs(_underLying, i, _tick));
                    i--;
                }
            }
        }
        private int FallingSpeed
        {
            get
            {
                if (_controller.Act(GameAction.Down))
                {
                    return 15*_gameSpeed;
                }
                return 1*_gameSpeed;
            }
        }
        private void GenTetris()
        {
            if (_state == 0) return;
            if (Block!=null) throw new Exception("multiple sprite generating");
            var b = _factory.GenTetris();
            b.LPos = _h - 1;
            if (Try(b))
            {
                Block = b;
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
                Block.Rotate();
                if (Intersect())
                {
                    Block.CounterRotate();
                }
            }

            if (_controller.Act(GameAction.Left))
            {
                Block.RPos--;
                if (Intersect())
                {
                    Block.RPos++;
                }
            }
            if (_controller.Act(GameAction.Right))
            {
                Block.RPos++;
                if (Intersect())
                {
                    Block.RPos--;
                }
            }
        }

        private void HandleFalling()
        {
            if (Try(Block.Clone().Fall()))
            {
                Block.Fall();
            }
            else
            {
                AddToUnderlying();
            }
        }

        private void AddToUnderlying()
        {
            AddToUnderlyingEvent.Invoke(this,new AddToUnderlyingEventArgs(_tick,Block));
            if (Block != null)
            {
                for (int i = 0; i < Block.Height; i++)
                    for (int j = 0; j < Block.Width; j++)
                    {
                        if (Block.SquareAt(i, j) != null)
                        {
                            if (!Valid(Block.LPos + i, Block.RPos + j))
                            {
                                End();
                                return;
                            }
                            _underLying[Block.LPos + i, Block.RPos + j] = Block.SquareAt(i, j);
                        }
                    }
                Block = null;
            }
        }
    }
}
