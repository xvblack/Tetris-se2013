using System;
using System.ComponentModel;
using System.Timers;

namespace Tetris.GameBase
{
    [Obsolete]
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
        public int Fps { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
