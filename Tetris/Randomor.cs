using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Randomor
    {
        private static Random _random=new Random();

        public static int Next(int i, int j)
        {
            return _random.Next(i, j);
        }

        public static int Next(int i)
        {
            return _random.Next(i);
        }
    }
}
