using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Tetris
{
    class TimerEngine : Timer,IEngine
    {
        private int _tick;
        

        public TimerEngine()
        {
            _tick = 0;
            Elapsed += delegate(object sender, ElapsedEventArgs e)
            {
                lock (this)
                {
                    TickEvent.Invoke(this,_tick++);
                }
            };
        }

        public event TickHandler TickEvent;
    }
}
