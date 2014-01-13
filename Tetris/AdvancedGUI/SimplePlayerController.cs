using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Tetris.GameBase;

namespace Tetris.AdvancedGUI
{
    class SimplePlayerController : IPlayerController
    {
        public ControllerConfig Config {get; set;}
        private bool _inversed;

        private readonly IDictionary<TetrisGame.GameAction, bool> _pressing;
        private readonly IDictionary<TetrisGame.GameAction, int> _pressed;

        public SimplePlayerController()
        {
            _pressing=new Dictionary<TetrisGame.GameAction, bool>()
            {
                {TetrisGame.GameAction.Down, false},
                {TetrisGame.GameAction.Left, false},
                {TetrisGame.GameAction.Right,false},
                {TetrisGame.GameAction.Rotate,false},
                {TetrisGame.GameAction.Pause,false},
            };
            _pressed = new Dictionary<TetrisGame.GameAction, int>()
            {
                {TetrisGame.GameAction.Down, 0},
                {TetrisGame.GameAction.Left, 0},
                {TetrisGame.GameAction.Right,0},
                {TetrisGame.GameAction.Rotate,0},
                {TetrisGame.GameAction.Pause,0},
            };
            _inversed = false;
        }

        public ControllerConfig GetConfig()
        {
            return Config;
        }

        public void SetConfig(ControllerConfig config)
        {
            Config = config;
        }

        public bool Act(TetrisGame.GameAction action)
        {
            if (_inversed)
            {
                if (action == TetrisGame.GameAction.Left)
                {
                    action = TetrisGame.GameAction.Right;
                }
                else if (action == TetrisGame.GameAction.Right)
                {
                    action = TetrisGame.GameAction.Left;
                }
            }
            if (_pressed[action] > 0)
            {
                _pressed[action] --;
                return true;
            }
            return _pressing[action];
        }

        public void InverseControl()
        {
            _inversed = !_inversed;
        }

        public void SetInversed(bool isInversed)
        {
            _inversed = isInversed;
        }

        public void OnKeyDown(KeyEventArgs e)
        {
            if (!e.IsRepeat)
            {
                TetrisGame.GameAction action;
                if (Config.TryGetValue(e.Key, out action))
                {
                    //Console.WriteLine("down {0} act {1}", e.Key.ToString(), action.ToString());
                    _pressing[action] = true;
                    _pressed[action]++;
                }
            }
        }

        public void OnKeyUp(KeyEventArgs e)
        {
            TetrisGame.GameAction action;
            if (Config.TryGetValue(e.Key, out action))
            {
                _pressing[action] = false;
            }
        }
    }
}
