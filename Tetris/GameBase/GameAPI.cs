using System.Collections.Generic;
using System.Diagnostics;
using Tetris.GameSystem;

namespace Tetris.GameBase
{
    public partial class TetrisGame
    {
        #region TickAPI
        public int TickClearedBars
        {
            get;
            private set;
        }


        private void InitTickAPI()
        {
            TickClearedBars = 0;
            ClearBarEvent += TickOnClearBar;
            UpdateBeginEvent += TickOnUpdateBegin;
        }

        private void TickOnClearBar(object sender, ClearBarEventArgs e)
        {
            TickClearedBars++;
            Trace.WriteLine(e.Tick);
        }

        private void TickOnUpdateBegin(object sender, UpdateBeginEventArgs e)
        {
            TickClearedBars = 0;
        }
        #endregion
        #region Score System Api
        public ScoreSystem ScoreSystem;
        #endregion
        #region Achievement System Api

        public AchievementSystem.AchievementState AchievementState;

        #endregion
        #region Duel Game Api

        public bool IsDuelGame = false;
        public TetrisGame DuelGame = null;
        private readonly Stack<Square[]> _pendingLines=new Stack<Square[]>();

        private void DevoidBlock() // 标记当前方块无效（下次更新删除）
        {
            if (this.Block != null) Block.IsVoid = true;
        }

        public void PushLine(Square[] line) // 添加下一次更新中要加入的行
        {
            _pendingLines.Push(line);
        }

        private void PushLines() // 在底部添加行
        {
            foreach (var line in _pendingLines)
            {
                for (var i = this.Height - 1; i > 0; i--)
                {
                    for (var j = 0; j < this.Width; j++)
                    {
                        this.UnderLying[i, j] = this.UnderLying[i - 1, j];
                    }
                }
                for (var j = 0; j < this.Width; j++)
                {
                    this.UnderLying[0, j] = line[j];
                }
                DevoidBlock();
            }
            _pendingLines.Clear();
        }

        public void SpeedUp() // 加速
        {
            GameSpeed++;
        }
        #endregion
    }

    public partial class Block
    {
        public bool IsVoid=false;
    }
}
