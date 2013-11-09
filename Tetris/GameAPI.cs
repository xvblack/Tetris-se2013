using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.GameBase
{
    public partial class TetrisGame
    {
        #region TickAPI
        public int TickScore
        {
            get;
            private set;
        }


        private void InitTickAPI()
        {
            TickScore = 0;
            ClearBarEvent += TickOnClearBar;
            UpdateBeginEvent += TickOnUpdateBegin;
        }

        private void TickOnClearBar(object sender, ClearBarEventArgs e)
        {
            TickScore++;
        }

        private void TickOnUpdateBegin(object sender, UpdateBeginEventArgs e)
        {
            TickScore = 0;
        }
        #endregion
        #region UnderlyingAPI

        public int UpperBound
        {
            get
            {
                return 0;
            }
        }
        #endregion
    }
}
