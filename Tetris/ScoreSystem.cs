using Tetris.GameBase;

namespace Tetris.GameSystem
{
    public class ScoreSystem
    {
        public static void Bind(TetrisGame game)
        {
            var ss = new ScoreSystem();
            game.ScoreSystem = ss;
            game.UpdateEndEvent += ss.OnUpdateEnd;
        }
        public int Score { get; private set; }

        private void OnUpdateEnd(object sender, TetrisGame.UpdateEndEventArgs e)
        {
            var game = (TetrisGame) sender;
            Score += game.TickClearedBars * game.TickClearedBars;
        }
    }
}
