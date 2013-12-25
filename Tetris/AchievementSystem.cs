using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Tetris.GameBase;

namespace Tetris.GameSystem
{


    public static class XMLHelper
    {
        public static XElement ToXElement<T>(this object obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(streamWriter, obj);
                    return XElement.Parse(Encoding.ASCII.GetString(memoryStream.ToArray()));
                }
            }
        }

        public static T FromXElement<T>(this XElement xElement)
        {
            using (var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(xElement.ToString())))
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                return (T)xmlSerializer.Deserialize(memoryStream);
            }
        }
    }
    public class AchievementSystem
    {

        public class AchievementState
        {
            public int HighScore=0;
            public int TotalClearBar=0;
            public bool SeqClear=false;
            public bool HardSurvive = false;
            public bool HighestScore = false;
            public bool HighestTotalClearBar = false;
        }

        public abstract class BindSystem
        {
            public abstract void Bind(TetrisGame game);
        }

        public class SeqClearBind:BindSystem
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

        public class HardSurviveBind : BindSystem
        {
            private int state = 0;
            public override void Bind(TetrisGame game)
            {
                if (game.AchievementState.HardSurvive) return;
                if (game.GameSpeed >= 2)
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

        private static readonly IDictionary<string, AchievementState> States=new Dictionary<string, AchievementState>();

        public static void Bind(TetrisGame game, string name="")
        {
            if (!States.ContainsKey(name))
            {
                States[name]=new AchievementState();
            }
            game.AchievementState = States[name];
            game.GameEndEvent += new TetrisGame.GameEndCallback(
                delegate(object sender, TetrisGame.GameEndEventArgs e)
                {
                    if (e.Score > game.AchievementState.HighScore)
                        game.AchievementState.HighScore = e.Score;
                }
                );
            game.UpdateEndEvent += new TetrisGame.UpdateEndCallback(
                delegate(TetrisGame sender, TetrisGame.UpdateEndEventArgs e)
                {
                    game.AchievementState.TotalClearBar += game.TickClearedBars;
                    AchievementSystem.UpdateHighest();
                });
            new SeqClearBind().Bind(game);
            new HardSurviveBind().Bind(game);

        }

        private const string savePath = ".\\save.xml";
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
                doc.Save(savePath);

            }
            catch
            {
                
            }
        }

        public static void Load()
        {
            try
            {
                var doc = XDocument.Load(savePath);
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

        private static void UpdateHighest()
        {
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
            foreach (var state in States)
            {
                if (state.Value.HighScore == hs)
                {
                    state.Value.HighestScore = true;
                }
                if (state.Value.TotalClearBar == htcb)
                {
                    state.Value.HighestTotalClearBar = true;
                }
            }
        }


    }
}
