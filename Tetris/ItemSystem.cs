using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.GameBase;

namespace Tetris
{
    public class ItemBlock : Block
    {
        public void ResetDirection()
        {
            Direction = 0;
        }

        public bool Acted()
        {
            return Direction > 0;
        }
        public ItemBlock(SquareArray style, int blockId = -1)
            : base(style, blockId)
        {
        }
    }

    public class ItemSquare : Square
    {
        public int ItemId;
        public ItemSquare(int color,int itemId) : base(color)
        {
            ItemId = itemId;
        }
    }

    public class GunItemBlock : ItemBlock
    {
        public GunItemBlock(SquareArray style, int blockId = -1)
            : base(style, blockId)
        {
        }
    }

    public class InverseGunItemBlock : ItemBlock
    {
        public InverseGunItemBlock(SquareArray style, int blockId = -1)
            : base(style, blockId)
        {
        }
    }

    public class TonItemBlock : ItemBlock
    {
        public TonItemBlock(SquareArray style, int blockId = -1)
            : base(style, blockId)
        {

        }
    }
    public class TetrisItemFactory : TetrisFactory
    {
        public readonly Queue<Block> ItemQueue;
        private static readonly int[][,] styles = new int[][,] { new int[,] { { 10 } }, new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } } };
        SquareArray oneStyle = Square.Styles(styles).First();
        SquareArray threeStyle = Square.Styles(styles).ElementAt(1);
        Random _random=new Random();

        public TetrisItemFactory(IEnumerable<SquareArray> styles)
            : base(styles)
        {
            ItemQueue = new Queue<Block>();
        }

        public int rand(int max)
        {
            return _random.Next(max);
        }

        public override Block GenTetris()
        {
            //if ((new Random()).Next(10)>8) ItemQueue.Enqueue(new GunItemBlock(oneStyle));
            //if ((new Random()).Next(10) > 8) ItemQueue.Enqueue(new TonItemBlock(threeStyle));
            //if ((new Random()).Next(10) > 8) ItemQueue.Enqueue(new InverseGunItemBlock(oneStyle));
            if (ItemQueue.Count > 0)
                return ItemQueue.Dequeue();
            else
            {
                var block=base.GenTetris();
                if (rand(100) > 90)
                {
                    var i = rand(block.Height);
                    var j = 0;
                    while (block.SquareAt(i, j) == null)
                    {
                        j++;
                    }
                    block._style = block._style.Clone();
                    block._style[i, j] = new ItemSquare(10, rand(3));
                }
                return block;
            }
        }

        public void PushItem(Block b)
        {
            ItemQueue.Enqueue(b);
        }

        public void PushGun()
        {
            ItemQueue.Enqueue(new GunItemBlock(oneStyle));
        }

        public void PushInverseGun()
        {
            ItemQueue.Enqueue(new InverseGunItemBlock(oneStyle));
        }

        public void PushTon()
        {
            ItemQueue.Enqueue(new TonItemBlock(threeStyle));
        }
    }
    class ItemSystem
    {
        public static void Bind(TetrisGame game)
        {
            game.UpdateBeginEvent += ProcessItem;
            game.AddToUnderlyingEvent += ProcessUnderlyingItem;
            game.BeforeClearBarEvent += ProcessItemSquare;
        }

        private static void ProcessItemSquare(TetrisGame game, TetrisGame.ClearBarEventArgs e)
        {
            foreach (var s in e.Squares)
            {
                if (s is ItemSquare)
                {
                    var si = s as ItemSquare;
                    switch (si.ItemId)
                    {
                        case 0:
                            (game.Factory as TetrisItemFactory).PushGun();
                            break;
                        case 1:
                            (game.Factory as TetrisItemFactory).PushInverseGun();
                            break;
                        case 2:
                            (game.Factory as TetrisItemFactory).PushTon();
                            break;
                    }
                }
            }
        }

        public static void ProcessItem(TetrisGame game, TetrisGame.UpdateBeginEventArgs e)
        {
            if (game.Block is ItemBlock)
            {
                if (game.Block is GunItemBlock)
                {
                    var block = game.Block as GunItemBlock;
                    if (block.Acted())
                    {
                        var j = block.RPos;
                        for (var i = game.Height - 1; i >= 0; i--)
                        {
                            if (game.UnderLying[i, j] != null)
                            {
                                game.UnderLying[i, j] = null;
                                break;
                            }
                        }
                        block.ResetDirection();
                    }
                }
                if (game.Block is InverseGunItemBlock)
                {
                    var block=game.Block as InverseGunItemBlock;
                    if (block.Acted())
                    {
                        var j = block.RPos;
                        var i = game.Height - 1;
                        for (; i >= 0; i--)
                        {
                            if (game.UnderLying[i, j] != null)
                            {
                                
                                break;
                            }
                        }
                        game.UnderLying[i + 1, j] = new Square(1);
                        block.ResetDirection();
                    }

                }
            }
        }

        public static void ProcessUnderlyingItem(TetrisGame game, TetrisGame.AddToUnderlyingEventArgs e)
        {
            if (game.Block is TonItemBlock)
            {
                var j = game.Block.RPos;
                for (; j < e.block.RPos + e.block.Width; j++)
                {
                    for (var i = 0; i < game.Block.LPos; i++)
                    {
                        game.UnderLying[i, j] = null;
                    }
                }
                game.ClearBlock();
            }
            if (game.Block is ItemBlock)
            {
                game.ClearBlock();
            }
        }
    }
}
