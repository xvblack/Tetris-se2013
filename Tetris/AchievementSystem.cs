using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.GameBase;

namespace Tetris.GameSystem
{
    public class AchievementSystem
    {
        public class AchievementState
        {
            public int HighScore;
            public int TotalClearBar;
        }

        private static readonly IDictionary<string, AchievementState> States=new Dictionary<string, AchievementState>();

        public static void Bind(TetrisGame game, string name="")
        {
            if (!States.ContainsKey(name))
            {
                States[name]=new AchievementState();
            }
            game.AchievementState = States[name];
            game.GameEndEvent += new TetrisGame.GameEndCallback(
                delegate(object sender, TetrisGame.GameEndEventArgs e)
                {
                    if (e.Score > AchievementSystem.States[name].HighScore)
                        AchievementSystem.States[name].HighScore = e.Score;
                }
                );
            game.UpdateEndEvent += new TetrisGame.UpdateEndCallback(
                delegate(TetrisGame sender, TetrisGame.UpdateEndEventArgs e)
                {
                    AchievementSystem.States[name].TotalClearBar += game.TickClearedBars;
                });
        }

        public void Save(){}
        public void Load(){}

    }
}
