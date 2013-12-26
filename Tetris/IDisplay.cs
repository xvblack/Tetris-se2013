using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Tetris.GameBase;

namespace Tetris
{
    public interface IDisplay
    {
        void OnDrawing(TetrisGame sender, TetrisGame.DrawEventArgs e); // 绘制回调函数
    }
}
