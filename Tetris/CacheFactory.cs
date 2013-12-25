using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.GameBase;

namespace Tetris
{
    internal class CacheFactory:TetrisItemFactory // 可以预览下一个方块的工厂
    {
        private Queue<Block> _blocks = new Queue<Block>(); // 方块缓存

        public CacheFactory(IEnumerable<SquareArray> styles, Random ran)
            : base(styles,ran)
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
