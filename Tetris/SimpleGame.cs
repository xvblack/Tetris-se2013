using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.GameBase;

namespace Tetris
{
    class SimpleGame
    {
        private class ClearRightItem
        {
            public static void ClearRight(TetrisGame game)
            {
                lock (game.Updating)
                {
                    for (var i = 0; i < game.Height; i++) game.UnderLying[i, game.Width - 1] = null;
                }
            }
        }

    }
}
