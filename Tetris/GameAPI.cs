using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Shapes;
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

        private void CleanNewSquare()
        {
            foreach (var square in this._newSquares)
            {
                square.Devoid();
            }
        }

        public bool IsDuelGame = false;
        public TetrisGame DuelGame = null;
        private Queue<Square[]> _pendingLines=new Queue<Square[]>();

        private void DevoidBlock()
        {
            if (this.Block != null) Block.IsVoid = true;
        }

        public void PushLine(Square[] line)
        {
            _pendingLines.Enqueue(line);
        }

        private void PushLines()
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

        public void SpeedUp()
        {
            GameSpeed++;
        }
    }

    public partial class Block
    {
        public bool IsVoid=false;
    }
}
