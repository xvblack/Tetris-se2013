using System;
using System.Collections.Generic;
using Tetris.GameBase;

namespace Tetris.GameSystem
{
    public class TetrisItemFactory : TetrisFactory
    {
        public readonly Queue<Block> ItemQueue;
        private static readonly List<SquareArray> ItemStyles = new List<SquareArray>();
        private static readonly List<SquareArray> SpecialStyles =new List<SquareArray>();
        readonly Random _random;
        private int[] _itemIds;
        public bool GenSpecialBlock;
        private bool _isDuel;

        public bool IsDuel
        {
            get { return _isDuel; }
            set
            {
                _isDuel = value;
                if (!_isDuel)
                {
                    _itemIds = new int[] { 0, 1, 2 };
                }
                else
                {
                    _itemIds = new int[] { 0, 1, 2, 3 };
                }
            }
        }

        static TetrisItemFactory()
        {
            ItemStyles.Add(Square.BuildStyle(1,1,GameColor.GunBlock));
            ItemStyles.Add(Square.BuildStyle(1, 1, GameColor.InverseGunBlock));
            ItemStyles.Add(Square.BuildStyle(3, 3, GameColor.TonBlock));

            SpecialStyles.Add(Square.BuildStyle(1,1,GameColor.BlockOne));
            SpecialStyles.Add(Square.BuildStyle(3, 3, GameColor.BlockThree));
            //SpecialStyles.Add(Square.BuildStyle(5, 5, GameColor.BlockFive));
        }

        public TetrisItemFactory(IEnumerable<SquareArray> styles,Random ran)
            : base(styles, ran)
        {
            ItemQueue = new Queue<Block>();
            GenSpecialBlock = false;
            _random = ran;
            IsDuel = false;
        }

        private int rand(int max)
        {
            return _random.Next(max);
        }

        private ItemSquare GenItemSquare()
        {
            //if (IsDuel) return new ItemSquare(_itemIds[3]);
            return new ItemSquare(_itemIds[rand(_itemIds.Length)]);
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
                    block.Style = block.Style.Clone();
                    block.Style[i, j] = GenItemSquare();
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
}