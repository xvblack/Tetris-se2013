using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class AIController : IController
    {
        private readonly TetrisGame _game;
        private int _sum;
        //private readonly Square[,] _underLying;

        public AIController(TetrisGame game)
        {
            _game = game;
            _sum = 0;
            //_underLying = null;
            Act(TetrisGame.GameAction.Down);
        }

        private readonly Dictionary<TetrisGame.GameAction, int> _keyState = new Dictionary<TetrisGame.GameAction, int>()
        {
            {TetrisGame.GameAction.Left,0},
            {TetrisGame.GameAction.Right,0},
            {TetrisGame.GameAction.Rotate,0},
            {TetrisGame.GameAction.Down,0}
        };

        void GenerateControll()
        {
            Block block = _game.block;
            Square[,] _underLying = _game.UnderLying;
            _keyState[TetrisGame.GameAction.Left] = 5;
            _keyState[TetrisGame.GameAction.Rotate] = 10;
            _sum = 15;
        }
        public bool Act(TetrisGame.GameAction action)
        {
            bool result = false;
            if (_keyState[action] > 0)
            {
                result = true;
                _keyState[action]--;
                _sum--;
            }
            if (_sum == 0)
            {
                //if (_underLying )
                    GenerateControll();
            }
            return result;
        }
    }
}
