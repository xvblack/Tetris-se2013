using System;
using System.Collections.Generic;
using Tetris.GameSystem;

namespace Tetris.GameBase
{
    /// <summary>
    /// 可以预览下一个方块的工厂
    /// </summary>
    internal class CacheFactory:TetrisItemFactory
    {
        private Queue<Block> _blocks = new Queue<Block>(); // 方块缓存

        public CacheFactory(IEnumerable<SquareArray> styles, Random ran)
            : base(styles,ran)
        {
        }

        public void Init() // 初始化
        {
            while (_blocks.Count < 2)
            {
                var block = base.GenTetris();
                _blocks.Enqueue(block);
           } 
        }

        public override Block GenTetris() // 生成方块，道具直接返回，其他缓存
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
