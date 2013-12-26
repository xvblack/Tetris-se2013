using System;
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
    /// <summary>
    /// 道具方块类
    /// </summary>
    public abstract class ItemBlock : Block 
    {
        ///
        public void ResetDirection() // 重置方向
        {
            Direction = 0;
        } 
        public bool Acted() // 方向键是否按下过
        {
            return Direction > 0;
        } 
        protected ItemBlock(SquareArray style, int blockId = -1)
            : base(style, blockId)
        {
        }
    }

    public class ItemSquare : Square
    {
        static int GetColorByItemId(int itemId) // 按道具Id返回颜色
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
        public int ItemId; // 道具Id
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

    class ItemSystem
    {
        private int _nextSpeedUp = 20;
        public static void Bind(TetrisGame game)
        {
            var system = new ItemSystem();
            game.UpdateBeginEvent += system.ProcessItem;
            game.AddToUnderlyingEvent += system.ProcessUnderlyingItem;
            game.BeforeClearBarEvent += system.ProcessItemSquare;
            game.BeforeClearBarEvent += system.ProcessLine;
            game.UpdateBeginEvent += system.ProcessSpeedUp;
        }

        private void ProcessSpeedUp(TetrisGame game, TetrisGame.UpdateBeginEventArgs e) // 按得分加速
        {
            if (game.ScoreSystem.Score >= _nextSpeedUp)
            {
                game.GameSpeed++;
                _nextSpeedUp += 20;
            }
        }

        private void ProcessItemSquare(TetrisGame game, TetrisGame.ClearBarEventArgs e) // ClearBar时检查是否有道具Square被消除
        {
            foreach (var s in e.Squares)
            {
                if (s is ItemSquare)
                {
                    Debug.Assert(game.Factory is TetrisItemFactory);
                    var si = s as ItemSquare;
                    var f = game.Factory as TetrisItemFactory;
                    switch (si.ItemId)
                    {
                        case 0:
                            f.PushGun();
                            break;
                        case 1:
                            f.PushInverseGun();
                            break;
                        case 2:
                            f.PushTon();
                            break;
                        case 3:
                            Debug.Assert(game.IsDuelGame);
                            Console.WriteLine("Inversed Control");
                            game.DuelGame.Controller.InverseControl();
                            game.Later(10*TetrisGame.RoundTicks, () =>
                            {
                                game.DuelGame.Controller.InverseControl();
                                Console.WriteLine("Inversed Back");
                            });
                            break;
                    }
                }
            }
        }

        private void ProcessItem(TetrisGame game, TetrisGame.UpdateBeginEventArgs e) // 处理道具操作
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
                        var s = new Square(GameColor.InverseGunFillSquare);
                        game.UnderLying[i + 1, j] = s;
                        game.PushNewSquare(s);
                        block.ResetDirection();
                    }

                }
            }
        }

        private void ProcessUnderlyingItem(TetrisGame game, TetrisGame.AddToUnderlyingEventArgs e) // 处理道具下落时的效果
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

        private void ProcessLine(TetrisGame sender, TetrisGame.ClearBarEventArgs e) // 将消除的行加入另一边
        {
            if (sender.IsDuelGame)
            {
                var line = e.Squares.Clone() as Square[];
                for(int i=0;i<e.Squares.Length;i++)
                {
                    Debug.Assert(line != null, "line != null");
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
