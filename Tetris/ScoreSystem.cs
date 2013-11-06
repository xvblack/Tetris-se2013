using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class ScoreSystem
    {
        public int Score { get; private set; }
        private int _tickScore=0;
        public void OnClearBar(object sender, TetrisGame.ClearBarEventArgs e)
        {
            _tickScore++;
        }

        public void OnUpdateEnd(object sender, TetrisGame.UpdateEndEventArgs e)
        {
            Score += _tickScore * _tickScore;
            _tickScore = 0;
        }
    }
}
