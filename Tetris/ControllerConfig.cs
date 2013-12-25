using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
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

        /**
         * 读取配置文件，参数为文件名
         */
        public void Load(String path)
        {
            XmlReader reader = null;
            TetrisGame.GameAction __action;
            Key __key;
            this.Clear();
            reader = XmlReader.Create(path);
            reader.ReadStartElement("Dict");
            while (true)
            {
                if (reader.GetAttribute("Key") == null) break;  //读取完毕则退出循环
                String __keyString = reader.GetAttribute("Key");
                String __actionStr = reader.GetAttribute("Action");
                __key = (Key)int.Parse(__keyString);
                __action = (TetrisGame.GameAction)int.Parse(__actionStr);
                this.Put(__key, __action);
                reader.ReadStartElement("Pair"); //读取下一个节点
            }
            reader.ReadEndElement();
            reader.Close();
        }

        /**
         * 将当前的配置保存到路径为path的文件中
         */ 
        public Boolean Save(String path)
        {
            Dictionary<Key, TetrisGame.GameAction>.Enumerator enumerator = this.GetEnumerator();
            XmlWriter writer = XmlWriter.Create(path);
            writer.WriteStartDocument(true); 
            writer.WriteStartElement("Dict");
            
            while(enumerator.MoveNext())
            {
                writer.WriteStartElement("Pair");
                writer.WriteAttributeString("Key",((int)enumerator.Current.Key).ToString());
                writer.WriteAttributeString("Action",((int)enumerator.Current.Value).ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
            return true;
        }

        /**
         * 注意：添加新的按键映射时，不检查对应的GameAction是否已经存在，
         * 可能造成多个按键同时映射同一个功能的情况，可以通过Clear方法清楚所有映射再重新写入解决。
         */
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
