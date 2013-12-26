using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tetris.GameBase;


namespace Tetris.GameControl
{

    class PlayerController : IController
    {
        //用于存储用户输入的操作序列,每读取一个操作消除最后一个序列;
        private Stack<TetrisGame.GameAction> actionStack;
        private ControllerConfig config;
        private Boolean _isInversed = false;  //是否设置反转左右按键

        //构造方法，初始化序列，读取配置文件
        public PlayerController()
        {
            actionStack = new Stack<TetrisGame.GameAction>();
            config = new ControllerConfig();//如果未制定则使用默认配置. 
            _isInversed = false;
        }
        //接受外界输入的设置参数创建controller
        public PlayerController(ControllerConfig config)
        {
            actionStack = new Stack<TetrisGame.GameAction>();
            this.config = config;
        }

        //设置和获取控制参数
        public ControllerConfig GetConfig()
        {
            return this.config;
        }
        public void SetConfig(ControllerConfig config)
        {
            this.config = config;
        }

        public void OnKeyDown(KeyEventArgs e)
        {
            TetrisGame.GameAction tempAction;
            TetrisGame.GameAction newAction;
            //如果能在config表中找到对应按键的对应动作，就把该动作添加到动作清单里.
            if (config.TryGetValue(e.Key,out newAction))
            {
                //检查是否反转左右按键
                if (_isInversed)
                {
                    if (newAction == TetrisGame.GameAction.Left)
                    {
                        newAction = TetrisGame.GameAction.Right;
                    }
                    else if (newAction == TetrisGame.GameAction.Right)
                    {
                        newAction = TetrisGame.GameAction.Left;
                    }
                }


                if (actionStack.Count > 0)
                {
                    if ((tempAction = actionStack.Pop()) != newAction)
                    {
                        actionStack.Push(tempAction);
                        actionStack.Push(newAction);
                    }
                    else
                    {
                        actionStack.Push(tempAction);
                    }
                }
                else  //如果堆栈为空，直接将新的action推入
                {
                    actionStack.Push(newAction);
                }
                    
            }
        }
        public  void OnKeyUp(KeyEventArgs e)
        {
        }

        public bool Act(TetrisGame.GameAction action)
        {
            TetrisGame.GameAction tempAction;
            if (actionStack.Count() > 0)
            {
                if ((tempAction = actionStack.Pop()) == action)
                {
                    return true;
                }
                else
                {
                    actionStack.Push(tempAction);
                }
            }
            

            return false;
        }

        //调用此方法反转左右按键
        public void InverseControl()
        {
            _isInversed = !_isInversed;
        }

        public void SetInversed(Boolean __isInversed)
        {
            this._isInversed = __isInversed;
        }
    }
}
