using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.GameBase;

namespace Tetris
{
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
        public TetrisItemFactory(IEnumerable<SquareArray> styles)
            : base(styles)
        {
            ItemQueue = new Queue<Block>();
        }

        public override Block GenTetris()
        {
            int[][,] styles = { new int[,] { { 1 } }, new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } } };
            var oneStyle = Square.Styles(styles).First();
            var threeStyle = Square.Styles(styles).ElementAt(1);
            if ((new Random()).Next(10)>8) ItemQueue.Enqueue(new GunItemBlock(oneStyle));
            if ((new Random()).Next(10) > 8) ItemQueue.Enqueue(new TonItemBlock(threeStyle));
            if ((new Random()).Next(10) > 8) ItemQueue.Enqueue(new InverseGunItemBlock(oneStyle));
            if (ItemQueue.Count > 0)
                return ItemQueue.Dequeue();
            else
            {
                return base.GenTetris();
            }
        }

        public void PushItem(Block b)
        {
            ItemQueue.Enqueue(b);
        }
    }
    class ItemSystem
    {
        public static void Bind(TetrisGame game)
        {
            game.UpdateBeginEvent += ProcessItem;
            game.AddToUnderlyingEvent += ProcessUnderlyingItem;
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
        }
    }
}
