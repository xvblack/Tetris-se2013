﻿using System;
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
    public class DuelGame : Tuple<TetrisGame, TetrisGame>
    {
        public int Winner { get; private set; }
        public delegate void DuelGameEndHandler(DuelGame game,int winner) ;
        public event DuelGameEndHandler DuelGameEndEvent;
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
        private readonly IEngine _engine;
        private Dictionary<int,TetrisGame> games;
        private Random ran;
        private static List<Tetrisor> tetrisors=new List<Tetrisor>();
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

        public DuelGame NewDuelGame(string user1="",string user2="AI",bool withItem=true)
        {
            var game1 = NewGame(user1,null,withItem);
            var game2 = NewGame(user2,null,withItem);
            (game1.Factory as TetrisItemFactory).IsDuel = true;
            (game2.Factory as TetrisItemFactory).IsDuel = true;
            // preserved for duel game
            return new DuelGame(game1,game2);
        }

        public TetrisFactory NewFactory()
        {
            return new TetrisFactory(Square.Styles(styles));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
