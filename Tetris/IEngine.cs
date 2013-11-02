using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public delegate void TickHandler(object sender, int tick);
    public interface IEngine
    {
        double Interval { set; }
        event TickHandler TickEvent;
        bool Enabled { get; set; }
    }
}
