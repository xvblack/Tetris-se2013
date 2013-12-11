using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
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

        public override Square Clone()
        {
            return new ItemSquare(Color,ItemId);
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
        private static readonly List<SquareArray> ItemStyles = Square.Styles(new int[][,] { new int[,] { { 10 } }, new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } } });
        private static readonly List<SquareArray> SpecialStyles = Square.Styles(new int[][,] { new int[,] { { 11 } }, new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } }, new int[,] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }} });
        private SquareArray oneStyle = ItemStyles[0];
        private SquareArray threeStyle = ItemStyles[1];
        readonly Random _random;
        private int[] _itemIds;
        public bool GenSpecialBlock;

        public TetrisItemFactory(IEnumerable<SquareArray> styles,Random ran, bool isDuel=false)
            : base(styles, ran)
        {
            ItemQueue = new Queue<Block>();
            if (isDuel)
            {
                _itemIds=new int[]{0,1,2};
            }
            else
            {
                _itemIds=new int[]{0,1,2,3};
            }
            GenSpecialBlock = false;
            _random = ran;
        }

        private int rand(int max)
        {
            return _random.Next(max);
        }

        private ItemSquare GenItemSquare()
        {
            return new ItemSquare(10,_itemIds[rand(_itemIds.Length)]);
            //return new ItemSquare(10, _itemIds[0]);
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
                if (GenSpecialBlock&&rand(100) > 90)
                {
                    block=new Block(SpecialStyles[rand(SpecialStyles.Count)]);
                }
                if (rand(100) > 90)
                {
                    var i = rand(block.Height);
                    var j = 0;
                    while (block.SquareAt(i, j) == null)
                    {
                        j++;
                    }
                    block._style = block._style.Clone();
                    block._style[i, j] = GenItemSquare();
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
            game.BeforeClearBarEvent += ProcessLine;
            game.UpdateBeginEvent += ProcessSpeedUp;
        }

        private static void ProcessSpeedUp(TetrisGame game, TetrisGame.UpdateBeginEventArgs e)
        {
            if (game.ScoreSystem.Score>20) game.GameSpeed=2;
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
                        case 3:
                            Debug.Assert(game.IsDuelGame);
                            game.DuelGame.Controller.InverseControl();
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

        private static void ProcessLine(TetrisGame sender, TetrisGame.ClearBarEventArgs e)
        {
            if (sender.IsDuelGame)
            {
                var line = e.Squares.Clone() as Square[];
                for(int i=0;i<e.Squares.Length;i++)
                {
                    if (line[i].NewSquare)
                    {
                        line[i] = null;
                    }
                    if (line[i] is ItemSquare)
                    {
                        line[i]=new Square(1);
                    }
                }
                sender.DuelGame.PushLine(line);
            }
        }
    }

}
