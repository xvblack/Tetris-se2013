using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;


namespace Tetris.AdvancedGUI.Styles
{
    public class PlayersName
    {   
        static String[] _names = new String[2]{"sh1", "sh2"};
        const String path = ".\\settings.xml";

        static public String getName(int index)
        {
            
            Load();
            return(_names[index % 2]);
            
        }

        static public void setName(int index, String name)
        {
            _names[index % 2] = name;
            Save();
        }


        static public void Save()
        {
            if (File.Exists(path))
            {
                var doc = XDocument.Load(path);
                var user = doc.Element("LastUsers");
                user.Element("User1").Value = _names[0];
                user.Element("User2").Value = _names[1];
                doc.Save(path);
            }
            else 
            {
                var doc = new XDocument();
                var users = new XElement("LastUsers");
                for (int i = 0; i < 2; i++)
                {
                    var user = new XElement("User" + (i + 1).ToString());
                    user.Value = _names[0];
                    users.Add(user);
                }
                doc.Add(users);
                doc.Save(path);
            }
        }

        public static void Load()

        {
            if (File.Exists(path))
            {
                var doc = XDocument.Load(path);
                var user = doc.Element("LastUsers");
                _names[0] = user.Element("User1").Value;
                _names[1] = user.Element("User2").Value;
            }
        } 
    }
}
