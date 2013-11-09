using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.GameBase;

namespace Tetris
{
    public interface IController
    {
        bool Act(TetrisGame.GameAction action);
    }
}