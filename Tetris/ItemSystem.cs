using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using Tetris.GameBase;

namespace Tetris
{
    partial class GameColor
    {
        public const int GunSquare = 10;
        public const int InverseGunSquare = 11;
        public const int TonSquare = 12;
        public const int InverseControlSquare = 14;
        public const int GunBlock = 20;
        public const int InverseGunBlock = 21;
        public const int TonBlock = 22;
        public const int InverseGunFillSquare = 25;
        public const int BlockOne = 30;
        public const int BlockThree = 31;
        public const int BlockFive = 32;
    }
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
        static int GetColorByItemId(int itemId)
        {
            switch (itemId)
            {
                case 0:
                    return GameColor.GunSquare;
                case 1:
                    return GameColor.InverseGunSquare;
                case 2:
                    return GameColor.TonSquare;
                case 3:
                    return GameColor.InverseControlSquare;
            }
            return 0;
        }
        public int ItemId;
        public ItemSquare(int itemId):base(GetColorByItemId(itemId))
        {
            ItemId = itemId;
        }

        public override Square Clone()
        {
            return new ItemSquare(ItemId);
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
        private static readonly List<SquareArray> ItemStyles = new List<SquareArray>();
        private static readonly List<SquareArray> SpecialStyles =new List<SquareArray>();
        readonly Random _random;
        private int[] _itemIds;
        public bool GenSpecialBlock;

        static TetrisItemFactory()
        {
            ItemStyles.Add(Square.BuildStyle(1,1,GameColor.GunBlock));
            ItemStyles.Add(Square.BuildStyle(1, 1, GameColor.InverseGunBlock));
            ItemStyles.Add(Square.BuildStyle(3, 3, GameColor.TonBlock));

            SpecialStyles.Add(Square.BuildStyle(1,1,GameColor.BlockOne));
            SpecialStyles.Add(Square.BuildStyle(3, 3, GameColor.BlockThree));
            //SpecialStyles.Add(Square.BuildStyle(5, 5, GameColor.BlockFive));
        }

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
            return new ItemSquare(_itemIds[rand(_itemIds.Length)]);
            //return new ItemSquare(10, _itemIds[1]);
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
            ItemQueue.Enqueue(new GunItemBlock(ItemStyles[0]));
        }

        public void PushInverseGun()
        {
            ItemQueue.Enqueue(new InverseGunItemBlock(ItemStyles[1]));
        }

        public void PushTon()
        {
            ItemQueue.Enqueue(new TonItemBlock(ItemStyles[2]));
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
                    Debug.Assert(game.Factory is TetrisItemFactory);
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
                        game.UnderLying[i + 1, j] = new Square(GameColor.InverseGunFillSquare);
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
