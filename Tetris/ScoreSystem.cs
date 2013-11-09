using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.GameBase;

namespace Tetris
{
    namespace GameSystem
    {
        public class ScoreSystem
        {
            public int Score { get; private set; }

            public void OnUpdateEnd(object sender, TetrisGame.UpdateEndEventArgs e)
            {
                var game = (TetrisGame) sender;
                Score += game.TickScore * game.TickScore;
            }
        }
    }
}
