using Tetris.GameBase;
using System.ComponentModel;
using System.Diagnostics;

namespace Tetris.GameSystem
{
    /// <summary>
    /// 得分系统
    /// </summary>
    public class ScoreSystem
    {
        public static void Bind(TetrisGame game)
        {
            var ss = new ScoreSystem();
            game.ScoreSystem = ss;
            game.ClearBarEvent += ss.OnClearBar;
        }
        public int Score
        {
            get; private set;
        }

        private void OnClearBar(object sender, TetrisGame.ClearBarEventArgs e)
        {
            var game = (TetrisGame) sender;
            Score += game.TickClearedBars * game.TickClearedBars; // 如果消除行，加行数的平方分
            
        }
    }
}
