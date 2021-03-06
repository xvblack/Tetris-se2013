﻿using System.Collections.Generic;
using System.Xml.Linq;
using Tetris.GameBase;

namespace Tetris.GameSystem
{
    public class AchievementSystem
    {

        public class AchievementState // 单个用户的成就状态
        {
            public static readonly List<string> HighestScoreUser=new List<string>();
            public static readonly List<string> HighestTotalClearBarUser=new List<string>();

            public static int HighestScoreGlobal=0;
            public static int HighestTotalClearBarGlobal=0;
            public int HighScore=0;
            public int TotalClearBar=0;
            public bool SeqClear=false;
            public bool HardSurvive = false;
            public bool HighestScore = false;
            public bool HighestTotalClearBar = false;
        }

        public static AchievementState GetAchievementState(string name = "")
        {
            return States[name];
        }

        public static bool ExistUser(string name)
        {
            return States.ContainsKey(name);
        }

        private abstract class BindSystem // 成就系统的绑定子系统
        {
            public abstract void Bind(TetrisGame game); // 将实例绑定到游戏
        }

        private class SeqClearBind:BindSystem // 连续消除三次两行的成就
        {
            private int state = 0;
            public SeqClearBind()
            {
            }
            public override void Bind(TetrisGame game)
            {
                if (game.AchievementState.SeqClear) return;
                game.UpdateEndEvent += delegate(TetrisGame sender, TetrisGame.UpdateEndEventArgs e)
                {
                    if (sender.TickClearedBars >= 2)
                    {
                        state++;
                        if (state == 3)
                        {
                            game.AchievementState.SeqClear = true;
                        }
                    }
                    else
                    {
                        state = 0;
                    }
                };
            }
        }
        private class HardSurviveBind : BindSystem // 困难模式存活30轮的成就
        {
            private int state = 0;
            public override void Bind(TetrisGame game)
            {
                if (game.AchievementState.HardSurvive) return;
                if (game.GameSpeed >= 3)
                {
                    state++;
                    if (state == 30)
                    {
                        game.AchievementState.HardSurvive = true;
                    }
                }
                else
                {
                    state = 0;
                }
            }
        } 

        private static readonly IDictionary<string, AchievementState> States=new Dictionary<string, AchievementState>(); // 全体成就状态

        public static void Bind(TetrisGame game, string name="") // 静态方法，绑定游戏
        {
            if (name == "") return;
            if (!States.ContainsKey(name)) // 如果没有现在的用户，设定为新用户
            {
                States[name]=new AchievementState();
            }
            game.AchievementState = States[name];
            game.GameEndEvent += new TetrisGame.GameEndCallback( // 更新最高分
                delegate(object sender, TetrisGame.GameEndEventArgs e)
                {
                    if (e.Score > game.AchievementState.HighScore)
                        game.AchievementState.HighScore = e.Score;
                }
                );
            game.UpdateEndEvent += new TetrisGame.UpdateEndCallback( // 更新总消除行数
                delegate(TetrisGame sender, TetrisGame.UpdateEndEventArgs e)
                {
                    game.AchievementState.TotalClearBar += game.TickClearedBars;
                    AchievementSystem.UpdateHighest();
                });
            new SeqClearBind().Bind(game); // 绑定子系统
            new HardSurviveBind().Bind(game);

        }

        /// 储存与载入
        /// 
        /// 使用XML存储在SavePath处

        private const string SavePath = ".\\save.xml"; // 成就存储位置

        public static void Save()
        {
            try
            {
                var doc = new XDocument();
                var users = new XElement("Users");
                foreach (var state in States)
                {
                    var elem = new XElement("User", new XAttribute("name", state.Key));
                    elem.Add(state.Value.ToXElement<AchievementState>());
                    users.Add(elem);
                }
                doc.Add(users);
                doc.Save(SavePath);

            }
            catch
            {
                
            }
        }

        public static void Load()
        {
            try
            {
                var doc = XDocument.Load(SavePath);
                var users = doc.Element("Users");
                foreach (var user in users.Elements("User"))
                {
                    var state = user.Element("AchievementState").FromXElement<AchievementState>();
                    States[user.Attribute("name").Value] = state;
                }
                UpdateHighest();

            }
            catch
            {
                
            }
        }

        public static void UpdateHighest() // 更新全体用户的最高分
        {
            AchievementState.HighestTotalClearBarUser.Clear();
            AchievementState.HighestScoreUser.Clear();
            int hs = 0, htcb = 0;
            foreach (var state in States)
            {
                if (state.Value.HighScore > hs)
                {
                    hs = state.Value.HighScore;
                }
                if (state.Value.TotalClearBar > htcb)
                {
                    htcb = state.Value.TotalClearBar;
                }
            }
            AchievementState.HighestScoreGlobal = hs;
            AchievementState.HighestTotalClearBarGlobal = htcb;
            foreach (var state in States)
            {
                if (state.Value.HighScore == hs)
                {
                    state.Value.HighestScore = true;
                    AchievementState.HighestScoreUser.Add(state.Key);
                }
                if (state.Value.TotalClearBar == htcb)
                {
                    state.Value.HighestTotalClearBar = true;
                    AchievementState.HighestTotalClearBarUser.Add(state.Key);
                }
            }
        }


    }
}
