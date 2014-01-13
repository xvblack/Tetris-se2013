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

    public class ControllerConfig : Dictionary<Key,TetrisGame.GameAction>
    {
        public enum ConfigType { Player1, Player2 };
        static private Dictionary<TetrisGame.GameAction, Key>[] DefaultConfig = new Dictionary<TetrisGame.GameAction, Key>[2]
        { new Dictionary<TetrisGame.GameAction, Key>() {
            { TetrisGame.GameAction.Left,   Key.A     },
            { TetrisGame.GameAction.Right,  Key.D     },
            { TetrisGame.GameAction.Rotate, Key.W     },
            { TetrisGame.GameAction.Down,   Key.S     },
            { TetrisGame.GameAction.Pause,  Key.Space }},  
          new Dictionary<TetrisGame.GameAction, Key>() {
            { TetrisGame.GameAction.Left,   Key.Left  },
            { TetrisGame.GameAction.Right,  Key.Right },
            { TetrisGame.GameAction.Rotate, Key.Up    },
            { TetrisGame.GameAction.Down,   Key.Down  },
            { TetrisGame.GameAction.Pause,  Key.Enter }}
        };

        public Dictionary<TetrisGame.GameAction, Key> inversedKeyAndValue = new Dictionary<TetrisGame.GameAction, Key>();

        //构造方法,如果未指定则使用默认配置.
        public ControllerConfig():base()
        {
            this.Put(ControllerConfig.DefaultConfig[1][TetrisGame.GameAction.Down], TetrisGame.GameAction.Down);
            this.Put(ControllerConfig.DefaultConfig[1][TetrisGame.GameAction.Rotate], TetrisGame.GameAction.Rotate);
            this.Put(ControllerConfig.DefaultConfig[1][TetrisGame.GameAction.Left], TetrisGame.GameAction.Left);
            this.Put(ControllerConfig.DefaultConfig[1][TetrisGame.GameAction.Right], TetrisGame.GameAction.Right);
            this.Put(ControllerConfig.DefaultConfig[1][TetrisGame.GameAction.Pause], TetrisGame.GameAction.Pause);
        }
        public ControllerConfig(ConfigType type)
        {
            defaultPut(type);
        }

        private void defaultPut(ConfigType type)
        {
            if (ConfigType.Player1 == type)
            {
                this.Clear();
                this.Put(ControllerConfig.DefaultConfig[0][TetrisGame.GameAction.Down], TetrisGame.GameAction.Down);
                this.Put(ControllerConfig.DefaultConfig[0][TetrisGame.GameAction.Rotate], TetrisGame.GameAction.Rotate);
                this.Put(ControllerConfig.DefaultConfig[0][TetrisGame.GameAction.Left], TetrisGame.GameAction.Left);
                this.Put(ControllerConfig.DefaultConfig[0][TetrisGame.GameAction.Right], TetrisGame.GameAction.Right);
                this.Put(ControllerConfig.DefaultConfig[0][TetrisGame.GameAction.Pause], TetrisGame.GameAction.Pause); 
            }
            else if (ConfigType.Player2 == type)
            {
                this.Clear();
                this.Put(ControllerConfig.DefaultConfig[1][TetrisGame.GameAction.Down], TetrisGame.GameAction.Down);
                this.Put(ControllerConfig.DefaultConfig[1][TetrisGame.GameAction.Rotate], TetrisGame.GameAction.Rotate);
                this.Put(ControllerConfig.DefaultConfig[1][TetrisGame.GameAction.Left], TetrisGame.GameAction.Left);
                this.Put(ControllerConfig.DefaultConfig[1][TetrisGame.GameAction.Right], TetrisGame.GameAction.Right);
                this.Put(ControllerConfig.DefaultConfig[1][TetrisGame.GameAction.Pause], TetrisGame.GameAction.Pause);
            }
            
        }

        //以参数文件路径为参数的构造方法
        public ControllerConfig(String path)
            : base()
        {
            if (File.Exists(path)) this.Load(path);
            else
            {
                if (path == Properties.Settings.Default.Player1Path)
                {
                    defaultPut(ConfigType.Player1);
                }
                else
                {
                    defaultPut(ConfigType.Player2);
                }
                Save(path);
            }
            
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
            this.inversedKeyAndValue.Add(action, key);
        }
        
    }
}
