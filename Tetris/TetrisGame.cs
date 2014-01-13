using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using Tetris.GameSystem;
using Tetris.Properties;

namespace Tetris.GameBase
{
    public partial class TetrisGame
    {
        public enum GameAction
        {
            Rotate,
            Left,
            Right,
            Down,
            Pause
        };

        /// <summary>
        /// 事件参数以及回调函数类型
        /// </summary>

        #region Events

        public event UpdateBeginCallback UpdateBeginEvent = delegate { }; // 更新开始事件
        public event ClearBarCallback ClearBarEvent = delegate { }; // 清除行事件
        public event ClearBarCallback BeforeClearBarEvent = delegate { }; // 清除行之前事件
        public event UpdateEndCallback UpdateEndEvent = delegate { }; // 更新结束事件
        public event DrawCallback DrawEvent = delegate { }; // 绘制事件
        public event GameEndCallback GameEndEvent = delegate { }; // 游戏结束事件
        public event AddToUnderlyingCallback AddToUnderlyingEvent = delegate { }; // 方块加入Underlying事件

        #endregion

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
            public DrawEventArgs(int tick) : base(tick)
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

            public AddToUnderlyingEventArgs(int tick, Block addingBlock) : base(tick)
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

        /// <summary>
        /// 更新操作回调函数
        /// </summary>
        private delegate void UpdateCallback();

        /// <summary>
        /// 游戏Id
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// 正在下落的方块
        /// </summary>
        public Block Block { get; private set; }
        /// <summary>
        /// 每Round的Tick数
        /// </summary>
        public int RoundTicks = Properties.Settings.Default.RoundTicks;
        /// <summary>
        /// 游戏速度
        /// </summary>
        public int GameSpeed { get; set; } 
        /// <summary>
        /// 是否需要重新绘制
        /// </summary>
        public volatile bool NeedDraw; 
        /// <summary>
        /// 已落下方块
        /// </summary>
        private readonly SquareArray _underLying;
        /// <summary>
        /// 内部tick
        /// </summary>
        private int _tick;
        /// <summary>
        /// 游戏状态
        /// 0 for game ending, 1 for looping, 2 for pause
        /// </summary>
        private volatile int _state;
        /// <summary>
        /// 上一轮刚刚落下的方块
        /// </summary>
        private Stack<Square> _newSquares = new Stack<Square>();

        /// <summary>
        /// 帧数
        /// </summary>
        public int Fps { get; set; }

        /// <summary>
        /// 方块工厂
        /// </summary>
        public ITetrisFactory Factory
        {
            get { return _factory; }
        }

        /// <summary>
        /// 基本画布，记录已经落下的方块
        /// </summary>
        public SquareArray UnderLying
        {
            get { return _underLying; }
        }

        /// <summary>
        /// 游戏高度
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// 游戏宽度
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// 游戏控制器
        /// </summary>
        public IController Controller 
        {
            get { return _controller; }
        }

        #region Reference to External Objects

        private readonly ITetrisFactory _factory; // 方块生成工厂
        private IController _controller; // 游戏控制器

        #endregion



        public TetrisGame(int id, IEnumerable<SquareArray> styles, IEngine engine, ITetrisFactory factory, int w, int h,
            int gameSpeed)
        {
            Width = w;
            Height = h;
            GameSpeed = gameSpeed;
            engine.TickEvent += UpdateDispatch;
            engine.PropertyChanged +=
                delegate(object sender, PropertyChangedEventArgs e) { this.Fps = (sender as IEngine).Fps; };
            _underLying = new SquareArray(h, w);
            _factory = factory;
            _factory.Game = this;
            if (_factory is CacheFactory) // 如果是CacheFactory，初始化
            {
                (_factory as CacheFactory).Init();
            }
            InitTickAPI();
            Id = id;
            _tick = 0;
            _state = 0;
        }

        /// <summary>
        /// 开始
        /// </summary>
        public void Start() 
        {
            if (Block == null) GenTetris();
            _state = 1;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause() 
        {
            //Debug.Assert(_state==1,"Can Only Pause a Ongoing Game");
            _state = 2;
        }

        /// <summary>
        /// 继续
        /// </summary>
        public void Continue() 
        {
            _state = 1;
        }

        /// <summary>
        /// 暂停或继续
        /// </summary>
        public void PauseOrContinue() 
        {
            if (_state == 1)
            {
                Pause();
                if (IsDuelGame) DuelGame.Pause();
            }
            else
            {
                Continue();
                if (IsDuelGame) DuelGame.Continue();
            }
        }

        /// <summary>
        /// 结束
        /// </summary>
        public void End() 
        {
            _state = 0;
            GameEndEvent.Invoke(this, new GameEndEventArgs(this.ScoreSystem.Score));
            Console.Out.Write("end");
        }

        /// <summary>
        /// 添加显示
        /// </summary>
        /// <param name="display"></param>
        public void AddDisplay(IDisplay display) 
        {
            DrawEvent += display.OnDrawing;
        }

        /// <summary>
        /// 设置控制器
        /// </summary>
        /// <param name="controller"></param>
        public void SetController(IController controller) 
        {
            _controller = controller;
        }

        /// <summary>
        /// 现在游戏的图像
        /// </summary>
        public Square[,] Image
        {
            get
            {
                var result = new SquareArray(Height, Width);
                Array.Copy(_underLying.Storage, result.Storage, Height*Width);
                if (Block != null)
                    for (var i = 0; i < Block.Height; i++)
                        for (var j = 0; j < Block.Width; j++)
                        {
                            if (Block.SquareAt(i, j) != null && Valid(Block.LPos + i, Block.RPos + j))
                                result[Block.LPos + i, Block.RPos + j] = Block.SquareAt(i, j);
                        }
                return result.Storage;
            }
        }

        /// <summary>
        /// 检验传入方块是否可行
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public bool Try(Block block)
        {
            var result = true;
            var tempBlock = Block;
            Block = block;
            if (Intersect()) result = false;
            Block = tempBlock;
            return result;
        }

        /// <summary>
        /// 每回合调用times次cb，辅助函数
        /// </summary>
        /// <param name="times">调用次数</param>
        /// <param name="cb">回调函数</param>
        private void PerRound(int times, UpdateCallback cb) 
        {
            if (times >= RoundTicks || _tick%(RoundTicks/times) == 0)
            {
                cb();
            }
        }

        /// <summary>
        /// 记录新方块
        /// </summary>
        /// <param name="s"></param>
        public void PushNewSquare(Square s) 
        {
            _newSquares.Push(s);
        }

        /// <summary>
        /// 位置是否合法
        /// </summary>
        /// <param name="i">y</param>
        /// <param name="j">x</param>
        /// <param name="UpperBound"></param>
        /// <returns></returns>
        private bool Valid(int i, int j, bool UpperBound = true) 
        {
            if (UpperBound)
            {
                return (i >= 0) && (i < Height) && (j >= 0) && (j < Width);
            }
            else
            {
                return (i >= 0) && (j >= 0) && (j < Width);
            }
        }

        /// <summary>
        /// 现在的Block是否与Underlying相交
        /// </summary>
        /// <returns></returns>
        private bool Intersect() 
        {
            for (int i = 0; i < Block.Height; i++)
                for (int j = 0; j < Block.Width; j++)
                {
                    if ((Block.SquareAt(i, j) != null) && (Valid(Block.LPos + i, Block.RPos + j)) &&
                        (_underLying[Block.LPos + i, Block.RPos + j] != null))
                    {
                        return true;
                    }
                    if ((Block.SquareAt(i, j) != null) && (!Valid(Block.LPos + i, Block.RPos + j, false)))
                        return true;
                }
            return false;
        }

        /// <summary>
        /// 去掉当前Block
        /// </summary>
        public void ClearBlock() 
        {
            Block = null;
        }

        /// <summary>
        /// 延时回调
        /// </summary>
        public delegate void LaterCallback(); 

        /// <summary>
        /// 回调函数记录
        /// </summary>
        private Dictionary<int, List<LaterCallback>> _laterCallbacks = new Dictionary<int, List<LaterCallback>>();

        /// <summary>
        /// ticks之后调用cb
        /// </summary>
        /// <param name="tick">延时tick数</param>
        /// <param name="cb">回调函数</param>
        public void Later(int tick, LaterCallback cb)
        {
            if (!_laterCallbacks.ContainsKey(_tick + tick))
            {
                _laterCallbacks[_tick + tick] = new List<LaterCallback>();
            }
            _laterCallbacks[_tick + tick].Add(cb);
        }

        /// <summary>
        /// 更新主函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="tick"></param>
        private void UpdateDispatch(object sender, int tick) 
        {
            if (NeedDraw) DrawEvent.Invoke(this, new DrawEventArgs(_tick));
            NeedDraw = false;
            HandlePause();
            //lock (this)
            {
                if (Block != null && Block.IsVoid) Block = null;
                if (_pendingLines.Count >= 2) PushLines();
                _pendingLines.Clear();
                if (_state == 0) return;
                if (_state == 2) return;
                if (Block == null) GenTetris();
                if (_state == 0)
                    return; // Check ending caused by cannot generate new block
                if (_state == 2) return; // Check for pause
                foreach (var newSquare in _newSquares)
                {
                    newSquare.Devoid();
                }
                _newSquares.Clear();
                _tick++; // internal tick add
                if (_laterCallbacks.ContainsKey(_tick))
                {
                    foreach (var cb in _laterCallbacks[_tick])
                    {
                        cb();
                    }
                    _laterCallbacks.Remove(_tick);
                }
                UpdateBeginEvent.Invoke(this, new UpdateBeginEventArgs(_tick));
                Debug.Assert(Block != null, "loop continue when Block is null");
                //Debug.Assert(Block.LPos>=0);
                Debug.Assert(Block.Id != Block.TempId, "loop using a temp block");
                Block.FallSpeed = FallingSpeed; // Use the Game FallingSpeed as the block fall speed
                PerRound(Settings.Default.TickActions, HandleAction);
                PerRound(Settings.Default.TickFalling, HandleFallingAction);
                PerRound(1*Block.FallSpeed, HandleFalling);
                ClearBar();
                UpdateEndEvent.Invoke(this, new UpdateEndEventArgs(_tick));
                if (_state == 0) return;
            }
        }

        /// <summary>
        /// 处理暂停
        /// </summary>
        private void HandlePause() 
        {
            if (_controller == null) return;
            if (_controller.Act(GameAction.Pause))
            {
                this.PauseOrContinue();
            }
        }

        /// <summary>
        /// 消行
        /// </summary>
        private void ClearBar() 
        {
            for (var i = 0; i < Height; i++)
            {
                bool clear = true;
                for (int j = 0; j < Width; j++)
                {
                    if (_underLying[i, j] == null) clear = false;
                }
                if (clear)
                {
                    BeforeClearBarEvent.Invoke(this, new ClearBarEventArgs(_underLying, i, _tick));
                    for (var s = i; s < Height - 1; s++)
                        for (var j = 0; j < Width; j++)
                        {
                            _underLying[s, j] = _underLying[s + 1, j];
                        }
                    for (int j = 0; j < Width; j++)
                    {
                        _underLying[Height - 1, j] = null;
                    }
                    ClearBarEvent.Invoke(this, new ClearBarEventArgs(_underLying, i, _tick));
                    i--;
                }
            }
            // DrawEvent.Invoke(this, new DrawEventArgs(_tick));
            NeedDraw = true;
        }

        /// <summary>
        /// 是否下落加速
        /// </summary>
        private bool _speedUp = false;

        /// <summary>
        /// 下落速度
        /// </summary>
        private int FallingSpeed 
        {
            get
            {
                if (_speedUp)
                {
                    return Settings.Default.SpeedUp*GameSpeed;
                }
                return 1*GameSpeed;
            }
        }

        /// <summary>
        /// 生成方块
        /// </summary>
        private void GenTetris() 
        {
            if (_state == 0) return;
            if (Block != null) throw new Exception("multiple sprite generating");
            var b = _factory.GenTetris();
            b.LPos = Height - 1;
            if (Try(b))
            {
                Block = b;
            }
            else
            {
                End();
            }
        }

        /// <summary>
        /// 处理加速操作
        /// </summary>
        private void HandleFallingAction() 
        {
            if (_controller.Act(GameAction.Down))
            {
                _speedUp = true;
            }
            else
            {
                _speedUp = false;
            }
            Trace.WriteLine(_speedUp);
        }

        /// <summary>
        /// 处理其他操作
        /// </summary>
        private void HandleAction() 
        {
            if (_controller.Act(GameAction.Rotate))
            {
                Block.Rotate();
                if (Intersect())
                {
                    Block.CounterRotate();
                }
                else
                {
                    NeedDraw = true;
                }
            }

            if (_controller.Act(GameAction.Left))
            {
                Block.RPos--;
                if (Intersect())
                {
                    Block.RPos++;
                }
                else NeedDraw = true;
            }
            if (_controller.Act(GameAction.Right))
            {
                Block.RPos++;
                if (Intersect())
                {
                    Block.RPos--;
                }
                else NeedDraw = true;
            }
            if (_controller.Act(GameAction.Pause))
            {
                HandlePause();
            }
        }

        /// <summary>
        /// 处理下落
        /// </summary>
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
            NeedDraw = true;
        }
        
        /// <summary>
        /// 添加到已落下方块
        /// </summary>
        private void AddToUnderlying() 
        {
            AddToUnderlyingEvent.Invoke(this, new AddToUnderlyingEventArgs(_tick, Block));
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
                            _underLying[Block.LPos + i, Block.RPos + j] = Block.SquareAt(i, j).Clone();
                            _newSquares.Push(_underLying[Block.LPos + i, Block.RPos + j]);
                        }
                    }
                Block = null;
            }
            NeedDraw = true;
        }
    }
}