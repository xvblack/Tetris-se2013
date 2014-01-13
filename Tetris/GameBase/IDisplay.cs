namespace Tetris.GameBase
{
    public interface IDisplay
    {
        void OnDrawing(TetrisGame sender, TetrisGame.DrawEventArgs e); // 绘制回调函数
    }
}
