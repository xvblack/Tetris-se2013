namespace Tetris.GameBase
{
    /// <summary>
    /// 工厂接口
    /// </summary>
    public interface ITetrisFactory
    {
        Block GenTetris();
        TetrisGame Game { get; set; }
    }
}