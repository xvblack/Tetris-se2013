﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Tetris.GameBase
{
    public class TetrisFactory : ITetrisFactory // Block的工厂类
    {
        private readonly List<SquareArray> _styles; // 供选择的方块样式
        public TetrisGame Game { get; set; } // 绑定的游戏

        [Obsolete("please use the constructor without random")]
        public TetrisFactory(IEnumerable<SquareArray> styles, Random random=null){ // preserved
            _styles = new List<SquareArray>(styles);
        }

        public TetrisFactory(IEnumerable<SquareArray> styles)
        { // preserved
            _styles = new List<SquareArray>(styles);
        }

        public virtual Block GenTetris(){ // 生成方块
            int type = Randomor.Next(0, _styles.Count());
            var block = new Block(_styles[type]){RPos = Game.Width / 2 - 1};
            return block;
        }
    }
}