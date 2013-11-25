using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.GameBase;
using System.Diagnostics;

namespace Tetris
{
    public class AIController : IController
    {
        private readonly TetrisGame _game;
        private int _id;
        private int _level;

        public AIController(TetrisGame game, int level = 0)
        {
            _game = game;
            _id = -1;
            _level = level;
        }

        private readonly Dictionary<TetrisGame.GameAction, int> _keyState = new Dictionary<TetrisGame.GameAction, int>()
        {
            {TetrisGame.GameAction.Left,0},
            {TetrisGame.GameAction.Right,0},
            {TetrisGame.GameAction.Rotate,0},
            {TetrisGame.GameAction.Down,0}
        };

        // Pierre Dellacherie Algorithm
        void GenerateControll()
        {
            // To controll the difficulty, we can random to make an error, or down times
            Block block = _game.Block.Clone();

            _keyState[TetrisGame.GameAction.Left] = 0;
            _keyState[TetrisGame.GameAction.Right] = 0;
            _keyState[TetrisGame.GameAction.Rotate] = 0;

            _keyState[TetrisGame.GameAction.Down] = _level * 5;

            int max_p = -1;           // Max position
            int max_r = -1;           // Max rotation
            int max_rating = -999999; // Max rating
            int max_prior = -1;       // Max prior
            for (int i = 0; i < _game.Width; i++) // Position
            {
                for (int j = 0; j < 4; j++)       // Rotation
                {
                    Block b = block.Clone().Rotate(j);
                    b.RPos = i;
                    //Trace.WriteLine(String.Format("---------------------------------------------------------"));
                    //Trace.WriteLine(String.Format("pos = {0}, rot = {1}", i, j));
                    int rating = calRating(b);
                    if (rating == -999999) continue;
                    int prior = calPrior(block, i, j);
                    if ((rating > max_rating) || ((rating == max_rating) && (prior > max_prior)))
                    {
                        max_rating = rating;
                        max_p = i;
                        max_r = j;
                        max_prior = prior;
                    }
                }
            }

            int mov = max_p - block.RPos;
            if (mov < 0)
            {
                _keyState[TetrisGame.GameAction.Left] = Math.Abs(mov);
            }
            else
            {
                _keyState[TetrisGame.GameAction.Right] = Math.Abs(mov);
            }
            _keyState[TetrisGame.GameAction.Rotate] = max_r;
        }
        private bool Intersect(Block block, SquareArray array)
        {
            if (block.LPos < 0) return true;
            for (int i = 0; i < block.Height; i++)
                for (int j = 0; j < block.Width; j++)
                {
                    if ((block.SquareAt(i, j) != null) && (array[block.LPos + i, block.RPos + j] != null))
                    {
                        return true;
                    }
                }
            return false;
        }
        private int min(int x, int y)
        {
            if (x < y)
                return x;
            return y;
        }
        private int calRating(Block block)
        {
            int h = _game.Height;
            int w = _game.Width;
            int i, j;
            for (i = 0; i < block.Height; i++)
                for (j = 0; j < block.Width; j++)
                {
                    if ((block.SquareAt(i, j) != null) && ((block.LPos + i < 0) || (block.RPos + j < 0) || (block.RPos + j >= w)))
                    {
                        return -999999;
                    }
                }

            SquareArray underlying = _game.UnderLying;
            SquareArray array = new SquareArray(h, w);
            Array.Copy(underlying.Storage, array.Storage, h * w);

            while (!Intersect(block.Fall(), underlying))
            {
                //Trace.WriteLine(String.Format("fall {0}", block.LPos));
            }
            block.LPos++;

            for (i = 0; i < block.Height; i++)
                for (j = 0; j < block.Width; j++)
                {
                    if (block.SquareAt(i, j) != null)
                    {
                        array[block.LPos + i, block.RPos + j] = block.SquareAt(i, j);
                    }
                }
            int landHeight = 0; // 落子距底部的方格数
            int metric = 0;     // 消去行数 * 当前落子被消去的格子数
            int rowTrans = 0;   // 各行变换次数之和
            int colTrans = 0;   // 各列变换次数之和
            int holes = 0;      // 各列空洞个数之和
            int wells = 0;      // 井深之和加1
            int line = 0;       // 消去的行数
            int bl = 0;         // 当前落子消去的block数
            for (i = h - 1; i >= 0; i--)
            {
                bool clear = true;
                for (j = 0; j < w; j++)
                {
                    if (array[i, j] == null)
                    {
                        clear = false;
                        break;
                    }
                }
                if (clear)
                {
                    line++;
                    for (j = 0; j < block.Width; j++)
                        if (block.SquareAt(i - block.LPos, j) != null)
                            bl++;
                    for (int s = i; s < h - 1; s++)
                        for (j = 0; j < w; j++)
                        {
                            array[s, j] = array[s + 1, j];
                        }
                    for (j = 0; j < w; j++)
                    {
                        array[h - 1, j] = null;
                    }
                }
            }
            metric = line * bl;
            landHeight = block.LPos - line;
            if (landHeight < 0) landHeight = 0;
            for (i = 0; i < h; i++)
            {
                bool pre = true;
                for (j = 0; j < w; j++)
                    if (array[i, j] != null)
                    {
                        pre = false;
                        break;
                    }
                if (pre) continue;
                pre = true;
                for (j = 0; j < w; j++)
                {
                    if (pre ^ (array[i, j] != null))
                    {
                        pre = !pre;
                        rowTrans++;
                    }
                }
                if (!pre)
                    rowTrans++;
            }
            int[] height = new int[w + 2];
            for (i = 0; i < w; i++)
            {
                bool pre = true;
                for (j = 0; j < h; j++)
                {
                    if (pre ^ (array[j, i] != null))
                    {
                        pre = !pre;
                        colTrans++;
                    }
                }
                if (!pre)
                    colTrans++;
                for (j = h - 1; j >= -1; j--)
                {
                    if (j == -1) break;
                    if (array[j, i] != null)
                        break;
                }
                height[i + 1] = j;
                for (j = j - 1; j >= 0; j--)
                {
                    if (array[j, i] == null)
                        holes++;
                }
            }
            height[0] = h + 1;
            height[w + 1] = h + 1;
            /*
            for (i = 0; i < w + 2; i++)
            {
                Trace.Write(height[i]);
                Trace.Write(" ");
            }
            Trace.WriteLine("");*/
            for (i = 0; i < w; i++)
            {
                j = min(height[i], height[i + 2]);
                if (j > height[i + 1])
                {
                    wells += (j - height[i + 1]);
                }
            }
            /*
            Trace.WriteLine(String.Format("height = {0}, line = {1}, block = {2}", landHeight, line, bl));
            Trace.WriteLine(String.Format("row = {0}, col = {1}", rowTrans, colTrans));
            Trace.WriteLine(String.Format("hole = {0}, well = {1}", holes, wells));*/
            

            return -landHeight + metric - rowTrans - colTrans - 4 * holes - wells;
        }
        private int calPrior(Block block, int pos, int rot)
        {
            int mov = pos - block.RPos;
            int result = 100 * Math.Abs(mov) + rot;
            if (mov < 0)
            {
                result += 10;
            }
            return result;
        }
        public bool Act(TetrisGame.GameAction action)
        {
            if (_game.Block == null)
                return false;
            bool result = false;
            if (_game.Block.Id != _id)
            {
                _id = _game.Block.Id;
                GenerateControll(); // May put it in a thread
            }
            if (_keyState[action] > 0)
            {
                result = true;
                _keyState[action]--;
            }
            
            return result;
        }
    }
}
