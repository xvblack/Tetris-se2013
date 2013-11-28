using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.GameBase;

namespace Tetris
{
    class ItemSystem
    {
        public static void Bind(TetrisGame game)
        {
            game.UpdateEndEvent += ProcessItem;
        }

        public static void ProcessItem(TetrisGame game, TetrisGame.UpdateEndEventArgs e)
        {
            if (game.Block is ItemBlock)
            {
                if (game.Block is GunItemBlock)
                {
                    var block = game.Block as GunItemBlock;
                    if (block.Acted())
                    {
                        var j = block.RPos;
                        for (var i = game.Height - 1; i >= 0; i--)
                        {
                            if (game.UnderLying[i, j] != null)
                            {
                                game.UnderLying[i, j] = null;
                                break;
                            }
                        }
                        block.ResetDirection();
                    }
                }
            }
        }
    }
}
