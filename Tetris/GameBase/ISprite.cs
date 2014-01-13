namespace Tetris.GameBase
{
    interface ISprite
    {
        int LPos { get; set; } // 行位置
        int RPos { get; set; } // 列位置
        int FallSpeed { get; set; } // 下落速度
    }
}
