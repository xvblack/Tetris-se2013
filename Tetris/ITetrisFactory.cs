namespace Tetris.GameBase
{
    public interface ITetrisFactory
    {
        Block GenTetris();
        TetrisGame Game { get; set; }
    }
}