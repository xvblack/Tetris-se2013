using System.ComponentModel;

namespace Tetris.GameBase
{
    public delegate void TickHandler(object sender, int tick); // 引擎回调函数
    public interface IEngine:INotifyPropertyChanged
    {
        double Interval { set; } // 最小间隔
        event TickHandler TickEvent; // 定时触发的时间
        bool Enabled { get; set; } // 是否启用
        int Fps { get; }
    }
}
