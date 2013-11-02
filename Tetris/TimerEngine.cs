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
        }

        event TickHandler IEngine.TickEvent
        {
            add
            {
                Elapsed += (sender, args) => value(sender, _tick++);
            }
            remove
            {
                throw new NotImplementedException();
            }
        }
    }
}
