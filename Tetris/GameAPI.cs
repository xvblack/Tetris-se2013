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
        #region UnderlyingAPI

        #endregion
        #region Score System Api
        public ScoreSystem ScoreSystem;
        #endregion
        #region Achievement System Api

        public AchievementSystem.AchievementState AchievementState;

        #endregion
    }
}
