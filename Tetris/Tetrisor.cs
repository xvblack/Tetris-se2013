﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.GameBase;
using Tetris.GameSystem;

namespace Tetris
{
    class Tetrisor
    {
        private readonly IEngine _engine;
        private Dictionary<int,TetrisGame> games;
        readonly int[][,] styles = {new int[2, 2]{{1, 1}, {1, 1}}, new int[1, 4]{{1, 1, 1, 1}}, new int[2, 3]{{0, 1, 0},{1, 1, 1}},
                               new int[2, 3]{{1, 0, 0}, {1, 1, 1}}, new int[2, 3]{{0, 0, 1}, {1, 1, 1}}, 
                               new int[2, 3]{{1, 1, 0}, {0, 1, 1}}, new int[2, 3]{{0, 1, 1}, {1, 1, 0}}};
        public Tetrisor()
        {
            _engine=new TimerEngine();
            games=new Dictionary<int, TetrisGame>();
            _engine.Enabled = true;
        }

        public TetrisGame NewGame(IController controller=null)
        {
            var id = games.Count;
            var factory = new TetrisFactory(Square.Styles(styles));
            var game = new TetrisGame(id,Square.Styles(styles), _engine, factory,10,15,1);
            game.SetController(controller);
            ScoreSystem.Bind(game);
            AchievementSystem.Bind(game);
            games[id] = game;
            return game;
        }

        public Tuple<TetrisGame, TetrisGame> NewDuelGame()
        {
            var game1 = NewGame();
            var game2 = NewGame();
            // preserved for duel game
            return new Tuple<TetrisGame, TetrisGame>(game1,game2);
        }
    }
}
