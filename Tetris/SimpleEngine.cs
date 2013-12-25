﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tetris
{
    class SimpleEngine : IEngine
    {
        public double Interval { set; private get; }
        public event TickHandler TickEvent;
        private bool _enabled;
        private Thread thread;
        public bool Enabled {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
                if (_enabled)
                {
                    thread.Start();
                }
                else
                {
                    thread.Abort();
                }
            }
        }

        public SimpleEngine()
        {
            _enabled = false;
            Interval = 1;
            thread = new Thread(delegate()
            {
                while (true)
                {
                    if (_enabled)
                    {
                        TickEvent.Invoke(this, 0);
                    }
                    Thread.Sleep((int)(Interval * 1000));
                }
            });
            var time = DateTime.Now;
            TickEvent += (sender, tick) =>
            {
                var newtime = DateTime.Now;
                //Trace.WriteLine(newtime-time);
                time = newtime;
                Trace.Flush();
            };
        }
    }
}
