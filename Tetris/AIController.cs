using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.GameBase;
using System.Diagnostics;
using System.Threading;

namespace Tetris
{
    public class AIController : IController
    {
        private readonly TetrisGame _game;
        private int _id;
        private int _speed;
        private bool _running = false;
        private bool _left = false;
        private readonly Random _random = new Random();
        private int _error;
        private int _countError;
        private int _count;
        public enum AIType { Low, Middle, High };
        public AIController(TetrisGame game, AIType type = AIType.High)
        {
            _game = game;
            _id = -1;
            int speed = 0;
            int error = 0;
            int count = 0;
            // speed: 0~15 (速度，0为完全不按加速)
            // error: 0~100 （每次犯错的概率）
            // count: >=0 （每几次才可能犯错一次，0为不犯错，1为每次都可能犯错）
            if (type == AIType.High)
            {
                speed = 15;
            }
            else if (type == AIType.Middle)
            {
                speed = 12;
                count = 2;
                error = 10;
            }
            else if (type == AIType.Low)
            {
                speed = 12;
                count = 1;
                error = 30;
            }
            _speed = speed;
            _error = error;
            _count = count;
            _countError = 0;
        }

        private readonly Dictionary<TetrisGame.GameAction, int> _keyState = new Dictionary<TetrisGame.GameAction, int>()
        {
            {TetrisGame.GameAction.Left,0},
            {TetrisGame.GameAction.Right,0},
            {TetrisGame.GameAction.Rotate,0},
            {TetrisGame.GameAction.Down,0}
        };

        // Pierre Dellacherie Algorithm
        private void GenerateControll()
        {
            _running = true;
            // To controll the difficulty, we can random to make an error, or down times
            if ((_game == null) || (_game.Block == null))
            {
                _running = false;
                return;
            }
            Block block = _game.Block.Clone();
            Console.WriteLine(_countError);
            if (_countError >= _count)
            {
                _countError = 0;
            }
            else
            {
                _countError++;
            }
            if ((_countError == 0) && (_random.Next(100) < _error))
            {
                _keyState[TetrisGame.GameAction.Left] = _random.Next(5) * _random.Next(2);
                _keyState[TetrisGame.GameAction.Right] = _random.Next(5) * _random.Next(2);
                _keyState[TetrisGame.GameAction.Rotate] = _random.Next(4);
                _keyState[TetrisGame.GameAction.Down] = 100;
            }
            else
            {
                if (_game.Block is TonItemBlock)
                {
                    int max_p = -1;           // Max position
                    int max_rating = -999999; // Max rating
                    int max_prior = -1;       // Max prior
                    for (int i = 0; i < _game.Width - _game.Block.Width + 1; i++) // Position
                    {
                        Block b = block.Clone();
                        b.RPos = i;
                        int rating = calTonRating(b);
                        if (rating == -999999) continue;
                        int prior = Math.Abs(i - _game.Width / 2 + 1);
                        if ((rating > max_rating) || ((rating == max_rating) && (prior > max_prior)))
                        {
                            max_rating = rating;
                            max_p = i;
                            max_prior = prior;
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
                    if (_keyState[TetrisGame.GameAction.Left] + _keyState[TetrisGame.GameAction.Right] == 0)
                        _keyState[TetrisGame.GameAction.Down] = _speed * 5;
                }
                else
                {
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
                    /*
                    _keyState[TetrisGame.GameAction.Left] = 0;
                    _keyState[TetrisGame.GameAction.Right] = 0;
                    _keyState[TetrisGame.GameAction.Rotate] = 0;*/
                    if (_keyState[TetrisGame.GameAction.Left] + _keyState[TetrisGame.GameAction.Right] + _keyState[TetrisGame.GameAction.Rotate] == 0)
                        _keyState[TetrisGame.GameAction.Down] = _speed * 5;
                }
            }
            _running = false;
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
        private int calTonRating(Block block)
        {
            int h = _game.Height;
            int w = _game.Width;
            int i, j;
            SquareArray underlying = _game.UnderLying;
            SquareArray array = new SquareArray(h, w);
            Array.Copy(underlying.Storage, array.Storage, h * w);

            int rowTrans = 0;   // 各行变换次数之和
            int colTrans = 0;   // 各列变换次数之和
            int holes = 0;      // 各列空洞个数之和
            int wells = 0;      // 井深之和加1
            int bl = 0;         // 当前落子消去的block数
            for (i = 0; i < h; i++)
            {
                for (j = 0; j < block.Width; j++)
                {
                    if (array[i, j + block.RPos] != null)
                    {
                        bl++;
                        array[i, j + block.RPos] = null;
                    }
                }
            }
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
            for (i = 0; i < w; i++)
            {
                j = min(height[i], height[i + 2]);
                if (j > height[i + 1])
                {
                    wells += (j - height[i + 1]);
                }
            }


            return bl - rowTrans - colTrans - 4 * holes - wells;
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
            for (i = 0; i < w; i++)
            {
                j = min(height[i], height[i + 2]);
                if (j > height[i + 1])
                {
                    wells += (j - height[i + 1]);
                }
            }
            

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
            if (action == TetrisGame.GameAction.Pause) return false;
            if (_game.Block == null)
                return false;
            if (_running)
                return false;
            bool result = false;
            if (_game.Block.Id != _id)
            {
                _keyState[TetrisGame.GameAction.Left] = 0;
                _keyState[TetrisGame.GameAction.Right] = 0;
                _keyState[TetrisGame.GameAction.Rotate] = 0;
                _keyState[TetrisGame.GameAction.Down] = 0;
            }
            if (_game.Block is InverseGunItemBlock)
            {
                if (action == TetrisGame.GameAction.Down)
                    return true;
                return false;
            }
            else if (_game.Block is GunItemBlock)
            {
                if (_id != _game.Block.Id)
                {
                    if (_countError >= _count)
                    {
                        _countError = 0;
                    }
                    else
                    {
                        _countError++;
                    }
                    if ((_countError == 0) && (_random.Next(100) < _error))
                    {
                        _keyState[TetrisGame.GameAction.Down] = 100;
                        return false;
                    }
                    _id = _game.Block.Id;
                    _keyState[TetrisGame.GameAction.Left] = 1;
                    _left = true;
                }
                else
                {
                    if (_keyState[action] > 0)
                    {
                        result = true;
                        _keyState[action]--;
                        if (_keyState[action] == 0)
                        {
                            if ((action == TetrisGame.GameAction.Left) || (action == TetrisGame.GameAction.Right))
                            {
                                _keyState[TetrisGame.GameAction.Rotate] = 1;
                                if (_game.Block.RPos <= 0)
                                    _left = false;
                                if (_game.Block.RPos >= _game.Width - 1)
                                    _left = true;
                            }
                            else if (action == TetrisGame.GameAction.Rotate)
                            {
                                if (_left)
                                    _keyState[TetrisGame.GameAction.Left] = 1;
                                else
                                    _keyState[TetrisGame.GameAction.Right] = 1;
                            }
                        }
                    }
                }
            }
            else
            {
                if (_game.Block.Id != _id)
                {
                    _id = _game.Block.Id;
                    Thread th = new Thread(new ThreadStart(GenerateControll));
                    th.IsBackground = true;
                    th.Start();
                }
                if (_keyState[action] > 0)
                {
                    result = true;
                    _keyState[action]--;
                    if ((action != TetrisGame.GameAction.Down) && (_keyState[TetrisGame.GameAction.Left]
                        + _keyState[TetrisGame.GameAction.Right] + _keyState[TetrisGame.GameAction.Rotate] == 0))
                        _keyState[TetrisGame.GameAction.Down] = _speed;
                }
            }
            return result;
        }

        public void InverseControl()
        {
        }
    }
}
