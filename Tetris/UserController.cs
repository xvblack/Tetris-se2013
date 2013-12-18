using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.GameBase;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Tetris
{
    
    
    public class UserController : IController
    {
        public AdvancedGUI.MainWindow holderWin;

        public UserController() {
            Console.WriteLine("controller in");

        }
        public void setupController(){
            Console.WriteLine("setup in");
            if (holderWin != null) {
                holderWin.PreviewKeyDown += keyPressed;
                Console.WriteLine("key presedd added");
            }
        }
        
        static readonly Dictionary<TetrisGame.GameAction,Key> 
            Ht=new Dictionary<TetrisGame.GameAction, Key>()
        {
            {TetrisGame.GameAction.Left,Key.Left},
            {TetrisGame.GameAction.Right,Key.Right},
            {TetrisGame.GameAction.Down,Key.Down},
            {TetrisGame.GameAction.Rotate,Key.Up}
        };

        private readonly Dictionary<Key,bool> _keyState
            = new Dictionary<Key, bool>()
            {
                {Key.Left,false},
                {Key.Right,false},
                {Key.Up,false},
                {Key.Down,false}
            };
        static public readonly Key[] Keys = new Key[]
        {
            Key.Left,
            Key.Right,
            Key.Up,
            Key.Down
        };

        public Dictionary<Key, bool> KeyState
            {
                get { return _keyState; }
            }
            public bool Act(TetrisGame.GameAction action)
            {
                var result= KeyState[Ht[action]];
                KeyState[Ht[action]] = false;
                return result;
            }

        public void keyPressed(object sender, KeyEventArgs e)
        {
            Console.WriteLine("keypressd in");
            if (Keys.Contains(e.Key))
            {
                this.KeyState[e.Key] = true;
                if (e.Key == Key.Left) 
                    this.KeyState[Key.Right] = false;
                if (e.Key == Key.Right)
                    this.KeyState[Key.Left] = false;
            }
        }
    }
}
