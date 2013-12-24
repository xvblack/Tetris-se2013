using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.GameBase;

namespace Tetris
{
    internal class CacheFactory:TetrisItemFactory
    {
        private Queue<Block> _blocks = new Queue<Block>();

        public CacheFactory(IEnumerable<SquareArray> styles, Random ran, bool isDuel = false)
            : base(styles,ran,isDuel)
        {
        }

        public void Init()
        {
            while (_blocks.Count < 2)
            {
                var block = base.GenTetris();
                _blocks.Enqueue(block);
           } 
        }

        public override Block GenTetris()
        {
            var block = base.GenTetris();
            if (block is ItemBlock)
            {
                return block;
            }
            else
            {
                _blocks.Enqueue(block);
                return _blocks.Dequeue();
            }
        }

        public Block NextBlock()
        {
            return _blocks.Peek();
        }

    }

}
