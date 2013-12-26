using Tetris.GameBase;
using System.ComponentModel;
using System.Diagnostics;

namespace Tetris.GameSystem
{
    /// <summary>
    /// 得分系统
    /// INotifyPropertyChanged 用于GUI
    /// </summary>
    public class ScoreSystem : INotifyPropertyChanged
    {
        private int _score = 0;
        public static void Bind(TetrisGame game)
        {
            var ss = new ScoreSystem();
            game.ScoreSystem = ss;
            game.UpdateEndEvent += ss.OnUpdateEnd;
            //game.ClearBarEvent += ss.OnUpdateEnd;
        }
        public int Score 
        {
            get 
            { 
                return _score; 
            }
            private set 
            {
                if (value != _score)
                {
                    _score = value;
                    Notify("Score");
                }
            } 
        }

       // private void OnUpdateEnd(object sender, TetrisGame.ClearBarEventArgs e)
        private void OnUpdateEnd(object sender, TetrisGame.UpdateEndEventArgs e)
        {
            var game = (TetrisGame) sender;
            Score += game.TickClearedBars * game.TickClearedBars;
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void Notify(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
