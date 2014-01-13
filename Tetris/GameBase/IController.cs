using System;

namespace Tetris.GameBase
{
    public interface IController
    {
        bool Act(TetrisGame.GameAction action); // 检查动作是否按下
        void InverseControl(); // 反向
        void SetInversed(Boolean isInversed);
    }
}