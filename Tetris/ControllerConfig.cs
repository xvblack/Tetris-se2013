using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tetris.GameBase;

namespace Tetris.GameControl
{
    /**
     * 此类保存了键盘按键和游戏操作的对应关系，可以通过Add()方法进行修改和添加操作.
     * 
     * 要修改按键的方法是，先创建一个ControllerConfig类，然后通过Put方法修改
     * 按键的映射关系，然后把这个类作为参数构造一个新的PlayerController类；
     * 
     * 注意，需要在MainWindow的OnKeyDown和OnKeyUp中把按键事件传递给PlayerController。
     * 
     */

    class ControllerConfig : Dictionary<Key,TetrisGame.GameAction>
    {
        //构造方法,如果未指定则使用默认配置.
        public ControllerConfig():base()
        {
            this.Put(Key.Down, TetrisGame.GameAction.Down);
            this.Put(Key.Up, TetrisGame.GameAction.Rotate);
            this.Put(Key.Left, TetrisGame.GameAction.Left);
            this.Put(Key.Right, TetrisGame.GameAction.Right);
        }

        public void Put(Key key, TetrisGame.GameAction action)
        {
            //如果已存在这种映射，就先将他覆盖掉，再加入新的映射关系.
            if (this.ContainsKey(key))
            {
                this.Remove(key);
            }
            else
            {
            }
            base.Add(key,action);
        }
        
    }
}
