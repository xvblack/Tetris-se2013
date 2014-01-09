using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.GameBase;
using Tetris.GameSystem;
using Tetris.Properties;

namespace Tetris
{
    /// <summary>
    /// 对战游戏
    /// </summary>
    public class DuelGame : Tuple<TetrisGame, TetrisGame>
    {
        /// <summary>
        /// 胜者
        /// </summary>
        public int Winner { get; private set; }
        /// <summary>
        /// 对战游戏结束回调
        /// </summary>
        /// <param name="game"></param>
        /// <param name="winner"></param>
        public delegate void DuelGameEndHandler(DuelGame game,int winner) ;
        /// <summary>
        /// 对战游戏结束事件
        /// </summary>
        public event DuelGameEndHandler DuelGameEndEvent;
        /// <summary>
        /// 是否已经结束
        /// </summary>
        private volatile bool ended = false;

        public DuelGame(TetrisGame item1, TetrisGame item2) : base(item1, item2)
        {
            Winner = -1;
            DuelGameEndEvent += delegate { Trace.WriteLine("ended"); };
            item1.IsDuelGame = true;
            item2.IsDuelGame = true;
            item1.DuelGame = item2;
            item2.DuelGame = item1;
            item1.GameEndEvent += delegate(object sender, TetrisGame.GameEndEventArgs e)
            {
                if (!ended)
                {
                    Winner = 1;
                    this.ended = true;
                    item2.End();
                    DuelGameEndEvent.Invoke(this, Winner);
                }
            };
            item2.GameEndEvent += delegate(object sender, TetrisGame.GameEndEventArgs e)
            {
                if (!ended)
                {
                    Winner = 0;
                    this.ended = true;
                    item1.End();
                    DuelGameEndEvent.Invoke(this, Winner);
                }
            };

        }
    }

    public class Tetrisor
    {
        /// <summary>
        /// 公用引擎
        /// </summary>
        private readonly IEngine _engine;
        /// <summary>
        /// 生成的全部游戏
        /// </summary>
        private Dictionary<int,TetrisGame> games;
        /// <summary>
        /// 全局随机数生成器
        /// </summary>
        private Random ran;
        /// <summary>
        /// 静态变量记录全部Tetrisor
        /// </summary>
        private static List<Tetrisor> tetrisors=new List<Tetrisor>();
        /// <summary>
        /// 全部方块样式
        /// </summary>
        readonly int[][,] styles = {new int[2, 2]{{1, 1}, {1, 1}}, new int[1, 4]{{2, 2, 2, 2}}, new int[2, 3]{{0, 3, 0},{3, 3, 3}},
                               new int[2, 3]{{4, 0, 0}, {4, 4, 4}}, new int[2, 3]{{0, 0, 5}, {5, 5, 5}}, 
                               new int[2, 3]{{6, 6, 0}, {0, 6, 6}}, new int[2, 3]{{0, 7, 7}, {7, 7, 0}}};
        public Tetrisor()
        {
            _engine=new SimpleEngine();
            _engine.Interval = 0.005;
            games=new Dictionary<int, TetrisGame>();
            _engine.Enabled = true;
            ran = new Random();
            tetrisors.Add(this);
        }
        /// <summary>
        /// 停止全部引擎
        /// </summary>
        public static void StopEngines()
        {
            foreach (var tetrisor in tetrisors)
            {
                tetrisor.StopEngine();
            }
        }

        public void StopEngine()
        {
            _engine.Enabled = false;
        }
        /// <summary>
        /// 生成新游戏
        /// </summary>
        /// <param name="user">用户名</param>
        /// <param name="controller">控制器</param>
        /// <param name="withItem">是否启用道具</param>
        /// <returns></returns>
        public TetrisGame NewGame(string user="",IController controller=null,bool withItem=true)
        {
            var id = games.Count;          
            ITetrisFactory factory;
            if (withItem)
            {
                factory = new CacheFactory(Square.Styles(styles), ran);
                (factory as TetrisItemFactory).GenSpecialBlock = true;
            }
            else
            {
                factory = new TetrisFactory(Square.Styles(styles), ran);
            }

            // use AdvancedGUI.Styles.WindowSizeGenerator to set the game size! shuo han

            int gameWidth = AdvancedGUI.Styles.WindowSizeGenerator.gameWidth;
            int gameHeight = AdvancedGUI.Styles.WindowSizeGenerator.gameHeight;
            
            var game = new TetrisGame(id,Square.Styles(styles), _engine, factory,gameWidth,gameHeight,Settings.Default.DefaultSpeed);
            game.SetController(controller);
            ItemSystem.Bind(game);
            ScoreSystem.Bind(game);
            AchievementSystem.Bind(game,user);
            games[id] = game;
            return game;
        }
        /// <summary>
        /// 新对战游戏
        /// </summary>
        /// <param name="user1">用户1用户名</param>
        /// <param name="user2">用户2用户名</param>
        /// <param name="withItem">是否启用道具</param>
        /// <returns></returns>
        public DuelGame NewDuelGame(string user1="",string user2="",bool withItem=true)
        {
            var game1 = NewGame(user1,null,withItem);
            var game2 = NewGame(user2,null,withItem);
            (game1.Factory as TetrisItemFactory).IsDuel = true;
            (game2.Factory as TetrisItemFactory).IsDuel = true;
            // preserved for duel game
            return new DuelGame(game1,game2);
        }
    }
}
