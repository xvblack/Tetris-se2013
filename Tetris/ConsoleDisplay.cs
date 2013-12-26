using System;
using System.Diagnostics;
using Tetris.GameBase;

namespace Tetris
{
    /// <summary>
    /// 命令行显示
    /// </summary>
    public class ConsoleDisplay:IDisplay 
    {
        private Square[,] _image;
        public ConsoleDisplay()
        {
        }

        string GetChar(int color) // 根据颜色选择字符
        {
            if (color == -2)
            {
                return "N";
            }
            if (color == -1)
            {
                return " ";
            }
            if (color == 14)
            {
                return "C";
            }
            if (color>=0 && color < 10)
            {
                return "#";
            }
            if (color >= 10 && color < 20)
            {
                return "I";
            }
            if (color >= 20 && color < 30)
            {
                return "B";
            }
            {
                return "F";
            }
        }

        public void OnDrawing(TetrisGame game, TetrisGame.DrawEventArgs e) // 绘制屏幕
        {
            _image = game.Image;
            Console.Clear();
            Console.WriteLine(game.ScoreSystem.Score);
            Console.WriteLine(game.GameSpeed);
            Console.WriteLine("============");
            for (int i = 0; i < 15; i++)
            {
                Console.Write("|");
                for (int j = 0; j < 10; j++)
                    Console.Out.Write(_image[i, j] == null ? " " : GetChar(_image[i, j].NewSquare ? -2 : _image[i, j].Color));
                Console.Out.WriteLine("|");
            }
            Console.WriteLine("============");
            Debug.Assert(game.Factory is CacheFactory); // 显示下一个方块
            var factory = game.Factory as CacheFactory;
            Debug.Assert(factory.NextBlock()!=null);
            var block = factory.NextBlock();
            for (var i = block.Height - 1; i >= 0; i--)
            {
                Console.Write("|");
                for (var j = 0; j < block.Width; j++)
                {
                    Console.Write(GetChar(block.SquareAt(i, j)==null? -1: block.SquareAt(i, j).Color));
                }
                Console.WriteLine("|");
            }
        }
    }
}