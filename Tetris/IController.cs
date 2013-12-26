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
        bool Act(TetrisGame.GameAction action); // 检查动作是否按下
        void InverseControl(); // 反向
    }
}