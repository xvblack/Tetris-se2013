using System;
using System.Diagnostics;
using Tetris.GameBase;

namespace Tetris
{
    public class ConsoleDisplay:IDisplay
    {
        private Square[,] _image;
        public ConsoleDisplay()
        {
        }

        string GetChar(int color)
        {
            if (color == -1)
            {
                return " ";
            }else
            if (color>=0 && color < 10)
            {
                return "#";
            }else if (color >= 10 && color < 20)
            {
                return "I";
            }else if (color >= 20 && color < 30)
            {
                return "B";
            }else
            {
                return "F";
            }
        }

        public void OnDrawing(TetrisGame game, TetrisGame.DrawEventArgs e)
        {
            _image = game.Image;
            Console.Clear();
            Console.WriteLine(game.ScoreSystem.Score);
            Console.WriteLine("============");
            for (int i = 0; i < 15; i++)
            {
                Console.Write("|");
                for (int j = 0; j < 10; j++)
                    Console.Out.Write(_image[i, j] == null ? " " : GetChar(_image[i,j].Color));
                Console.Out.WriteLine("|");
            }
            Console.WriteLine("============");
            Debug.Assert(game.Factory is CacheFactory);
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